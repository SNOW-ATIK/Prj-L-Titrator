using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

using ATIK;

namespace L_Titrator
{
    public class HistoryObj
    {
        public enum Section
        { 
            Sequence,
            Titration,
        }

        public enum SeqKey
        {
            StartTime_Sequence,
            EndTime_Sequence,
            Duration_Sequence,
            RecipeNo,
            RecipeName,
            NoOfTitrations,
        }

        public enum TitrationKey
        {
            SampleName,
            ReagentName,
            Syringe_LogicalName,
            AnalogInput_LogicalName,
            MaxIterationCount,
            Offset_mL,
            MixingTime_AfterOffset_ms,
            MixingTime_General_ms,
            ScaleFactor_VolumeToConcentration,
            TitrationTarget_mV,
            TitrationEnd_mV,
            InjThr_LargeToMiddle,
            InjThr_MiddleToSmall,
            InjVol_Large_mL,
            InjVol_Middle_mL,
            InjVol_Small_mL,

            StartTime_Titration,
            EndTime_Titration,
            Duration_Titration,
            IterationCount,
            InjectionInfo,
        }


        public bool Init_Success { get; private set; }
        public bool Load_Success { get; private set; }
        public bool End_Success { get; private set; }
        public DateTime StartTime_Sequence { get; private set; }
        public DateTime EndTime_Sequence { get; private set; }
        public TimeSpan Duration_Sequence { get; private set; }
        public int RecipeNo { get; private set; }
        public string RecipeName { get; private set; }
        public int NoOfTitrations { get; private set; }

        public string MyID
        {
            get
            {
                return Util.GetStringFromDateTime(StartTime_Sequence);
            }
        }
        public string MyFilePath
        {
            get
            {
                return $@"{PreDef.Path_History}\{StartTime_Sequence.Year:0000}\{StartTime_Sequence.Month:00}\{StartTime_Sequence.Day:00}";
            }
        }
        public string MyFileName
        {
            get
            {
                return $"{StartTime_Sequence.Year:0000}{StartTime_Sequence.Month:00}{StartTime_Sequence.Day:00}_{StartTime_Sequence.Hour:00}{StartTime_Sequence.Minute:00}{StartTime_Sequence.Second:00}.log";
            }
        }

        private RecipeObj MyRecipeObj;
        private IniCfg HistoryFile;
        private Dictionary<string, TitrationObj> DicTtrObjs = new Dictionary<string, TitrationObj>();
        private Dictionary<string, string> DicTtrSectionNames = new Dictionary<string, string>();

        public HistoryObj(DateTime startTime, int rcpNo)
        {
            if (Load_Success == true)
            {
                return;
            }

            if (LT_Recipe.Get_RecipeObj(rcpNo, out RecipeObj rcpObj) == false)
            {
                Init_Success = false;
                return;
            }

            MyRecipeObj = rcpObj;

            StartTime_Sequence = startTime;
            RecipeNo = rcpNo;
            RecipeName = rcpObj.Name;


            if (Directory.Exists(MyFilePath) == false)
            {
                Directory.CreateDirectory(MyFilePath);
                Thread.Sleep(50);
            }

            // Init History File
            HistoryFile = new IniCfg($@"{MyFilePath}\{MyFileName}");

            // Add Sections
            File_AddSection(Section.Sequence);

            // Add known value
            File_AddKey(Section.Sequence, SeqKey.StartTime_Sequence, MyID);
            File_AddKey(Section.Sequence, SeqKey.EndTime_Sequence, MyID);           // File을 열었을때 보기 편하게 EndTime을 먼저 작성한다. (Seq.종료 시에는 Add 대신 Set)
            File_AddKey(Section.Sequence, SeqKey.Duration_Sequence, "00:00:00");    // File을 열었을때 보기 편하게 Duration을 먼저 작성한다. (Seq.종료 시에는 Add 대신 Set)
            File_AddKey(Section.Sequence, SeqKey.RecipeNo, RecipeNo.ToString());
            File_AddKey(Section.Sequence, SeqKey.RecipeName, RecipeName);

            Init_Success = true;
        }

