using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ATIK;

namespace L_Titrator
{
    public partial class HistoryObj
    {
        public enum ObjHandleType
        { 
            Unknown,
            Created,
            Opened
        }

        public class TitrationObj : IAnalyzeObj
        {
            public ObjHandleType HandleType { get; private set; }
            public int MyAnalyzeOrder { get; set; }
            public AnalyzeType MyAnalyzeType { get; set; }

            private HistoryObj MyParent;
            private string MySection;
            private TitrationRef MyRef;
            private List<InjectedObj> InjectedList = new List<InjectedObj>();

            public string SampleName
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.SampleName;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return MyParent.File_GetValue(MySection, Titration_Key.SampleName);
                            }
                            break;
                    }
                    return "";
                }
            }
            public string ReagentName
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.ReagentName;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return MyParent.File_GetValue(MySection, Titration_Key.ReagentName);
                            }
                            break;
                    }
                    return "";
                }
            }
            public string ReagentSyringeLogicalName
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.ReagentSyringeLogicalName;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return MyParent.File_GetValue(MySection, Titration_Key.Syringe_LogicalName);
                            }
                            break;
                    }
                    return "";
                }
            }
            public string AnalogInputLogicalName
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.AnalogInfo.AnalogInputLogicalName;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return MyParent.File_GetValue(MySection, Titration_Key.AnalogInput_LogicalName);
                            }
                            break;
                    }
                    return "";
                }
            }
            public int MaxIterationCount
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.MaxIterationCount;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return int.Parse(MyParent.File_GetValue(MySection, Titration_Key.MaxIterationCount));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double Offset_mL
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.Offset;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.Offset_mL));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public int MixingTime_AfterOffset_ms
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.MixingTime_AfterOffset;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return int.Parse(MyParent.File_GetValue(MySection, Titration_Key.MixingTime_AfterOffset_ms));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public int MixingTime_General_ms
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.MixingTime_General;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return int.Parse(MyParent.File_GetValue(MySection, Titration_Key.MixingTime_General_ms));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double ScaleFactor_VolumeToConcentration
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.ScaleFactor;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.ScaleFactor_VolumeToConcentration));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double AnalogValue_Target
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.AnalogInfo.TargetValue;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.TitrationTarget_mV));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double AnalogValue_End
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.AnalogInfo.EndValue;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.TitrationEnd_mV));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double IncThr_ToMiddle
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.IncThreshold_ChangeToMiddle;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.InjThr_LargeToMiddle));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double IncThr_ToSmall
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.IncThreshold_ChangeToSmall;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.InjThr_MiddleToSmall));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double Inc_Large_mL
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.Inc_Large;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.InjVol_Large_mL));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double Inc_Middle_mL
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.Inc_Middle;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.InjVol_Middle_mL));
                            }
                            break;
                    }
                    return -1;
                }
            }
            public double Inc_Small_mL
            {
                get
                {
                    switch (HandleType)
                    {
                        case ObjHandleType.Created:
                            if (MyRef != null)
                            {
                                return MyRef.InjectionInfo.Inc_Small;
                            }
                            break;

                        case ObjHandleType.Opened:
                            if (MyParent != null)
                            {
                                return double.Parse(MyParent.File_GetValue(MySection, Titration_Key.InjVol_Small_mL));
                            }
                            break;
                    }
                    return -1;
                }
            }

            public DateTime StartTime_Titration { get; private set; }
            public DateTime EndTime_Titration { get; private set; }
            public TimeSpan Duration_Titration { get; private set; }
            public int IterationCount
            {
                get
                {
                    if (MyRef != null)
                    {
                        return InjectedList.Count;
                    }
                    if (MyParent != null)
                    {
                        return int.Parse(MyParent.File_GetValue($"{Section.Analyze}-{MyAnalyzeOrder}", Titration_Key.IterationCount));
                    }
                    return -1;
                }
            }
            public double TotalInjectionVolume_mL
            {
                get
                {
                    if (MyRef != null)
                    {
                        return InjectedList.Sum(info => info.InjVol_Single);
                    }
                    return -1;
                }
            }

            public bool InterpolationEnabled
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.EnableInterpolation;
                    }
                    if (MyParent != null)
                    {
                        return bool.Parse(MyParent.File_GetValue(MySection, Titration_Key.InterpolationEnabled));
                    }
                    return false;
                }
            }

            //private Dictionary<string, string> DicTtrSectionNames = new Dictionary<string, string>();

            // Create object for Analyzing
            public TitrationObj(HistoryObj parent, AnalyzeType type,  TitrationRef titrationRef)
            {
                HandleType = ObjHandleType.Created;

                MyParent = parent;      // History 작성을 위한 객체
                MyAnalyzeType = type;
                MyRef = titrationRef;
            }

            // Load object for Reviewing
            public TitrationObj(int analyzeOrder, HistoryObj parent)
            {
                HandleType = ObjHandleType.Opened;

                MyAnalyzeOrder = analyzeOrder;
                MySection = $"{Section.Analyze}-{MyAnalyzeOrder}";
                MyParent = parent;

                string sType = MyParent?.File_GetValue(MySection, Analyze_Key.AnalyzeType);
                if (Enum.TryParse(sType, out AnalyzeType type) == true)
                {
                    MyAnalyzeType = type;
                }
            }

            public bool Start_Analyze(DateTime startTime, int analyzeOrder)
            {
                MyAnalyzeOrder = analyzeOrder;
                MySection = $"{Section.Analyze}-{MyAnalyzeOrder}";

                // Add Section
                MyParent.File_AddSection(MySection);

                // Add Analyze Type
                MyParent.File_AddKey(MySection, Analyze_Key.AnalyzeType, MyAnalyzeType);

                // Add StartTime
                Set_StartTime(startTime);
                MyParent.File_AddKey(MySection, Titration_Key.StartTime_Titration, Util.GetStringFromDateTime(startTime));
                MyParent.File_AddKey(MySection, Titration_Key.EndTime_Titration, Util.GetStringFromDateTime(startTime));   // 보기 편하게 하려고 EndTime을 먼저 작성한다. (Ttr.종료 시에는 Add 대신 Set)
                MyParent.File_AddKey(MySection, Titration_Key.Duration_Titration, "00:00:00");   // 보기 편하게 하려고 EndTime을 먼저 작성한다. (Ttr.종료 시에는 Add 대신 Set)

                // Add Condition
                MyParent.File_AddKey(MySection, Titration_Key.SampleName, SampleName);
                MyParent.File_AddKey(MySection, Titration_Key.ReagentName, ReagentName);
                MyParent.File_AddKey(MySection, Titration_Key.Syringe_LogicalName, ReagentSyringeLogicalName);
                MyParent.File_AddKey(MySection, Titration_Key.AnalogInput_LogicalName, AnalogInputLogicalName);
                MyParent.File_AddKey(MySection, Titration_Key.MaxIterationCount, MaxIterationCount);
                MyParent.File_AddKey(MySection, Titration_Key.Offset_mL, Offset_mL);
                MyParent.File_AddKey(MySection, Titration_Key.MixingTime_AfterOffset_ms, MixingTime_AfterOffset_ms);
                MyParent.File_AddKey(MySection, Titration_Key.MixingTime_General_ms, MixingTime_General_ms);
                MyParent.File_AddKey(MySection, Titration_Key.InterpolationEnabled, InterpolationEnabled);
                MyParent.File_AddKey(MySection, Titration_Key.ScaleFactor_VolumeToConcentration, ScaleFactor_VolumeToConcentration);
                MyParent.File_AddKey(MySection, Titration_Key.TitrationTarget_mV, AnalogValue_Target);
                MyParent.File_AddKey(MySection, Titration_Key.TitrationEnd_mV, AnalogValue_End);
                MyParent.File_AddKey(MySection, Titration_Key.InjThr_LargeToMiddle, IncThr_ToMiddle);
                MyParent.File_AddKey(MySection, Titration_Key.InjThr_MiddleToSmall, IncThr_ToSmall);
                MyParent.File_AddKey(MySection, Titration_Key.InjVol_Large_mL, Inc_Large_mL);
                MyParent.File_AddKey(MySection, Titration_Key.InjVol_Middle_mL, Inc_Middle_mL);
                MyParent.File_AddKey(MySection, Titration_Key.InjVol_Small_mL, Inc_Small_mL);

                return true;
            }

            public bool End_Analyze(DateTime endTime, string filePath, string fileName)
            {
                Set_EndTime(endTime);

                MyParent.File_SetValue(MySection, Titration_Key.EndTime_Titration, Util.GetStringFromDateTime(EndTime_Titration));
                MyParent.File_SetValue(MySection, Titration_Key.Duration_Titration, Util.GetStringFromTimeSpan(Duration_Titration));

                if (Get_ResultAll(out List<InjectedObj> injList) == true)
                {
                    MyParent.File_AddKey(MySection, Titration_Key.IterationCount, injList.Count);
                    for (int i = 0; i < injList.Count; i++)
                    {
                        string key = $"{Titration_Key.InjectionInfo}{i}";
                        MyParent.File_AddKey(MySection, key, injList[i].ToString());
                    }

                    SaveIntectedInfo_AsCSV(filePath, fileName);

                    return true;
                }

                return false;
            }

            public void Set_StartTime(DateTime dt)
            {
                StartTime_Titration = dt;
            }

            public void Set_EndTime(DateTime endTime)
            {
                EndTime_Titration = endTime;
                Duration_Titration = EndTime_Titration - StartTime_Titration;
            }

            public void End_Inject(DateTime readTime, double injVol, double analog)
            {
                InjectedObj injObj = new InjectedObj()
                {
                    No = InjectedList.Count,
                    Time = readTime,
                    InjVol_Single = injVol,
                    InjVol_Accum = TotalInjectionVolume_mL + injVol,
                    Analog = analog,
                    Concentration = (TotalInjectionVolume_mL + injVol) * ScaleFactor_VolumeToConcentration
                };
                InjectedList.Add(injObj);
            }

            public bool Get_ResultAll(out List<InjectedObj> injList)
            {
                injList = InjectedList;
                if (injList.Count == 0)
                {
                    return false;
                }
                return true;
            }

            public bool Get_ProperResult(out InjectedObj injObj)
            {
                injObj = null;
                if (InjectedList.Count == 0)
                {
                    return false;
                }

                List<InjectedObj> validInfos = null;
                if (InterpolationEnabled == true)
                {
                    validInfos = InjectedList.Where(injInfo => injInfo.Analog == AnalogValue_Target).ToList();

                    // TBD: Migration Previous
                    //if (validInfos.Count == 0)
                    //{
                    //    var overTarget = InjectedList.Where(injInfo => injInfo.Analog > AnalogValue_Target).ToList();
                    //    if (InjectedList.Count > 1 && overTarget.Count > 0)
                    //    {
                    //        InjectedObj injObj_L = null;
                    //        InjectedObj injObj_H = null;
                    //        for (int i = 0; i < InjectedList.Count; i++)
                    //        {
                    //            if (InjectedList[i].Analog <= AnalogValue_Target)
                    //            {
                    //                injObj_L = InjectedList[i];
                    //            }
                    //            else
                    //            {
                    //                injObj_H = InjectedList[i];
                    //            }
                    //        }

                    //        double inj_Accum = Util.GetInterpolatedValue(AnalogValue_Target, injObj_L.InjVol_Accum, injObj_L.Analog, injObj_H.InjVol_Accum, injObj_H.Analog);
                    //    }
                    //}
                }
                else
                {
                    validInfos = InjectedList.Where(injInfo => injInfo.Analog >= AnalogValue_Target).ToList();
                }

                if (validInfos.Count == 0)
                {
                    return false;
                }
                injObj = validInfos[0];

                return true;
            }

            public bool Get_LastPoint(out InjectedObj injLast)
            {
                injLast = null;
                if (Get_InjectedObjList(out var list) == false)
                {
                    return false;
                }
                if (list.Count == 0)
                {
                    return false;
                }
                injLast = list[list.Count - 1];
                return true;
            }

            public bool Get_InjectedObjList(out List<InjectedObj> injObjs)
            {
                injObjs = null;
                if (InjectedList.Count == 0)
                {
                    return false;
                }
                injObjs = InjectedList;
                return true;
            }

            public bool CollectInjectedObj()
            {
                if (MyParent == null)
                {
                    return false;
                }
                InjectedList.Clear();

                for (int i = 0; i < IterationCount; i++)
                {
                    string injInfoLine = MyParent.File_GetValue(MySection, $"{Titration_Key.InjectionInfo}{i}");
                    InjectedObj injObj = new InjectedObj(injInfoLine);
                    InjectedList.Add(injObj);
                }
                return true;
            }

            public void SaveIntectedInfo_AsCSV(string filePath, string fileName_log)
            {
                if (InjectedList.Count == 0)
                {
                    return;
                }

                string fileName = fileName_log.Replace(".log", $"_{SampleName}_InjectedInfo.csv");
                using (FileStream fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("No,Time,InjVol_Single,InjVol_Accum,Concentration,Analog");
                        for (int i = 0; i < InjectedList.Count; i++)
                        {
                            sw.WriteLine(InjectedList[i].ToString());
                        }
                    }
                }
            }

            public class InjectedObj
            {
                public int No;
                public DateTime Time;
                public double InjVol_Single;
                public double Analog;
                public double InjVol_Accum;
                public double Concentration;

                public InjectedObj()
                {
                }

                public InjectedObj(string line)
                {
                    string[] parsed = line.Split(',');
                    No = int.Parse(parsed[0]);
                    Time = DateTime.Parse(parsed[1]);
                    InjVol_Single = double.Parse(parsed[2]);
                    InjVol_Accum = double.Parse(parsed[3]);
                    Concentration = double.Parse(parsed[4]);
                    Analog = double.Parse(parsed[5]);
                }

                public override string ToString()
                {
                    return $"{No},{Time.Hour}:{Time.Minute}:{Time.Second},{InjVol_Single},{InjVol_Accum:0.00##},{Concentration:0.000#},{Analog}";
                }
            }
        }
    }
}
