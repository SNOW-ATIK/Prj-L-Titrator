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

namespace L_Titrator_Alpha
{
    public class FluidicsControl
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
        }

        public enum StepProgress
        { 
            None,
            Start,
            Ing,
        }

        public delegate void RunEndDelegate();
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

        //private static MeasureInfo CurMeasInfo = new MeasureInfo();

        private static int Temp_RcpNo = 0;
        private static int Temp_IterationMax = 0;
        private static int Temp_IterationCount = 0;


        private static StepProgress StepState = StepProgress.None;
        private static HistoryObj HistoryObjCurrent;

        private class SeqAssist
        {
            public static int RecipeNo { get; private set; }
            public static int SeqNo { get; private set; }
            public static int StepNo { get; private set; }

            private static Stopwatch TimeCheck = new Stopwatch();
            public static bool IsTimerRunning { get { return TimeCheck.IsRunning; } }
            public static int TimeElapsed_ms { get { return (int)TimeCheck.ElapsedMilliseconds; } }

            public static void Init(int rcpNo)
            {
                RecipeNo = rcpNo;
                SeqNo = 0;
                StepNo = 0;
            }

            public static void Set_SeqStep(int seqNo, int stepNo)
            {
                SeqNo = seqNo;
                StepNo = stepNo;
            }

            public static void Start_Timer()
            {
                TimeCheck.Reset();
                TimeCheck.Start();
            }

            public static void Stop_Timer()
            {
                TimeCheck.Stop();
            }
        }

        private class TtrAssist
        {
            public static bool Flag_AnalyzingPoint = false;
            public static bool Flag_RefillSyringe = false;
            public static bool Flag_NotifyResultOnce = false;
            public static double RecentInjVolume_mL = 0;
            public static int TargetSyringePos_Digit = 0;

            public static HistoryObj.TitrationObj TtrObj;

            private static Stopwatch MixingStopWatch = new Stopwatch();
            public static bool IsMixing { get { return MixingStopWatch.IsRunning; } }
            public static int MixingTimeElapsed_ms { get { return (int)MixingStopWatch.ElapsedMilliseconds; } }

            public static void Init(HistoryObj.TitrationObj ttrObj)
            {
                TtrObj = ttrObj;
                Flag_AnalyzingPoint = false;
                Flag_RefillSyringe = false;
                Flag_NotifyResultOnce = false;
                RecentInjVolume_mL = 0;
                TargetSyringePos_Digit = 0;
                MixingStopWatch.Stop();
                MixingStopWatch.Reset();
            }

            public static void Start_Mixing()
            {
                MixingStopWatch.Reset();
                MixingStopWatch.Start();
            }

            public static void Stop_Mixing()
            {
                MixingStopWatch.Stop();
            }
        }

        public static bool Initialize()
        {
            thrSeq = new Thread(SequenceProcess) { IsBackground = true };
            thrSeq.Start();

            // Test
            MainState = FluidicsState.Idle;

            Log.Init_Log(PreDef.Path_Log, "Seq");
            Log.Init_Log(PreDef.Path_Log, "Result");

            return true;
        }

        public static void StartMeasure(int RcpNo, int IterationCnt)
        {
            Temp_RcpNo = RcpNo;
            Temp_IterationCount = 0;
            Temp_IterationMax = IterationCnt;
            qSeq.Enqueue(new object[] { FluidicsMsg.Measure, RcpNo });
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
                st_Loop.Reset();
                st_Loop.Start();

                Msg = FluidicsMsg.None;

                if (qSeq.TryDequeue(out object input) == true)
                {
                    object[] inputData = (object[])input;
                    Msg = (FluidicsMsg)inputData[0];
                    if (Msg == FluidicsMsg.Measure)
                    {
                        RcpNo = (int)inputData[1];

                        Log.WriteLog("Seq", $"[{MainState}/{RunState}] MsgRecv={Msg}, RcpNo={RcpNo}");
                    }
                    else
                    {
                        Log.WriteLog("Seq", $"[{MainState}/{RunState}] MsgRecv={Msg}");
                    }

                }

                switch (MainState)
                {
                    case FluidicsState.None:
                        if (Msg == FluidicsMsg.Initialize)
                        {
                            MainState = FluidicsState.Run;
                            RunState = FluidicsRunState.Initializing;
                        }
                        break;

                    case FluidicsState.Idle:
                        if (Msg == FluidicsMsg.Measure)
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
                            NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"Start Measure {Temp_IterationCount + 1}/{Temp_IterationMax}");

                            MainState = FluidicsState.Run;
                            RunState = FluidicsRunState.Measuring;
                            StepState = StepProgress.Start;
                        }
                        break;

                    case FluidicsState.Run:
                        switch (RunState)
                        {
                            case FluidicsRunState.Initializing:
                                InitProc();
                                break;

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
                        break;
                }

                if (Prv_MainState != MainState || Prv_RunState != RunState)
                {
                    Prv_MainState = MainState;
                    Prv_RunState = RunState;
                }

                if (Prv_SeqNo != SeqAssist.SeqNo || Prv_StepNo != SeqAssist.StepNo)
                {
                    if (RunState == FluidicsRunState.Measuring)
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

        private static void RunSequence(RecipeObj Cur_Recipe)
        {
            Sequence Cur_Seq = Cur_Recipe.Get_Sequence(SeqAssist.SeqNo);
            Step Cur_Step = Cur_Seq.Get_Step(SeqAssist.StepNo);

            Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Seq={Cur_Seq.No}:{Cur_Seq.Name}, Step={Cur_Step.No}:{Cur_Step.Name}:{Cur_Step.Enabled}");

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
                            Run_End();
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
                            Run_End();
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

                        NotifyProcessInfo?.Invoke(NotifyProcess.AddPoint, new object[] { TtrAssist.TtrObj.SampleName, TtrAssist.TtrObj.TotalInjectionVolume_mL, dReadAnalogVal });

                        Log.WriteLog("Result", $"> Read analog value after injecting Sample. (1st mV={dReadAnalogVal})");

                        injVol_Digit = (int)(TtrAssist.TtrObj.Offset_mL * SyringeElem.ScaleFactor);
                    }
                    else
                    {
                        if (dReadAnalogVal < TtrAssist.TtrObj.IncThr_ToMiddle)
                        {
                            injVol_Digit = (int)(TtrAssist.TtrObj.Inc_Large_mL * SyringeElem.ScaleFactor);
                        }
                        else if (dReadAnalogVal < TtrAssist.TtrObj.IncThr_ToSmall)
                        {
                            injVol_Digit = (int)(TtrAssist.TtrObj.Inc_Middle_mL * SyringeElem.ScaleFactor);
                        }
                        else
                        {
                            injVol_Digit = (int)(TtrAssist.TtrObj.Inc_Small_mL * SyringeElem.ScaleFactor);
                        }
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
                        int nRtn = syringeRtn.Volume_Raw;
                        int nTgt = TtrAssist.TargetSyringePos_Digit;
                        if (nRtn == nTgt)
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

                                        // Write Result
                                        TtrAssist.TtrObj.End_Inject(dtMixingEnd, TtrAssist.RecentInjVolume_mL, dReadAnalogVal);

                                        double dConcentration = TtrAssist.TtrObj.TotalInjectionVolume_mL * TtrAssist.TtrObj.ScaleFactor_VolumeToConcentration;

                                        Log.WriteLog("Result", $"> End Mixing (AnalogValue={dReadAnalogVal}mV, TotalInjectionVolume={TtrAssist.TtrObj.TotalInjectionVolume_mL}, Elapsed ms={TtrAssist.MixingTimeElapsed_ms}ms, Untill={mixingTime_ms}ms)");

                                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $"End Mixing, Read Result");
                                        NotifyProcessInfo?.Invoke(NotifyProcess.Log, $">>{AnalogElem.LogicalName}={dReadAnalogVal}mV, Injected(Total)={TtrAssist.TtrObj.TotalInjectionVolume_mL}mL");
                                        NotifyProcessInfo?.Invoke(NotifyProcess.AddPoint, new object[] { TtrAssist.TtrObj.SampleName, TtrAssist.TtrObj.TotalInjectionVolume_mL, dReadAnalogVal });

                                        // Analyze Result
                                        if (dReadAnalogVal > TtrAssist.TtrObj.AnalogValue_Target)
                                        {
                                            // Notify Once
                                            if (TtrAssist.Flag_NotifyResultOnce == false)
                                            {
                                                // #. TBD. 공급 설비 출력 추가

                                                

                                                NotifyProcessInfo?.Invoke(NotifyProcess.Result, new object[] { AnalogElem.LineNo, dConcentration, dReadAnalogVal, (double)0 });
                                                NotifyProcessInfo?.Invoke(NotifyProcess.TitrationEnd, dtMixingEnd);

                                                TtrAssist.Flag_NotifyResultOnce = true;
                                            }
                                        }

                                        // Reset Analyzing Flag after Mixing and Reading.
                                        TtrAssist.Flag_AnalyzingPoint = false;

                                        if (dReadAnalogVal < TtrAssist.TtrObj.AnalogValue_End)
                                        {
                                            if (TtrAssist.TtrObj.IterationCount < TtrAssist.TtrObj.MaxIterationCount)
                                            {

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
                    int maxVol = elem.MaxVolume_Raw;
                    int curVol_Abs = rtn.Volume_Raw;
                    int tgtVol_Abs = 0;
                    MB_SyringeFlow flow = syringe.Get_Flow();
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

                    elem.Run_Raw(syringe.Get_Flow(), syringe.Get_Direction(), injVol, syringe.Get_Speed(), false);

                    Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. Syringe : Name={syringe.Name}, SetCondition={syringe.Get_Direction()},{syringe.Get_Speed()},{injVol}(CurAbs={curVol_Abs / elem.ScaleFactor},Inj={injVol / elem.ScaleFactor},TgtAbs={tgtVol_Abs / elem.ScaleFactor},Condition={syringe.Get_Volume_mL()})");
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
                bool Done_TimeDelay = false;
                bool Done_PositionSync = false;
                bool Done_SensorDetect = false;

                if (step.StepEndCheck != null)
                {
                    // Check Time Delay
                    if (step.StepEndCheck.TimeDelay.Time > 0 && Done_TimeDelay == false)
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
                                Done_TimeDelay = true;

                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: End TimeDelay (Current={elapsedTime}ms, Target={step.StepEndCheck.TimeDelay.Time * 1000}ms)");
                            }
                        }
                    }
                    else
                    {
                        Done_TimeDelay = true;
                    }

                    // Check Position End (Syringe)
                    if (step.StepEndCheck.Get_PositionSync(out bool posSync) == true)
                    {
                        if (posSync == true)
                        {
                            //Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Check");

                            int PosDone = 0;
                            step.Syringes.ForEach(syringe =>
                            {
                                var elem = MB_Elem_Syringe.GetElem(syringe.Name);
                                if (elem.RunCmdDone == MB_Elem_Syringe.RunCmdStatus.Done)
                                {
                                    var rtn = elem.Get_Volume_Raw();
                                    if (rtn.IsValid == true)
                                    {
                                        // Compare Volume
                                        if (rtn.Volume_Raw == syringe.Get_TargetVolume_Raw())
                                        {
                                            ++PosDone;
                                        }
                                    }
                                    else
                                    {
                                        Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: #. Check Syringe (CmdVolume={syringe.Get_TargetVolume_Raw() / elem.ScaleFactor}, TgtVolume={syringe.Get_TargetVolume_mL()}, CurVolume={elem.Get_Volume_mL()}");
                                    }
                                }
                            });
                            if (PosDone == step.Syringes.Count)
                            {
                                Done_PositionSync = true;

                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Syringe Done");
                            }
                        }
                        else
                        {
                            Done_PositionSync = true;
                        }
                    }
                    else
                    {
                        Done_PositionSync = true;
                    }

                    // Check Sensor Detect
                    if (step.StepEndCheck.Get_Sensors(out List<string> sensors) == true)
                    {
                        if (sensors.Count > 0)
                        {
                            //Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Sensor Check");

                            int SensDone = 0;
                            sensors.ForEach(sensor =>
                            {
                                var elem = MB_Elem_Bit.GetElem(sensor);
                                if (elem.Get_State() == true)   // TBD. Activity 확인
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

                                Done_TimeDelay = true;
                                Done_SensorDetect = true;

                                Log.WriteLog("Seq", $"[{MainState}/{RunState}] Run. StepEndCheck: Sensor Done (DetectTime={SeqAssist.TimeElapsed_ms}ms)");
                            }
                        }
                        else
                        {
                            Done_SensorDetect = true;
                        }
                    }
                    else
                    {
                        Done_SensorDetect = true;
                    }
                }
                else
                {
                    Done_TimeDelay = true;
                    Done_PositionSync = true;
                    Done_SensorDetect = true;
                }

                if (Done_TimeDelay == true && Done_PositionSync == true && Done_SensorDetect == true)
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

        private static void Run_End()
        {
            MainState = FluidicsState.Idle;
            RunState = FluidicsRunState.None;

            DateTime seqEndTime = DateTime.Now;

            NotifyProcessInfo?.Invoke(NotifyProcess.ChangeSeqStep, new object[] { "None", "None" });
            NotifyProcessInfo?.Invoke(NotifyProcess.SequenceEnd, seqEndTime);
            NotifyProcessInfo?.Invoke(NotifyProcess.Log, "End Measure");

            HistoryObjCurrent.End_Sequence(seqEndTime);

            ++Temp_IterationCount;

            Log.WriteLog("Seq", $"[{MainState}/{RunState}] RunEnd. (Iteration {Temp_IterationCount}/{Temp_IterationMax})");

            if (Temp_IterationCount < Temp_IterationMax)
            {
                qSeq.Enqueue(new object[] { FluidicsMsg.Measure, Temp_RcpNo });
            }
            else
            {
                RunEnd?.Invoke();
            }
        }
    }
}