        public HistoryObj(string fileName)
        {
            if (Init_Success == true)
            {
                return;
            }

            // Load History File
            if (File.Exists(fileName) == false)
            {
                Load_Success = false;
                return;
            }

            HistoryFile = new IniCfg(fileName);
            StartTime_Sequence = DateTime.Parse(File_GetValue(Section.Sequence, SeqKey.StartTime_Sequence));
            EndTime_Sequence = DateTime.Parse(File_GetValue(Section.Sequence, SeqKey.EndTime_Sequence));
            Duration_Sequence = TimeSpan.Parse(File_GetValue(Section.Sequence, SeqKey.Duration_Sequence));
            RecipeNo = int.Parse(File_GetValue(Section.Sequence, SeqKey.RecipeNo));
            RecipeName = File_GetValue(Section.Sequence, SeqKey.RecipeName);
            NoOfTitrations = int.Parse(File_GetValue(Section.Sequence, SeqKey.NoOfTitrations));

            for (int i = 0; i < NoOfTitrations; i++)
            {
                string sectionName = $"{Section.Titration}-{i}";
                TitrationObj ttrObj = new TitrationObj(i, this);
                ttrObj.Set_StartTime(DateTime.Parse(File_GetValue(sectionName, TitrationKey.StartTime_Titration)));
                ttrObj.Set_EndTime(DateTime.Parse(File_GetValue(sectionName, TitrationKey.EndTime_Titration)));
                ttrObj.CollectInjectedObj();

                DicTtrObjs.Add(ttrObj.SampleName, ttrObj);
                DicTtrSectionNames.Add(ttrObj.SampleName, sectionName);
            }

            Load_Success = true;
        }

        // TBD. Write Sequence Info
        public void Set_StepProgress()
        { 
        }

        public bool Start_Titration(string sampleName, DateTime startTime, TitrationRef titrationRef)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            if (DicTtrObjs.ContainsKey(sampleName) == true)
            {
                return false;
            }
            TitrationObj ttrObj = new TitrationObj(titrationRef);
            DicTtrSectionNames.Add(sampleName, $"{Section.Titration}-{DicTtrObjs.Count}");
            DicTtrObjs.Add(sampleName, ttrObj);

            // Add Section
            File_AddSection(DicTtrSectionNames[sampleName]);

