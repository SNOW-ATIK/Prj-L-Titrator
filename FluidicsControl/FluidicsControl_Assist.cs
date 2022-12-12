using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace L_Titrator
{
    public partial class FluidicsControl
    {
        private class SeqAssist
        {
            public static int RecipeNo { get; private set; }
            public static int SeqNo { get; private set; }
            public static int StepNo { get; private set; }

            public static bool VLD_Enabled { get; private set; }

            private static Stopwatch TimeCheck = new Stopwatch();
            public static bool IsTimerRunning { get { return TimeCheck.IsRunning; } }
            public static int TimeElapsed_ms { get { return (int)TimeCheck.ElapsedMilliseconds; } }

            public const int PEndCheckMax = 3;
            public static int PEndCheckCnt = 0;

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
                if (TimeCheck.IsRunning == true)
                {
                    TimeCheck.Stop();
                    TimeCheck.Reset();
                }
            }

            public static void Init_StepEndCheck()
            {
                DicStepEndCheck.Clear();
                DicStepEndCheck.Add(StepEndCheck.TimeDelay, false);
                DicStepEndCheck.Add(StepEndCheck.SensorDetect, false);
                DicStepEndCheck.Add(StepEndCheck.SyringeEnd, false);
            }

            public static void Set_StepEndCheck(StepEndCheck stepEnd, bool done)
            {
                DicStepEndCheck[stepEnd] = done;
            }

            public static bool IsDone_StepEndCheck(StepEndCheck stepEnd)
            {
                return DicStepEndCheck[stepEnd];
            }

            public static bool AllDone_StepEndCheck()
            {
                return DicStepEndCheck.ContainsValue(false) == false;
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

            public static double NextInjectionVol = 0;

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

        public static bool IsResultOK()
        {
            bool isOk = false;

            var vldCfg = LT_Config.ValidationList[Glb_VLD_Order];
            if (Glb_RcpNo == vldCfg.RecipeNo.Value)
            {
                if (vldCfg.Enabled.Value == true)
                {
                    var analyzeList = LT_Recipe.Get_AllAnalyzeRef(Glb_RcpNo);
                    if (analyzeList != null && analyzeList.Count == 1)
                    {
                        AnalyzeRef analyzeRef = analyzeList[0];
                        switch (analyzeRef.Type)
                        {
                            case AnalyzeType.pH:
                            case AnalyzeType.ORP:
                                ValidationInfo vldInfo = analyzeRef.TtrRef.ValidationInfo;
                                isOk = (Glb_Concentration >= vldInfo.Limit_Low && Glb_Concentration <= vldInfo.Limit_High);
                                break;

                            case AnalyzeType.ISE:
                                break;
                        }
                    }
                }
                else
                {
                    // Validation is disabled. No need to operate validations.
                    isOk = true;
                }
            }

            return isOk;
        }

        public static bool VLD_Continue()
        {
            bool vld_Continue = false;

            var vldCfg = LT_Config.ValidationList[Glb_VLD_Order];
            if (Glb_RcpNo == vldCfg.RecipeNo.Value)
            {
                if (vldCfg.Enabled.Value == true)
                {
                    if (Glb_IterationCount < vldCfg.RepeatCounts.Value)
                    {
                        // Repeat Current Recipe
                        ++Glb_IterationCount;
                        vld_Continue = true;
                    }
                    else
                    {
                        int chkIdx = Glb_VLD_Order + 1; // 다음 운용할 Recipe 정보 탐색을 위한 확인용 Index 증가.
                        if (chkIdx < LT_Config.MaxValidations)
                        {
                            var vldNext = LT_Config.ValidationList[chkIdx];

                            if (vldNext.Enabled.Value == true)
                            {
                                // Check VLD Recipe Existvar analyzeList = LT_Recipe.Get_AllAnalyzeRef(Glb_RcpNo);
                                var analyzeList = LT_Recipe.Get_AllAnalyzeRef(vldNext.RecipeNo.Value);
                                if (analyzeList != null && analyzeList.Count == 1)
                                {
                                    AnalyzeRef analyzeRef = analyzeList[0];
                                    switch (analyzeRef.Type)
                                    {
                                        case AnalyzeType.pH:
                                        case AnalyzeType.ORP:
                                            // Start Next Validation Recipe
                                            Glb_RcpNo = LT_Config.ValidationList[chkIdx].RecipeNo.Value;
                                            Glb_IterationCount = 0;
                                            vld_Continue = true;
                                            break;

                                        case AnalyzeType.ISE:
                                            break;
                                    }
                                }
                                else
                                {
                                    // Config에 설정한 Recipe 번호에 해당하는 Recipe를 불러올 수 없음.
                                }
                            }
                            else
                            {
                                // 다음 운용할 Recipe에 대한 VLD가 활성화 되어있지 않으면 종료.
                                // ex> 0:Enabled, 1:Disabled, 2:Enabled 일 경우, 
                                //     0번 Recipe 운용한 이후, 1번 Recipe가 Disable이면 2번 Recipe가 Enabled 상태여도 2번 Recipe로 넘어가지 않고 종료.
                            }
                        }
                        else
                        { 
                            // 다음 VLD Recipe가 없으므로 종료.
                        }
                    }
                }
                else
                {
                    // 직전 운용한 Recipe에 대한 VLD가 설정되어있지 않으면 다음 VLD를 수행하지 않는다.
                }
            }

            return vld_Continue;
        }
    }
}
