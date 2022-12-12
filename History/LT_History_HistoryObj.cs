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

    public partial class HistoryObj
    {
        public interface IAnalyzeObj
        {
            int MyAnalyzeOrder { get; set; }
            AnalyzeType MyAnalyzeType { get; set; }

            bool Start_Analyze(DateTime startTime, int analyzeOrder);
            bool End_Analyze(DateTime endTime, string filePath, string fileName);
        }

        public enum Section
        {
            Sequence,
            Analyze,
        }

        public enum Seq_Key
        {
            StartTime_Sequence,
            EndTime_Sequence,
            Duration_Sequence,
            RecipeNo,
            RecipeName,
            NoOfAnalyzes,
        }

        public enum Analyze_Key
        { 
            AnalyzeType,
            Titration,
            ISE,
        }

        public enum Titration_Key
        {
            SampleName,
            ReagentName,
            Syringe_LogicalName,
            AnalogInput_LogicalName,
            MaxIterationCount,
            Offset_mL,
            MixingTime_AfterOffset_ms,
            MixingTime_General_ms,
            InterpolationEnabled,
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

        public enum ISE_Key
        {
        }


        public bool Init_Success { get; private set; }
        public bool Load_Success { get; private set; }
        public bool End_Success { get; private set; }
        public DateTime StartTime_Sequence { get; private set; }
        public DateTime EndTime_Sequence { get; private set; }
        public TimeSpan Duration_Sequence { get; private set; }
        public int RecipeNo { get; private set; }
        public string RecipeName { get; private set; }
        public int NoOfAnalyzes { get; private set; }

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
                return $@"{PreDef.Path_History_Data}\{StartTime_Sequence.Year:0000}\{StartTime_Sequence.Month:00}\{StartTime_Sequence.Day:00}";
            }
        }
        public string MyFileName
        {
            get
            {
                return $"{StartTime_Sequence.Year:0000}{StartTime_Sequence.Month:00}{StartTime_Sequence.Day:00}_{StartTime_Sequence.Hour:00}{StartTime_Sequence.Minute:00}{StartTime_Sequence.Second:00}.log";
            }
        }

        private Dictionary<string, IAnalyzeObj> DicAnalyzeObjs = new Dictionary<string, IAnalyzeObj>();
        private RecipeObj MyRecipeObj;
        private IniCfg HistoryFile;

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

            // Add Common value
            File_AddKey(Section.Sequence, Seq_Key.StartTime_Sequence, MyID);
            File_AddKey(Section.Sequence, Seq_Key.EndTime_Sequence, MyID);           // File을 열었을때 보기 편하게 EndTime을 먼저 작성한다. (Seq.종료 시에는 Add 대신 Set)
            File_AddKey(Section.Sequence, Seq_Key.Duration_Sequence, "00:00:00");    // File을 열었을때 보기 편하게 Duration을 먼저 작성한다. (Seq.종료 시에는 Add 대신 Set)
            File_AddKey(Section.Sequence, Seq_Key.RecipeNo, RecipeNo.ToString());
            File_AddKey(Section.Sequence, Seq_Key.RecipeName, RecipeName);

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
            StartTime_Sequence = DateTime.Parse(File_GetValue(Section.Sequence, Seq_Key.StartTime_Sequence));
            EndTime_Sequence = DateTime.Parse(File_GetValue(Section.Sequence, Seq_Key.EndTime_Sequence));
            Duration_Sequence = TimeSpan.Parse(File_GetValue(Section.Sequence, Seq_Key.Duration_Sequence));
            RecipeNo = int.Parse(File_GetValue(Section.Sequence, Seq_Key.RecipeNo));
            RecipeName = File_GetValue(Section.Sequence, Seq_Key.RecipeName);
            NoOfAnalyzes = int.Parse(File_GetValue(Section.Sequence, Seq_Key.NoOfAnalyzes));

            for (int i = 0; i < NoOfAnalyzes; i++)
            {
                string sectionName = $"{Section.Analyze}-{i}";
                string sType = File_GetValue(sectionName, Analyze_Key.AnalyzeType);
                if (Enum.TryParse(sType, out AnalyzeType type) == false)
                {
                    continue;
                }

                switch (type)
                {
                    case AnalyzeType.pH:
                    case AnalyzeType.ORP:
                        TitrationObj ttrObj = new TitrationObj(i, this);
                        ttrObj.Set_StartTime(DateTime.Parse(File_GetValue(sectionName, Titration_Key.StartTime_Titration)));
                        ttrObj.Set_EndTime(DateTime.Parse(File_GetValue(sectionName, Titration_Key.EndTime_Titration)));
                        ttrObj.CollectInjectedObj();

                        DicAnalyzeObjs.Add(ttrObj.SampleName, ttrObj);
                        break;

                    case AnalyzeType.ISE:
                        break;
                };
            }

            Load_Success = true;
        }

        // TBD. Write Sequence Info
        public void Set_StepProgress()
        {
        }

        public bool Start_Analyze(DateTime startTime, AnalyzeRef analyzeRef)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            switch (analyzeRef.Type)
            {
                case AnalyzeType.pH:
                case AnalyzeType.ORP:
                    TitrationObj ttrObj = new TitrationObj(this, analyzeRef.Type,  analyzeRef.TtrRef);
                    DicAnalyzeObjs.Add(analyzeRef.SampleName, ttrObj);
                    break;

                case AnalyzeType.ISE:
                    break;
            }

            if (DicAnalyzeObjs.ContainsKey(analyzeRef.SampleName) == false)
            {
                return false;
            }
            DicAnalyzeObjs[analyzeRef.SampleName].Start_Analyze(startTime, DicAnalyzeObjs.Count - 1);   // -1 for zero-based index of AnalyzeOrder

            return true;
        }

        public bool End_Analyze(string sampleName, DateTime endTime)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            if (DicAnalyzeObjs.ContainsKey(sampleName) == false)
            {
                return false;
            }

            return DicAnalyzeObjs[sampleName].End_Analyze(endTime, MyFilePath, MyFileName);
        }

        public bool Get_AnalyzeObj(string sampleName, out IAnalyzeObj analyzeObj)
        {
            analyzeObj = null;
            if (DicAnalyzeObjs.ContainsKey(sampleName) == false)
            {
                return false;
            }
            analyzeObj = DicAnalyzeObjs[sampleName];
            return true;
        }

        public bool Get_AnalyzeObj(int order, out IAnalyzeObj analyzeObj)
        {
            analyzeObj = null;
            var take = DicAnalyzeObjs.Values.Where(obj => obj.MyAnalyzeOrder == order).ToList();
            if (take.Count == 1)
            {
                analyzeObj = take[0];
                return true;
            }
            return false;
        }

        //public bool Get_TitrationObj(string sampleName, out TitrationObj titrationObj)
        //{
        //    titrationObj = null;
        //    if (DicAnalyzeObjs.ContainsKey(sampleName) == false)
        //    {
        //        return false;
        //    }

        //    if (DicAnalyzeObjs[sampleName].GetType() == typeof(TitrationObj))
        //    {
        //        titrationObj = (TitrationObj)DicAnalyzeObjs[sampleName];
        //    }

        //    return true;
        //}

        //public bool Get_TitrationObj(int order, out TitrationObj titrationObj)
        //{
        //    titrationObj = null;
        //    var take = DicAnalyzeObjs.Values.Where(analyzeObj => analyzeObj.MyAnalyzeOrder == order).ToList();
        //    if (take.Count == 1 && take[0].GetType() == typeof(TitrationObj))
        //    {
        //        titrationObj = (TitrationObj)take[0];
        //        return true;
        //    }
        //    return false;
        //}

        public void End_Sequence(DateTime endTime)
        {
            if (Load_Success == true)
            {
                throw new InvalidOperationException();
            }

            EndTime_Sequence = endTime;
            File_SetValue(Section.Sequence, Seq_Key.EndTime_Sequence, Util.GetStringFromDateTime(EndTime_Sequence));

            Duration_Sequence = EndTime_Sequence - StartTime_Sequence;
            File_SetValue(Section.Sequence, Seq_Key.Duration_Sequence, Util.GetStringFromTimeSpan(Duration_Sequence));

            File_AddKey(Section.Sequence, Seq_Key.NoOfAnalyzes, DicAnalyzeObjs.Count);

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
    }
}