            // Add StartTime
            ttrObj.Set_StartTime(startTime);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.StartTime_Titration, Util.GetStringFromDateTime(startTime));
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.EndTime_Titration, Util.GetStringFromDateTime(startTime));   // 보기 편하게 하려고 EndTime을 먼저 작성한다. (Ttr.종료 시에는 Add 대신 Set)
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.Duration_Titration, "00:00:00");   // 보기 편하게 하려고 EndTime을 먼저 작성한다. (Ttr.종료 시에는 Add 대신 Set)

            // Add Condition
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.SampleName, ttrObj.SampleName);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.ReagentName, ttrObj.ReagentName);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.Syringe_LogicalName, ttrObj.ReagentSyringeLogicalName);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.AnalogInput_LogicalName, ttrObj.AnalogInputLogicalName);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.MaxIterationCount, ttrObj.MaxIterationCount);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.Offset_mL, ttrObj.Offset_mL);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.MixingTime_AfterOffset_ms, ttrObj.MixingTime_AfterOffset_ms);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.MixingTime_General_ms, ttrObj.MixingTime_General_ms);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.ScaleFactor_VolumeToConcentration, ttrObj.ScaleFactor_VolumeToConcentration);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.TitrationTarget_mV, ttrObj.AnalogValue_Target);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.TitrationEnd_mV, ttrObj.AnalogValue_End);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.InjThr_LargeToMiddle, ttrObj.IncThr_ToMiddle);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.InjThr_MiddleToSmall, ttrObj.IncThr_ToSmall);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.InjVol_Large_mL, ttrObj.Inc_Large_mL);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.InjVol_Middle_mL, ttrObj.Inc_Middle_mL);
            File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.InjVol_Small_mL, ttrObj.Inc_Small_mL);

            return true;
        }

        public bool Get_TitrationObj(string sampleName, out TitrationObj titrationObj)
        {
            if (DicTtrObjs.ContainsKey(sampleName) == false)
            {
                titrationObj = null;
                return false;
            }
            titrationObj = DicTtrObjs[sampleName];
            return true;
        }

        public bool Get_TitrationObj(int order, out TitrationObj titrationObj)
        {
            titrationObj = null;
            var take = DicTtrObjs.Values.Where(ttrObj => ttrObj.MyTitrationOrder == order).ToList();
            if (take.Count == 1)
            {
                titrationObj = take[0];
                return true;
            }
            return false;
        }

        public void End_Titration(string sampleName, DateTime endTime)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            if (Get_TitrationObj(sampleName, out TitrationObj ttrObj) == true)
            {
                ttrObj.Set_EndTime(endTime);
                File_SetValue(DicTtrSectionNames[sampleName], TitrationKey.EndTime_Titration, Util.GetStringFromDateTime(endTime));
                File_SetValue(DicTtrSectionNames[sampleName], TitrationKey.Duration_Titration, Util.GetStringFromTimeSpan(ttrObj.Duration_Titration));

                if (ttrObj.Get_ResultAll(out List<TitrationObj.InjectedObj> injList) == true)
                {
                    File_AddKey(DicTtrSectionNames[sampleName], TitrationKey.IterationCount, injList.Count);
                    for (int i = 0; i < injList.Count; i++)
                    {
                        string key = $"{TitrationKey.InjectionInfo}{i}";
                        File_AddKey(DicTtrSectionNames[sampleName], key, injList[i].ToString());
                    }
                    ttrObj.SaveIntectedInfo_AsCSV(MyFilePath, MyFileName);
                }
                else
                {
                }
            }
            else
            { 
            }
        }

        public void End_Sequence(DateTime endTime)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            EndTime_Sequence = endTime;
            File_SetValue(Section.Sequence, SeqKey.EndTime_Sequence, Util.GetStringFromDateTime(EndTime_Sequence));

            Duration_Sequence = EndTime_Sequence - StartTime_Sequence;
            File_SetValue(Section.Sequence, SeqKey.Duration_Sequence, Util.GetStringFromTimeSpan(Duration_Sequence));

            File_AddKey(Section.Sequence, SeqKey.NoOfTitrations, DicTtrObjs.Count);

            File_Save();
        }

        private void File_AddSection(object section)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            HistoryFile.AddSection(section.ToString());
        }

        private void File_AddKey(object section, object key, object initValue)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            HistoryFile.AddKey(section.ToString(), key.ToString(), initValue.ToString());
        }

        private void File_SetValue(object section, object key, object value)
        {
            HistoryFile.SetString(section.ToString(), key.ToString(), value.ToString());
        }

        private string File_GetValue(object section, object key)
        {
            return HistoryFile.GetString(section.ToString(), key.ToString());
        }

        private bool File_Save()
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            return HistoryFile.Save();
        }


        public class TitrationObj
        {
            private HistoryObj MyParent;
            private string MySection;
            private TitrationRef MyRef;
            private List<InjectedObj> InjectedList = new List<InjectedObj>();
            public int MyTitrationOrder { get; private set; }

            public string SampleName
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.SampleName;
                    }
                    if (MyParent != null)
                    { 
                        return MyParent.File_GetValue(MySection, TitrationKey.SampleName);
                    }
                    return "";
                }
            }
            public string ReagentName
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.ReagentName;
                    }
                    if (MyParent != null)
                    {
                        return MyParent.File_GetValue(MySection, TitrationKey.ReagentName);
                    }
                    return "";
                }
            }
            public string ReagentSyringeLogicalName
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.ReagentSyringeLogicalName;
                    }
                    if (MyParent != null)
                    {
                        return MyParent.File_GetValue(MySection, TitrationKey.Syringe_LogicalName);
                    }
                    return "";
                }
            }
            public string AnalogInputLogicalName
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.AnalogInfo.AnalogInputLogicalName;
                    }
                    if (MyParent != null)
                    {
                        return MyParent.File_GetValue(MySection, TitrationKey.AnalogInput_LogicalName);
                    }
                    return "";
                }
            }
            public int MaxIterationCount
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.MaxIterationCount;
                    }
                    if (MyParent != null)
                    {
                        return int.Parse(MyParent.File_GetValue(MySection, TitrationKey.MaxIterationCount));
                    }
                    return -1;
                }
            }
            public double Offset_mL
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.Offset;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.Offset_mL));
                    }
                    return -1;
                }
            }
            public int MixingTime_AfterOffset_ms
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.MixingTime_AfterOffset;
                    }
                    if (MyParent != null)
                    {
                        return int.Parse(MyParent.File_GetValue(MySection, TitrationKey.MixingTime_AfterOffset_ms));
                    }
                    return -1;
                }
            }
            public int MixingTime_General_ms
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.MixingTime_General;
                    }
                    if (MyParent != null)
                    {
                        return int.Parse(MyParent.File_GetValue(MySection, TitrationKey.MixingTime_General_ms));
                    }
                    return -1;
                }
            }
            public double ScaleFactor_VolumeToConcentration
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.ScaleFactor;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.ScaleFactor_VolumeToConcentration));
                    }
                    return -1;
                }
            }
            public double AnalogValue_Target
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.AnalogInfo.TargetValue;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.TitrationTarget_mV));
                    }
                    return -1;
                }
            }
            public double AnalogValue_End
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.AnalogInfo.EndValue;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.TitrationEnd_mV));
                    }
                    return -1;
                }
            }
            public double IncThr_ToMiddle
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.IncThreshold_ChangeToMiddle;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.InjThr_LargeToMiddle));
                    }
                    return -1;
                }
            }
            public double IncThr_ToSmall
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.IncThreshold_ChangeToSmall;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.InjThr_MiddleToSmall));
                    }
                    return -1;
                }
            }
            public double Inc_Large_mL
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.Inc_Large;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.InjVol_Large_mL));
                    }
                    return -1;
                }
            }
            public double Inc_Middle_mL
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.Inc_Middle;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.InjVol_Middle_mL));
                    }
                    return -1;
                }
            }
            public double Inc_Small_mL
            {
                get
                {
                    if (MyRef != null)
                    {
                        return MyRef.InjectionInfo.Inc_Small;
                    }
                    if (MyParent != null)
                    {
                        return double.Parse(MyParent.File_GetValue(MySection, TitrationKey.InjVol_Small_mL));
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
                        return int.Parse(MyParent.File_GetValue($"{Section.Titration}-{MyTitrationOrder}", TitrationKey.IterationCount));
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

            public TitrationObj(TitrationRef titrationRef)
            {
                MyRef = titrationRef;
            }

            public TitrationObj(int titrationOrder, HistoryObj parent)
            {
                MyTitrationOrder = titrationOrder;
                MySection = $"{Section.Titration}-{MyTitrationOrder}";
                MyParent = parent;
            }

            public void Set_StartTime(DateTime dt)
            {
                StartTime_Titration = dt;
            }

            public void Set_EndTime(DateTime dt)
            {
                EndTime_Titration = dt;
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

                var validInfos = InjectedList.Where(injInfo => injInfo.Analog > AnalogValue_Target).ToList();
                if (validInfos.Count == 0)
                {
                    return false;
                }
                injObj = validInfos[0];
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
                    string injInfoLine = MyParent.File_GetValue(MySection, $"{TitrationKey.InjectionInfo}{i}");
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
                    return $"{No},{Time.Hour}:{Time.Minute}:{Time.Second},{InjVol_Single},{InjVol_Accum},{Concentration:0.000},{Analog}";
                }
            }
        }
    }

    public static class LT_History
    {
        public static List<HistoryObj> LoadAll()
        {
            List<HistoryObj> allHistory = new List<HistoryObj>();
            return allHistory;
        }

        public static Dictionary<int, List<HistoryObj>> LoadMonth(int year, int month)
        {
            Dictionary<int, List<HistoryObj>> monthHistory = new Dictionary<int, List<HistoryObj>>();
            string monthPath = Path.Combine(PreDef.Path_History, year.ToString("0000"), month.ToString("00"));
            for (int i = 1; i < DateTime.DaysInMonth(year, month); i++)
            {
                string dayPath = Path.Combine(monthPath, i.ToString("00"));
                if (LoadDay(dayPath, out List<HistoryObj> dayHistorys) == true)
                {
                    monthHistory.Add(i, dayHistorys);
                }

            }
            return monthHistory;
        }

        public static bool LoadDay(string dayPath, out List<HistoryObj> dayHistorys)
        {
            dayHistorys = new List<HistoryObj>();
            if (Directory.Exists(dayPath) == false)
            {
                return false;
            }

            List<string> fileInDay = Directory.GetFiles(dayPath).ToList();
            for (int i = 0; i < fileInDay.Count; i++)
            {
                try
                {
                    HistoryObj history = new HistoryObj(fileInDay[i]);
                    dayHistorys.Add(history);
                }
                catch
                { 
                }
            }

            return true;
        }

        public static bool LoadDay(int year, int month, int day, out List<HistoryObj> historyInDay)
        {
            historyInDay = new List<HistoryObj>();
            string FilePath = Path.Combine(PreDef.Path_History, year.ToString("00"), month.ToString("00"), day.ToString("00"));
            if (LoadDay(FilePath, out historyInDay) == true)
            {
                return true;
            }

            return false;
        }
    }
}
