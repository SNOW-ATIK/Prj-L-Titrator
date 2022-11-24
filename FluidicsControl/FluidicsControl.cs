using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using ATIK;
using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator
{
    public partial class FluidicsControl
    {
        public enum NotifyProcess
        { 
            Clear,
            Start,
            ChangeSeqStep,
            Log,
            AddPoint,
            Result,
            TitrationEnd,
            SequenceEnd,
            Error,
        }

        public enum StepProgress
        { 
            None,
            Start,
            Ing,
        }

        public enum StepEndCheck
        { 
            TimeDelay,
            SensorDetect,
            SyringeEnd
        }

        public delegate void RunEndDelegate(RunEndState runEndState);
        public static RunEndDelegate RunEnd;

        public delegate void NotifyProcessInfoDelegate(NotifyProcess notifyProcess, object data);
        public static NotifyProcessInfoDelegate NotifyProcessInfo;

        private const int LOOP_PERIOD = 200;

        private static FluidicsState Prv_MainState = FluidicsState.None;
        private static FluidicsRunState Prv_RunState = FluidicsRunState.None;
        public static FluidicsState MainState { get; private set; }
        public static FluidicsRunState RunState { get; private set; }

        public static ConcurrentQueue<object> qSeq = new ConcurrentQueue<object>();
        public static Thread thrSeq;

        private static int Glb_RcpNo = 0;
        private static int Glb_IterationMax = 0;
        private static int Glb_IterationCount = 0;
        private static double Glb_Concentration = 0;
        private static int Glb_VLD_Order = 0;

        private static StepProgress StepState = StepProgress.None;
        private static HistoryObj HistoryObjCurrent;

        private static Dictionary<StepEndCheck, bool> DicStepEndCheck = new Dictionary<StepEndCheck, bool>();

        public static bool Initialize()
        {
            thrSeq = new Thread(SequenceProcess) { IsBackground = true };
            thrSeq.Start();

            MainState = FluidicsState.None;

            Log.Init_Log(PreDef.Path_Log, "Seq");
            Log.Init_Log(PreDef.Path_Log, "Result");

            return true;
        }

        public static void StartMeasure(int RcpNo, int IterationCnt)
        {
            Glb_RcpNo = RcpNo;
            Glb_IterationCount = 0;
            Glb_IterationMax = IterationCnt;
            Glb_Concentration = 0;
            Glb_VLD_Order = 0;

            qSeq.Enqueue(new object[] { FluidicsMsg.Measure, RcpNo });
        }

        public static void StartHotKeyFunction(int RcpNo, int IterationCnt)
        {
            Glb_RcpNo = RcpNo;
            Glb_IterationCount = 0;
            Glb_IterationMax = IterationCnt;
            Glb_Concentration = 0;
            Glb_VLD_Order = 0;

            qSeq.Enqueue(new object[] { FluidicsMsg.HotKey, RcpNo });
        }

        public static void AbortMeasure()
        {
            qSeq.Enqueue(new object[] { FluidicsMsg.Abort });
        }

        public static void SequenceProcess()
        {
            Stopwatch st_Loop = new Stopwatch();
            FluidicsMsg Msg = FluidicsMsg.None;
            int RcpNo = 0;
            int Prv_SeqNo = -1;
            int Prv_StepNo = -1;

            NotifyProcessInfo?.Invoke(NotifyProcess.Clear, null);

            while (true)
            {
                // Check Leak and Overflow
                bool Flag_Alarm = false;
                if (IsLeak() == true || IsOverflow() == true)
                {
                    //Flag_Alarm = true;
                }

                // Reset Msg.
                Msg = FluidicsMsg.None;

                if (Flag_Alarm == false)
                {
                    st_Loop.Reset();
                    st_Loop.Start();

                    if (qSeq.TryDequeue(out object input) == true)
                    {
                        object[] inputData = (object[])input;
                        Msg = (FluidicsMsg)inputData[0];
                        if (Msg == FluidicsMsg.Measure || Msg == FluidicsMsg.HotKey)
                        {
                            RcpNo = (int)inputData[1];

                            Log.WriteLog("Seq", $"[{MainState}/{RunState}] MsgRecv={Msg}, RcpNo={RcpNo}");
                        }
                        else
                        {
                            Log.WriteLog("Seq", $"[{MainState}/{RunState}] MsgRecv={Msg}");
                        }
                    }
                }

                switch (MainState)
                {
                    case FluidicsState.None:
                        if (Msg == FluidicsMsg.HotKey)
                        {
                            if (LT_Recipe.Get_RecipeObj(RcpNo, out var RcpInfo) == true)
                            {
                                SeqAssist.Init(RcpNo);

                                NotifyProcessInfo?.Invoke(NotifyProcess.Clear, null);
                                NotifyProcessInfo?.Invoke(NotifyProcess.Start, new object[] { DateTime.Now, RcpNo });
                                NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Start {Msg}({RcpNo}:{RcpInfo.Name}) {Glb_IterationCount + 1}/{Glb_IterationMax}");

                                MainState = FluidicsState.Run;
                                RunState = FluidicsRunState.HotKey_Running;
                                StepState = StepProgress.Start;
                            }
                        }
                        break;

                    case FluidicsState.Idle:
                        if (Msg == FluidicsMsg.Measure || Msg == FluidicsMsg.HotKey)
                        {
                            if (LT_Recipe.Get_RecipeObj(RcpNo, out var RcpInfo) == true)
                            {
                                DateTime seqStartTime = DateTime.Now;
                                HistoryObjCurrent = new HistoryObj(seqStartTime, RcpNo);
                                if (HistoryObjCurrent.Init_Success == false)
                                {
                                    // #. Failed to load recipe
                                }

                                SeqAssist.Init(RcpNo);

                                NotifyProcessInfo?.Invoke(NotifyProcess.Clear, null);
                                NotifyProcessInfo?.Invoke(NotifyProcess.Start, new object[] { DateTime.Now, RcpNo });
                                NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Start {Msg}({RcpNo}:{RcpInfo.Name}) {Glb_IterationCount + 1}/{Glb_IterationMax}");

                                MainState = FluidicsState.Run;
                                if (Msg == FluidicsMsg.Measure)
                                {
                                    RunState = FluidicsRunState.Measuring;
                                }
                                else if (Msg == FluidicsMsg.HotKey)
                                {
                                    RunState = FluidicsRunState.HotKey_Running;
                                }
                                StepState = StepProgress.Start;
                            }
                        }
                        break;

                    case FluidicsState.Run:
                        if (Flag_Alarm == false)
                        {
                            if (Msg == FluidicsMsg.Abort)
                            {
                                AbortSeq();

                                Run_End(RunEndState.Abort);

                                MainState = FluidicsState.None;
                                RunState = FluidicsRunState.None;
                                StepState = StepProgress.None;
                            }
                            else
                            {
                                switch (RunState)
                                {
                                    case FluidicsRunState.HotKey_Running:
                                    case FluidicsRunState.Measuring:
                                        if (LT_Recipe.Get_RecipeObj(RcpNo, out RecipeObj Cur_Recipe) == true)
                                        {
                                            RunSequence(Cur_Recipe);
                                        }
                                        else
                                        {
                                            // #. TBD. Invalid
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Run_End(RunEndState.Alarm);

                            Thread.Sleep(LOOP_PERIOD);

                            continue;
                        }
                        break;

                    case FluidicsState.Error:
                        // Only HotKey Msg. is valid after handling error
                        if (Msg == FluidicsMsg.HotKey)
                        {
                            if (LT_Recipe.Get_RecipeObj(RcpNo, out var RcpInfo) == true)
                            {
                                SeqAssist.Init(RcpNo);

                                NotifyProcessInfo?.Invoke(NotifyProcess.Clear, null);
                                NotifyProcessInfo?.Invoke(NotifyProcess.Start, new object[] { DateTime.Now, RcpNo });
                                NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Start {Msg}({RcpNo}:{RcpInfo.Name}) {Glb_IterationCount + 1}/{Glb_IterationMax}");

                                MainState = FluidicsState.Run;
                                RunState = FluidicsRunState.HotKey_Running;
                                StepState = StepProgress.Start;
                            }
                        }
                        break;
                }

                if (Prv_MainState != MainState || Prv_RunState != RunState)
                {
                    Prv_MainState = MainState;
                    Prv_RunState = RunState;
                }

                if (Prv_SeqNo != SeqAssist.SeqNo || Prv_StepNo != SeqAssist.StepNo)
                {
                    if (RunState == FluidicsRunState.Measuring || RunState == FluidicsRunState.HotKey_Running)
                    {
                        var rtn = Get_SetStepName(RcpNo, SeqAssist.SeqNo, SeqAssist.StepNo);
                        NotifyProcessInfo?.Invoke(NotifyProcess.ChangeSeqStep, new object[] { rtn.SeqName, rtn.StepName });
                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"SeqName={rtn.SeqName}, StepName={rtn.StepName}");

                        Prv_SeqNo = SeqAssist.SeqNo;
                        Prv_StepNo = SeqAssist.StepNo;
                    }
                }


                st_Loop.Stop();
                int Extra = LOOP_PERIOD - (int)st_Loop.ElapsedMilliseconds;
                if (Extra >= 0)
                {
                    Thread.Sleep(Extra);
                }
                else
                {
                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Loop delay. ({Extra})");
                    Thread.Sleep(1);
                }
            }
        }

        private static void InitProc()
        { 
        }

        private static bool IsLeak()
        {
            var elem = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Leak_1);
            if (elem.Get_State() == false)
            {
                // Leak
                return true;
            }
            return false;
        }

        private static bool IsOverflow()
        {
            var elem = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Level_2);
            if (elem.Get_State() == false)
            {
                // Overflow
                return true;
            }
            return false;
        }

        private static void AbortSeq()
        {
            // Close supply valves
            MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_6Way).Set_State(false);
            MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_Vessel).Set_State(false);
            MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_Sample_6Way).Set_State(false);

            // TBD: Stop Syringe
            //MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Ceric_To_3Way).Set_State(false);    // TBD: Stop Syringe First

            // Stop Mixer
            MB_Elem_Analog.GetElem(PreDef.ElemLogicalName.AnalogOutput.Mixer_Duty).Set_Value(0);

            SeqAssist.Stop_Timer();
        }

        private static void RunSequence(RecipeObj Cur_Recipe)
        {
            Sequence Cur_Seq = Cur_Recipe.Get_Sequence(SeqAssist.SeqNo);
            Step Cur_Step = Cur_Seq.Get_Step(SeqAssist.StepNo);

            Log.WriteLog("Seq", $"[{MainState}/{RunState}/{StepState}] Run. Seq={Cur_Seq.No}:{Cur_Seq.Name}, Step={Cur_Step.No}:{Cur_Step.Name}:{Cur_Step.Enabled}");

            switch (StepState)
            {
                case StepProgress.None:
                    break;

                case StepProgress.Start:
                    if (Cur_Step.Enabled == true)
                    {
                        if (Cur_Step.IsTitration == true)
                        {
                            Init_Ttr(Cur_Step);
                        }
                        else
                        {
                            if (Cur_Step.Control_Valve == true)
                            {
                                Run_Valves(Cur_Step);
                            }
                            if (Cur_Step.Control_Syringe == true)
                            {
                                Run_Syringe(Cur_Step);
                            }
                            if (Cur_Step.Control_Mixer == true)
                            {
                                Run_Mixer(Cur_Step);
                            }

                            SeqAssist.Init_StepEndCheck();
                        }
                        StepState = StepProgress.Ing;
                    }
                    else
                    {
                        if (Check_NextProcExist(Cur_Recipe) == true)
                        {
                            StepState = StepProgress.Start;
                        }
                        else
                        {
                            Run_End(RunEndState.Success);
                            StepState = StepProgress.None;
                        }
                    }
                    break;

                case StepProgress.Ing:
                    if (Check_StepEnd(Cur_Step) == true)
                    {
                        if (Check_NextProcExist(Cur_Recipe) == true)
                        {
                            StepState = StepProgress.Start;
                        }
                        else
                        {
                            Run_End(RunEndState.Success);
                            StepState = StepProgress.None;
                        }
                    }
                    break;
            }
        }

        private static void RunStep()
        { 
        }

        // TBD. 
        // Load Info from Recipe File,
        // Set Info into HistoryObj.
        private static void Init_Ttr(Step curStep)
        {
            DateTime ttrStartTime = DateTime.Now;
            TitrationRef titrationRef = LT_Recipe.Load_TitrationRef(curStep);
            if (HistoryObjCurrent.Start_Titration(titrationRef.SampleName, ttrStartTime, (TitrationRef)titrationRef.Clone()) == true)
            {
                if (HistoryObjCurrent.Get_TitrationObj(titrationRef.SampleName, out HistoryObj.TitrationObj ttrObj) == true)
                {
                    TtrAssist.Init(ttrObj);

                    Log.WriteLog("Result", $"=== StartTitration (Step:{curStep.Name}) =========================================");
                }
            }
            else
            {
                Log.WriteLog("Seq", $@"#. Fail to load TitraionRef file. ({curStep.TitrationRefFileName})");
            }
        }

        private static bool Titrate(Step curStep)
        {
            var SyringeElem = MB_Elem_Syringe.GetElem(TtrAssist.TtrObj.ReagentSyringeLogicalName);
            var AnalogElem = MB_Elem_Analog.GetElem(TtrAssist.TtrObj.AnalogInputLogicalName);
            double dReadAnalogVal = AnalogElem.Get_Value();

            if (TtrAssist.Flag_AnalyzingPoint == false)
            {
                var rtn = SyringeElem.Get_Volume_Raw();
                if (rtn.IsValid == true)
                {
                    // Apply Offset
                    MB_SyringeFlow flow = MB_SyringeFlow.Dispense;
                    MB_SyringeDirection dir = MB_SyringeDirection.Out;
                    int speed = 15;
                    int maxVol_Digit = SyringeElem.MaxVolume_Raw;
                    int curVol_Abs_Digit = rtn.Volume_Raw;
                    int injVol_Digit = 0;
                    if (TtrAssist.TtrObj.IterationCount == 0)
                    {
                        // Write Analog value before Starting (1st mV)
                        TtrAssist.TtrObj.End_Inject(DateTime.Now, 0, dReadAnalogVal);

                        // TBD. Already exceed threshold
                        //if (dReadAnalogVal > TtrAssist.TtrObj.AnalogValue_Target)
                        //{
                        //    return true;
                        //}

                        NotifyProcessInfo?.Invoke(NotifyProcess.AddPoint, new object[] { TtrAssist.TtrObj.SampleName, TtrAssist.TtrObj.TotalInjectionVolume_mL, dReadAnalogVal });

                        Log.WriteLog("Result", $"> Read analog value after injecting Sample. (1st mV={dReadAnalogVal})");

                        injVol_Digit = (int)(TtrAssist.TtrObj.Offset_mL * SyringeElem.ScaleFactor);
                    }
                    else
                    {
                        // 20221120. 이전 주입에 대한 분석 직후 읽은 값으로 다음 주입량을 판단해야되서 삭제함.
                        //if (dReadAnalogVal < TtrAssist.TtrObj.IncThr_ToMiddle)
                        //{
                        //    injVol_Digit = (int)(TtrAssist.TtrObj.Inc_Large_mL * SyringeElem.ScaleFactor);
                        //}
                        //else if (dReadAnalogVal < TtrAssist.TtrObj.IncThr_ToSmall)
                        //{
                        //    injVol_Digit = (int)(TtrAssist.TtrObj.Inc_Middle_mL * SyringeElem.ScaleFactor);
                        //}
                        //else
                        //{
                        //    injVol_Digit = (int)(TtrAssist.TtrObj.Inc_Small_mL * SyringeElem.ScaleFactor);
                        //}
                        injVol_Digit = (int)(TtrAssist.NextInjectionVol * SyringeElem.ScaleFactor);
                    }

                    int tgtVol_Abs_Digit = curVol_Abs_Digit - injVol_Digit;
                    if (tgtVol_Abs_Digit < 0)
                    {
                        // Fill Syringe (Pick)
                        speed = 10;
                        tgtVol_Abs_Digit = maxVol_Digit;
                        injVol_Digit = maxVol_Digit - curVol_Abs_Digit;
                        flow = MB_SyringeFlow.Pick;
                        dir = MB_SyringeDirection.In;

                        TtrAssist.Flag_RefillSyringe = true;

                        Log.WriteLog("Result", $"> Refill Start (CurVolume={curVol_Abs_Digit / SyringeElem.ScaleFactor}mL, RefillVolume={injVol_Digit / SyringeElem.ScaleFactor}mL)");

                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Refill Syringe Start.");
                    }
                    else
                    {
                        // Normal
                        //MeasInfo.AddInjectionVolume(injVol_Digit / SyringeElem.ScaleFactor);
                        TtrAssist.RecentInjVolume_mL = injVol_Digit / SyringeElem.ScaleFactor;

                        Log.WriteLog("Result", $"> Inject Start (CurVolume={curVol_Abs_Digit / SyringeElem.ScaleFactor}mL, InjectVolume={TtrAssist.RecentInjVolume_mL}mL)");

                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Inject Start. {injVol_Digit / SyringeElem.ScaleFactor}mL");
                    }

                    TtrAssist.TargetSyringePos_Digit = tgtVol_Abs_Digit;
                    TtrAssist.Flag_AnalyzingPoint = true;

                    SyringeElem.Run_Raw(flow, dir, injVol_Digit, speed, false);
                }
            }
            else
            {
                //Log.WriteLog("Result", $"> Check Syringe Status (RunCmdDone={SyringeElem.RunCmdDone})");
                if (SyringeElem.RunCmdDone == MB_Elem_Syringe.RunCmdStatus.Done)
                {
                    var syringeRtn = SyringeElem.Get_Volume_Raw();
                    //Log.WriteLog("Result", $"> Check Volume Validity (IsValid={syringeRtn.IsValid}, Volume={syringeRtn.Volume_mL}mL)");
                    if (syringeRtn.IsValid == true)
                    {
                        bool bPEnd = false;
                        int nRtn = syringeRtn.Volume_Raw;
                        int nTgt = TtrAssist.TargetSyringePos_Digit;
                        bool bCompare = Util.PEnd_Raw(SyringeElem.LogicalName, nRtn, nTgt, SyringeElem.PositionEndBandwidth, SyringeElem.ScaleFactor);
                        if (bCompare == true)
                        {
                            ++SeqAssist.PEndCheckCnt;
                            if (SeqAssist.PEndCheckCnt == SeqAssist.PEndCheckMax)
                            {
                                SeqAssist.PEndCheckCnt = 0;
                                bPEnd = true;
                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Done, Complete.");
                            }
                            else
                            {
                                if (nRtn != nTgt)
                                {
                                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: {SyringeElem.LogicalName}> Current={nRtn}, Target={nTgt}");
                                }

                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Done, Wait {SeqAssist.PEndCheckCnt}/{SeqAssist.PEndCheckMax} to check position again.");
                            }
                        }

                        if (bPEnd == true)
                        {
                            if (TtrAssist.Flag_RefillSyringe == true)
                            {
                                // Reset Refill Flag
                                TtrAssist.Flag_RefillSyringe = false;

                                Log.WriteLog("Result", $"> Refill End (CurVolume={syringeRtn.Volume_Raw / SyringeElem.ScaleFactor}mL)");

                                NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Refill Syringe End.");
                            }
                            else
                            {
                                if (TtrAssist.IsMixing == false)
                                {   // Start Mixing
                                    // Normal
                                    TtrAssist.Start_Mixing();

                                    int mixingTime_ms = TtrAssist.TtrObj.MixingTime_General_ms * 1000;
                                    if (TtrAssist.TtrObj.IterationCount - 1 == 0)   // Except 1st mV
                                    {
                                        mixingTime_ms = TtrAssist.TtrObj.MixingTime_AfterOffset_ms * 1000;
                                    }

                                    NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Inject End, Start Mixing.");

                                    Log.WriteLog("Result", $"> Inject End (CurVolume={syringeRtn.Volume_Raw / SyringeElem.ScaleFactor}mL)");
                                }
                                else
                                {   // Mixing
                                    int mixingTime_ms = TtrAssist.TtrObj.MixingTime_General_ms * 1000;
                                    if (TtrAssist.TtrObj.IterationCount - 1 == 0)   // Except 1st mV
                                    {
                                        mixingTime_ms = TtrAssist.TtrObj.MixingTime_AfterOffset_ms * 1000;
                                    }

                                    if (TtrAssist.MixingTimeElapsed_ms < mixingTime_ms)
                                    {
                                        // Wait Mixing 

                                        Log.WriteLog("Result", $"> Mixing (AnalogValue={dReadAnalogVal}mV, Elapsed ms={TtrAssist.MixingTimeElapsed_ms}ms, Untill={mixingTime_ms}ms)");
                                    }
                                    else
                                    {
                                        DateTime dtMixingEnd = DateTime.Now;

                                        // End Mixing
                                        TtrAssist.Stop_Mixing();
                                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"End Mixing, Read Result");

                                        // Analyze Result
                                        double dConcentration = 0;
                                        double dIntepolated_Inj_Single = 0;
                                        if (dReadAnalogVal > TtrAssist.TtrObj.AnalogValue_Target)
                                        {
                                            // Notify Once
                                            if (TtrAssist.Flag_NotifyResultOnce == false)
                                            {
                                                // #. TBD. 공급 설비 출력 추가


                                                double dResultForNotifying = dReadAnalogVal;
                                                if (TtrAssist.TtrObj.InterpolationEnabled == true && TtrAssist.TtrObj.Get_LastPoint(out var injPrv) == true)
                                                {
                                                    double pX1 = injPrv.InjVol_Accum;
                                                    double pY1 = injPrv.Analog;
                                                    double pX2 = injPrv.InjVol_Accum + TtrAssist.RecentInjVolume_mL;
                                                    double pY2 = dReadAnalogVal;
                                                    double interpolated_Inj_Accum = Util.GetInterpolatedValue(TtrAssist.TtrObj.AnalogValue_Target, pX1, pY1, pX2, pY2);
                                                    Log.WriteLog("Debug", $"Interpolate> Target={TtrAssist.TtrObj.AnalogValue_Target}, pX1={pX1}, pY1={pY1}, pX2={pX2}, pY2={pY2}, Result={interpolated_Inj_Accum}");

                                                    dConcentration = interpolated_Inj_Accum * TtrAssist.TtrObj.ScaleFactor_VolumeToConcentration;
                                                    Log.WriteLog("Debug", $"Interpolate> ScaleFactor={TtrAssist.TtrObj.ScaleFactor_VolumeToConcentration}, Concentration={dConcentration}");

                                                    dResultForNotifying = TtrAssist.TtrObj.AnalogValue_Target;

                                                    // 기준 전압(800mV)에 대한 주입량을 역산하여 History에 추가함
                                                    dIntepolated_Inj_Single = interpolated_Inj_Accum - pX1;
                                                    Log.WriteLog("Debug", $"Interpolate> Inj_Single={dIntepolated_Inj_Single}");

                                                    TtrAssist.TtrObj.End_Inject(dtMixingEnd, dIntepolated_Inj_Single, TtrAssist.TtrObj.AnalogValue_Target);

                                                    NotifyProcessInfo?.Invoke(NotifyProcess.AddPoint, new object[] { TtrAssist.TtrObj.SampleName, interpolated_Inj_Accum, TtrAssist.TtrObj.AnalogValue_Target });

                                                    Log.WriteLog("Result", $"> Analyze (AnalogValue={TtrAssist.TtrObj.AnalogValue_Target}mV, TotalInjectionVolume={interpolated_Inj_Accum}, Elapsed ms={TtrAssist.MixingTimeElapsed_ms}ms, Untill={mixingTime_ms}ms)");
                                                }
                                                else
                                                {
                                                    // 주의: TtrAssist.TtrObj.End_Inject가 호출되지 않았으므로
                                                    // TtrAssist.TtrObj.TotalInjectionVolume_mL(총 주입량)가 아직 업데이트되지 않음. 따라서 TtrAssist.RecentInjVolume_mL(최근 주입량)을 더해준다.
                                                    dConcentration = (TtrAssist.TtrObj.TotalInjectionVolume_mL + TtrAssist.RecentInjVolume_mL) * TtrAssist.TtrObj.ScaleFactor_VolumeToConcentration;
                                                }

                                                NotifyProcessInfo?.Invoke(NotifyProcess.Result, new object[] { AnalogElem.LineNo, dConcentration, dResultForNotifying, (double)0 });

                                                Glb_Concentration = dConcentration;
                                                TtrAssist.Flag_NotifyResultOnce = true;
                                            }
                                        }

                                        NotifyProcessInfo?.Invoke(NotifyProcess.TitrationEnd, dtMixingEnd);

                                        // Write Result
                                        if (dIntepolated_Inj_Single > 0)
                                        {
                                            // Interpolate 한 경우, 주입량의 중복 합산을 피하기 위해, 
                                            // 기준 값을 환산하기 위해 역산한 양만큼을 뺀 양을 주입한 것으로 간주한다.
                                            // (TtrAssist.TtrObj.End_Inject가 호출되었으므로 연산한 만큼 바로 빼주면 된다)
                                            double inj_Single = TtrAssist.RecentInjVolume_mL - dIntepolated_Inj_Single;
                                            TtrAssist.TtrObj.End_Inject(dtMixingEnd, inj_Single, dReadAnalogVal);
                                        }
                                        else
                                        {
                                            TtrAssist.TtrObj.End_Inject(dtMixingEnd, TtrAssist.RecentInjVolume_mL, dReadAnalogVal);
                                        }

                                        Log.WriteLog("Result", $"> Analyze (AnalogValue={dReadAnalogVal}mV, TotalInjectionVolume={TtrAssist.TtrObj.TotalInjectionVolume_mL}, Elapsed ms={TtrAssist.MixingTimeElapsed_ms}ms, Untill={mixingTime_ms}ms)");

                                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $">>{AnalogElem.LogicalName}={dReadAnalogVal}mV, Injected(Total)={TtrAssist.TtrObj.TotalInjectionVolume_mL}mL");
                                        NotifyProcessInfo?.Invoke(NotifyProcess.AddPoint, new object[] { TtrAssist.TtrObj.SampleName, TtrAssist.TtrObj.TotalInjectionVolume_mL, dReadAnalogVal });

                                        // Reset Analyzing Flag after Mixing and Reading.
                                        TtrAssist.Flag_AnalyzingPoint = false;

                                        if (dReadAnalogVal < TtrAssist.TtrObj.AnalogValue_End)
                                        {
                                            if (TtrAssist.TtrObj.IterationCount < TtrAssist.TtrObj.MaxIterationCount)
                                            {
                                                // 20221120 다음 주입량 판단
                                                if (dReadAnalogVal < TtrAssist.TtrObj.IncThr_ToMiddle)
                                                {
                                                    TtrAssist.NextInjectionVol = TtrAssist.TtrObj.Inc_Large_mL;
                                                }
                                                else if (dReadAnalogVal < TtrAssist.TtrObj.IncThr_ToSmall)
                                                {
                                                    TtrAssist.NextInjectionVol = TtrAssist.TtrObj.Inc_Middle_mL;
                                                }
                                                else
                                                {
                                                    TtrAssist.NextInjectionVol = TtrAssist.TtrObj.Inc_Small_mL;
                                                }

                                                Log.WriteLog("Result", $"> Continue (Result={dReadAnalogVal}mV, Count={TtrAssist.TtrObj.IterationCount}/{TtrAssist.TtrObj.MaxIterationCount})");
                                            }
                                            else
                                            {
                                                // #. Iteration Count Over
                                                Log.WriteLog("Result", $"> Time Over (Result={dReadAnalogVal}mV, Count={TtrAssist.TtrObj.IterationCount}/{TtrAssist.TtrObj.MaxIterationCount})");

                                                DateTime ttrEndTime = DateTime.Now;
                                                HistoryObjCurrent.End_Titration(TtrAssist.TtrObj.SampleName, ttrEndTime);

                                                return true;
                                            }
                                        }
                                        else
                                        {
                                            // End
                                            Log.WriteLog("Result", $"> Successfully Done (Result={dReadAnalogVal}mV, Count={TtrAssist.TtrObj.IterationCount}/{TtrAssist.TtrObj.MaxIterationCount})");

                                            DateTime ttrEndTime = DateTime.Now;
                                            HistoryObjCurrent.End_Titration(TtrAssist.TtrObj.SampleName, ttrEndTime);

                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Log.WriteLog("Result", $"> #. Compare Result (SameAsTarget(Digital)={nRtn == nTgt}, IntRtn={nRtn}, IntTgt={nTgt})");
                        }
                    }
                    else
                    {
                        Log.WriteLog("Result", $"> #. Check Volume Validity (IsValid={syringeRtn.IsValid})");
                    }
                }
                else
                {
                    Log.WriteLog("Result", $"> Syringe is running.");
                }
            }

            return false;
        }

        private static (string SeqName, string StepName) Get_SetStepName(int rcpNo, int seqNo, int stepNo)
        {
            if (LT_Recipe.Get_RecipeObj(rcpNo, out RecipeObj curRecipe) == false)
            {
                return ("", "");
            }
            Sequence curSeq = curRecipe.Get_Sequence(seqNo);
            Step curStep = curSeq.Get_Step(stepNo);

            return (curSeq.Name, curStep.Name);
        }

        private static void Run_Valves(Step step)
        {
            step.Valves.ForEach(valve =>
            {
                var elem = MB_Elem_Bit.GetElem(valve.Name);

                if (elem.Get_State() != valve.Get_Condition())
                {
                    LT_LifeTime.IncreaseCount(elem.LogicalName);
                }

                elem.Set_State(valve.Get_Condition());

                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Valve : Name={valve.Name}, SetCondition={valve.Get_Condition()}");
            });
        }

        private static void Run_Syringe(Step step)
        {
            step.Syringes.ForEach(syringe =>
            {
                var elem = MB_Elem_Syringe.GetElem(syringe.Name);
                var rtn = elem.Get_Volume_Raw();
                if (rtn.IsValid == true)
                {
                    //double tgt_Vol_mL = syringe.Get_Volume_mL();
                    //int tgt_Vol_Raw = (int)(tgt_Vol_mL * elem.ScaleFactor);
                    //bool bCompare = Util.PEnd_Raw(rtn.Volume_Raw, tgt_Vol_Raw, elem.PositionEndBandwidth, elem.ScaleFactor);
                    //if (bCompare == true)
                    //{
                    //    syringe.Set_TargetVolume_Raw(tgt_Vol_Raw);
                    //    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Syringe : Name={syringe.Name}, No need to run. (Cur={rtn.Volume_Raw}, Tgt={tgt_Vol_Raw})");
                    //}
                    //else
                    //{
                    //}

                    int maxVol = elem.MaxVolume_Raw;
                    int curVol_Abs = rtn.Volume_Raw;
                    int tgtVol_Abs = 0;
                    MB_SyringeFlow flow = syringe.Get_Flow();
                    if (flow == MB_SyringeFlow.None)
                    {
                    }
                    else
                    {
                        int injVol = (int)(syringe.Get_Volume_mL() * elem.ScaleFactor);
                        switch (flow)
                        {
                            case MB_SyringeFlow.Pick:
                                tgtVol_Abs = curVol_Abs + injVol;
                                // 채우려는 양이 Max를 넘으면, Max까지 채울 양으로 치환
                                if (tgtVol_Abs > maxVol)
                                {
                                    injVol = maxVol - curVol_Abs;
                                    tgtVol_Abs = maxVol;
                                }
                                break;

                            case MB_SyringeFlow.Dispense:
                                tgtVol_Abs = curVol_Abs - injVol;
                                // 뱉는 양이 0보다 작으면, 0을 뱉을 양(현재 채워져 있는 양)으로 치환 
                                if (tgtVol_Abs < 0)
                                {
                                    injVol = curVol_Abs;
                                    tgtVol_Abs = 0;
                                }
                                break;
                        }
                        syringe.Set_TargetVolume_Raw(tgtVol_Abs);

                        bool bCompare = Util.PEnd_Raw(elem.LogicalName, rtn.Volume_Raw, tgtVol_Abs, elem.PositionEndBandwidth, elem.ScaleFactor);
                        if (bCompare == true)
                        {
                            Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Syringe : Name={syringe.Name}, No need to run. (Cur={rtn.Volume_Raw}, Tgt={tgtVol_Abs})");
                        }
                        else
                        {
                            LT_LifeTime.IncreaseCount(elem.LogicalName);

                            elem.Run_Raw(syringe.Get_Flow(), syringe.Get_Direction(), injVol, syringe.Get_Speed(), false);

                            Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Syringe : Name={syringe.Name}, SetCondition={syringe.Get_Direction()},{syringe.Get_Speed()},{injVol}(CurAbs={curVol_Abs / elem.ScaleFactor},Inj={injVol / elem.ScaleFactor},TgtAbs={tgtVol_Abs / elem.ScaleFactor},Condition={syringe.Get_Volume_mL()})");
                        }
                    }
                }
                else
                { 
                }                
            });
        }

        private static void Run_Mixer(Step step)
        {
            step.Mixers.ForEach(mixer =>
            {
                var elem = MB_Elem_Analog.GetElem(mixer.Name);
                elem.Set_Value(mixer.Get_Duty());

                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Mixer : Name={mixer.Name}, SetCondition={mixer.Get_Duty()}");
            });
        }

        private static bool Check_StepEnd(Step step)
        {
            if (step.IsTitration == true)
            {
                return Titrate(step);
            }
            else
            {
                if (step.StepEndCheck != null)
                {
                    // Check Time Delay
                    if (SeqAssist.IsDone_StepEndCheck(StepEndCheck.TimeDelay) == false)
                    {
                        if (step.StepEndCheck.TimeDelay.Time > 0)
                        {
                            //Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: TimeDelay Check");

                            if (SeqAssist.IsTimerRunning == false)
                            {
                                SeqAssist.Start_Timer();

                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Start TimeDelay (Target={step.StepEndCheck.TimeDelay.Time * 1000}ms)");
                            }
                            else
                            {
                                int elapsedTime = SeqAssist.TimeElapsed_ms;
                                if (elapsedTime >= step.StepEndCheck.TimeDelay.Time * 1000)
                                {
                                    SeqAssist.Stop_Timer();
                                    SeqAssist.Set_StepEndCheck(StepEndCheck.TimeDelay, true);

                                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: End TimeDelay (Current={elapsedTime}ms, Target={step.StepEndCheck.TimeDelay.Time * 1000}ms)");
                                }
                            }
                        }
                        else
                        {
                            SeqAssist.Set_StepEndCheck(StepEndCheck.TimeDelay, true);
                        }
                    }

                    // Check Position End (Syringe)
                    if (SeqAssist.IsDone_StepEndCheck(StepEndCheck.SyringeEnd) == false)
                    {
                        if (step.StepEndCheck.Get_PositionSync(out bool posSync) == true)
                        {
                            if (posSync == true)
                            {
                                //Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Check");

                                int PosDone = 0;
                                step.Syringes.ForEach(syringe =>
                                {
                                    if (syringe.Get_Flow() == MB_SyringeFlow.None)
                                    {
                                        PosDone++;
                                    }
                                    else
                                    {
                                        var elem = MB_Elem_Syringe.GetElem(syringe.Name);
                                        if (elem.RunCmdDone == MB_Elem_Syringe.RunCmdStatus.Done)
                                        {
                                            var rtn = elem.Get_Volume_Raw();
                                            if (rtn.IsValid == true)
                                            {
                                                // Compare Volume with PEndBandwidth
                                                bool PEnd = Util.PEnd_Raw(elem.LogicalName, rtn.Volume_Raw, syringe.Get_TargetVolume_Raw(), elem.PositionEndBandwidth, elem.ScaleFactor);
                                                if (PEnd == true)
                                                {
                                                    ++PosDone;
                                                }
                                                else
                                                {
                                                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Maybe moving (CmdVolume={syringe.Get_TargetVolume_Raw() / elem.ScaleFactor}, TgtVolume={syringe.Get_TargetVolume_mL()}, CurVolume={elem.Get_Volume_mL()}");
                                                }
                                                //if (rtn.Volume_Raw == syringe.Get_TargetVolume_Raw())
                                                //{
                                                //    ++PosDone;
                                                //}
                                            }
                                            else
                                            {
                                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: #. Check Syringe (CmdVolume={syringe.Get_TargetVolume_Raw() / elem.ScaleFactor}, TgtVolume={syringe.Get_TargetVolume_mL()}, CurVolume={elem.Get_Volume_mL()}");
                                            }
                                        }
                                    }
                                });
                                if (PosDone == step.Syringes.Count)
                                {
                                    ++SeqAssist.PEndCheckCnt;
                                    if (SeqAssist.PEndCheckCnt == SeqAssist.PEndCheckMax)
                                    {
                                        SeqAssist.PEndCheckCnt = 0;
                                        SeqAssist.Set_StepEndCheck(StepEndCheck.SyringeEnd, true);

                                        Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Done, Complete.");
                                    }
                                    else
                                    {
                                        step.Syringes.ForEach(syringe =>
                                        {
                                            if (syringe.Get_Flow() != MB_SyringeFlow.None)
                                            {
                                                var elem = MB_Elem_Syringe.GetElem(syringe.Name);
                                                var rtn = elem.Get_Volume_Raw();
                                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: {elem.LogicalName}> Current={rtn.Volume_Raw}, Target={syringe.Get_TargetVolume_Raw()}");
                                                if (rtn.Volume_Raw != syringe.Get_TargetVolume_Raw())
                                                {
                                                }
                                                else
                                                {
                                                }
                                            }
                                            else
                                            {
                                            }
                                        });

                                        Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Done, Wait {SeqAssist.PEndCheckCnt}/{SeqAssist.PEndCheckMax} to check position again.");
                                    }
                                }
                            }
                            else
                            {
                                SeqAssist.Set_StepEndCheck(StepEndCheck.SyringeEnd, true);
                            }
                        }
                        else
                        {
                            SeqAssist.Set_StepEndCheck(StepEndCheck.SyringeEnd, true);
                        }
                    }

                    // Check Sensor Detect
                    if (SeqAssist.IsDone_StepEndCheck(StepEndCheck.SensorDetect) == false)
                    {
                        if (step.StepEndCheck.Get_Sensors(out List<string> sensors) == true)
                        {
                            if (sensors.Count > 0)
                            {
                                //Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Sensor Check");

                                int SensDone = 0;
                                sensors.ForEach(sensor =>
                                {
                                    var elem = MB_Elem_Bit.GetElem(sensor);
                                    if (elem.Get_State() == false)   // Activity 확인> 감지 시 OFF. 즉 감지되면 다음 동작 수행
                                    {
                                        ++SensDone;
                                    }
                                    else
                                    {
                                        // Sensor를 감지하는 경우는 항상 TimeOut을 활성화 한다.
                                        int elapsedTime = SeqAssist.TimeElapsed_ms;
                                        if (elapsedTime >= step.StepEndCheck.TimeDelay.Time * 1000)
                                        {
                                            SeqAssist.Stop_Timer();

                                            Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: #. Check Sensor (SensorName={sensor}, Current={elapsedTime}ms, Target={step.StepEndCheck.TimeDelay.Time * 1000}ms)");
                                        }
                                    }
                                });
                                if (SensDone == sensors.Count)
                                {
                                    // TimeOut (정상 종료)
                                    SeqAssist.Stop_Timer();

                                    SeqAssist.Set_StepEndCheck(StepEndCheck.TimeDelay, true);
                                    SeqAssist.Set_StepEndCheck(StepEndCheck.SensorDetect, true);

                                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Sensor Done (DetectTime={SeqAssist.TimeElapsed_ms}ms)");
                                }
                            }
                            else
                            {
                                SeqAssist.Set_StepEndCheck(StepEndCheck.SensorDetect, true);
                            }
                        }
                        else
                        {
                            SeqAssist.Set_StepEndCheck(StepEndCheck.SensorDetect, true);
                        }
                    }
                }
                else
                {
                    SeqAssist.Set_StepEndCheck(StepEndCheck.TimeDelay, true);
                    SeqAssist.Set_StepEndCheck(StepEndCheck.SyringeEnd, true);
                    SeqAssist.Set_StepEndCheck(StepEndCheck.SensorDetect, true);
                }

                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Check All: TimeDelay={SeqAssist.IsDone_StepEndCheck(StepEndCheck.TimeDelay)}, SensorDetect={SeqAssist.IsDone_StepEndCheck(StepEndCheck.SensorDetect)}, SyringeEnd={SeqAssist.IsDone_StepEndCheck(StepEndCheck.SyringeEnd)}");
                if (SeqAssist.AllDone_StepEndCheck() == true)
                {
                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: All Done");

                    return true;
                }
            }

            return false;
        }

        private static bool Check_NextProcExist(RecipeObj curRecipe)
        {
            // 현재 Seq 안에 다음 Step이 있는지 확인
            Sequence curSeq = curRecipe.Get_Sequence(SeqAssist.SeqNo);
            Step nextStep = curSeq.Get_Step(SeqAssist.StepNo + 1);
            if (nextStep != null)
            {
                SeqAssist.Set_SeqStep(SeqAssist.SeqNo, SeqAssist.StepNo + 1);
                //CurMeasInfo.Set_StepProgress(StepProgress.Start);

                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. NextProc: Seq={curSeq.No}:{curSeq.Name}, Step={nextStep.No}:{nextStep.Name}");

                return true;
            }
            else
            {
                // 현재 Seq 안에 다음 Step이 없다면,
                // 다음 Seq가 있는지 확인
                Sequence nextSeq = curRecipe.Get_Sequence(SeqAssist.SeqNo + 1);
                if (nextSeq != null)
                {
                    // 다음 Seq가 있다면,
                    // 0번 Step이 있는지 확인
                    nextStep = curRecipe.Get_Sequence(SeqAssist.SeqNo + 1).Get_Step(0);
                    if (nextStep != null)
                    {
                        SeqAssist.Set_SeqStep(SeqAssist.SeqNo + 1, 0);
                        //CurMeasInfo.Set_StepProgress(StepProgress.Start);

                        Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. NextProc: Seq={nextSeq.No}:{nextSeq.Name}, Step={nextStep.No}:{nextStep.Name}");

                        return true;
                    }
                }
            }

            Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. NextProc: No more next process.");

            return false;
        }

        private static void Run_End(RunEndState runEndState)
        {
            switch (runEndState)
            {
                case RunEndState.Success:
                    if (Glb_RcpNo > (int)LT_Recipe.HotKeyRecipeNo.Initialize)
                    {
                        MainState = FluidicsState.None;
                    }
                    else
                    {
                        MainState = FluidicsState.Idle;
                    }
                    break;

                case RunEndState.Abort:
                    MainState = FluidicsState.None;
                    break;

                case RunEndState.Alarm:
                    MainState = FluidicsState.Error;
                    break;
            }
            RunState = FluidicsRunState.None;

            DateTime seqEndTime = DateTime.Now;

            NotifyProcessInfo?.Invoke(NotifyProcess.ChangeSeqStep, new object[] { "None", "None" });
            NotifyProcessInfo?.Invoke(NotifyProcess.SequenceEnd, seqEndTime);
            NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"End Sequence ({runEndState})");

            HistoryObjCurrent?.End_Sequence(seqEndTime);

            if (runEndState == RunEndState.Success)
            {
                if (LT_Config.GenPrm_Validation_Enabled.Value == true)
                {
                    bool bIsResultOK = IsResultOK();
                    if (bIsResultOK == true)
                    {
                        // Result=OK -> End
                        RunEnd?.Invoke(runEndState);
                    }
                    else
                    {
                        // Result=NG -> VLD
                        if (VLD_Continue() == true)
                        {
                            qSeq.Enqueue(new object[] { FluidicsMsg.Measure, Glb_RcpNo });

                            Log.WriteLog("Seq", $"[{MainState}/{RunState}] VLD Continue. (Iteration {Glb_IterationCount}/{Glb_IterationMax})");
                        }
                        else
                        {
                            RunEnd?.Invoke(runEndState);
                        }
                    }
                }
                else
                {
                    ++Glb_IterationCount;

                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] RunEnd. (Iteration {Glb_IterationCount}/{Glb_IterationMax})");

                    if (Glb_IterationCount < Glb_IterationMax)
                    {
                        if (Glb_RcpNo < LT_Recipe.RecipeMaxCount)
                        {
                            qSeq.Enqueue(new object[] { FluidicsMsg.Measure, Glb_RcpNo });
                        }
                        else
                        {
                            qSeq.Enqueue(new object[] { FluidicsMsg.HotKey, Glb_RcpNo });
                        }
                    }
                    else
                    {
                        RunEnd?.Invoke(runEndState);
                    }
                }                
            }
            else
            {
                RunEnd?.Invoke(runEndState);

                Log.WriteLog("Seq", $"[{MainState}/{RunState}] RunCancel. (Iteration {Glb_IterationCount}/{Glb_IterationMax})");
            }
        }
    }
}
