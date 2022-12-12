using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator
{
    public class LT_Recipe
    {
        public const int RecipeMaxCount = 10;
        public const int HotKeyMaxCount = 5;
        public const int ValidationMaxCount = 2;
        public const int DummyMaxCount = 3;

        public enum HotKeyRecipeNo
        { 
            Initialize = 10,
            Flush = 11,
            VesselEmpty = 12,
            RefillSyringe1 = 13,
            RefillSyringe2 = 14,
            Validation1 = 15,
            Validation2 = 16,
        }

        public enum ValidationRecipeNo
        { 
        }

        public enum DummyRecipeNo
        { 
            Dummy1 = 17,
            Dummy2 = 18, 
            Dummy3= 19,
        }

        public enum SequenceList
        {
            Initialize,
            PreProcess,
            Sampling,
            Titration,
            PostProcess,
        }

        private static Dictionary<int, RecipeObj> RecipeDic = new Dictionary<int, RecipeObj>();
        private static Dictionary<string, Sequence> PreDefinedSeqDic = new Dictionary<string, Sequence>();

        public static bool Load()
        {
            RecipeDic.Clear();

            List<string> fileNames = Directory.GetFiles(PreDef.Path_Recipe).ToList();
            fileNames.AddRange(Directory.GetFiles(PreDef.Path_Recipe_HotKey).ToList());
            fileNames.ForEach(filename =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeObj));
                StreamReader sr = new StreamReader(filename);
                RecipeObj loaded = (RecipeObj)xmlSerializer.Deserialize(sr);
                if (loaded != null)
                {
                    loaded.Load_AnalyzeRef();
                    RecipeDic.Add(loaded.No, loaded);
                }
                sr.Close();
                sr.Dispose();
            });

            return true;
        }

        public static bool Get_RecipeObj(int rcpNo, out RecipeObj rcpObj)
        {
            if (RecipeExist(rcpNo) == true)
            {
                rcpObj = RecipeDic[rcpNo];
                return true;
            }
            rcpObj = null;
            return false;
        }

        public static bool SerializeRecipe(RecipeObj rcpObj)
        {
            bool bDone = false;
            try
            {
                string saveFileName;
                if (rcpObj.No < RecipeMaxCount)
                {
                    saveFileName = $@"{PreDef.Path_Recipe}\{rcpObj.Name}.xml";
                }
                else
                {
                    saveFileName = $@"{PreDef.Path_Recipe_HotKey}\{rcpObj.Name}.xml";
                }

                if (File.Exists(saveFileName) == true)
                {
                    // TBD
                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeObj));
                StreamWriter sw = new StreamWriter(saveFileName);
                xmlSerializer.Serialize(sw, rcpObj);
                sw.Close();
                sw.Dispose();

                rcpObj.Sequences.ForEach(seq =>
                {
                    seq.Steps.ForEach(step =>
                    {
                        if (step.IsAnalyzeStep == true)
                        {
                            string ttrRefFileName = $@"{PreDef.Path_Recipe_AnalyzeRef}\{step.AnalyzeRefFileName}";
                            XmlSerializer xmlAnalyze = new XmlSerializer(typeof(AnalyzeRef));
                            StreamWriter swAnalyze = new StreamWriter(ttrRefFileName);
                            xmlAnalyze.Serialize(swAnalyze, step.AnalyzeRefObj);
                            swAnalyze.Close();
                            swAnalyze.Dispose();
                        }
                    });
                });

                if (RecipeDic.ContainsKey(rcpObj.No) == true)
                {
                    // TBD
                    RecipeDic.Remove(rcpObj.No);
                    RecipeDic.Add(rcpObj.No, (RecipeObj)rcpObj.Clone());
                }
                else
                {
                    RecipeDic.Add(rcpObj.No, (RecipeObj)rcpObj.Clone());
                }

                bDone = true;
            }
            catch
            {

            }

            return bDone;
        }

        public static bool CreateRecipe(int rcpNo, string rcpName)
        {
            if (RecipeExist(rcpNo) == true)
            {
                return false;
            }
            if (RecipeExist(rcpName) == true)
            {
                return false;
            }
            RecipeObj rcpObj = new RecipeObj();
            rcpObj.No = rcpNo;
            rcpObj.Name = rcpName;
            rcpObj.Sequences = new List<Sequence>();
            RecipeDic.Add(rcpNo, rcpObj);

            return true;
        }

        public static bool SaveRecipe(int rcpNo)
        {
            if (Get_RecipeObj(rcpNo, out var rcpObj) == false)
            {
                return false;
            }

            return SerializeRecipe(rcpObj);
        }

        public static void DeleteRecipe(int rcpNo)
        {
            if (Get_RecipeObj(rcpNo, out var rcpObj) == false)
            {
                return;
            }
            RecipeDic.Remove(rcpNo);

            string rcpFileName;
            if (rcpNo < RecipeMaxCount)
            {
                rcpFileName = $@"{PreDef.Path_Recipe}\{rcpObj.Name}.xml";
            }
            else
            {
                rcpFileName = $@"{PreDef.Path_Recipe_HotKey}\{rcpObj.Name}.xml";
            }

            if (File.Exists(rcpFileName) == false)
            {
                return;
            }
            File.Delete(rcpFileName);
            System.Threading.Thread.Sleep(100);
        }

        public static bool RecipeExist(int rcpNo)
        {
            if (RecipeDic == null)
            {
                return false;
            }
            return RecipeDic.ContainsKey(rcpNo);
        }

        public static bool RecipeExist(string rcpName)
        {
            if (RecipeDic == null)
            {
                return false;
            }

            return RecipeDic.Values.Where(rcp => rcp.Name == rcpName).ToList().Count > 0;
        }

        public static List<AnalyzeRef> Get_AllAnalyzeRef(int rcpNo)
        {
            if (RecipeExist(rcpNo) == false)
            {
                return null;
            }

            List<AnalyzeRef> lst = new List<AnalyzeRef>();
            RecipeDic[rcpNo].Sequences.ToList().ForEach(seq => seq.Steps.ForEach(step =>
            {
                if (step.IsAnalyzeStep == true && step.Enabled == true)
                {
                    //if (step.TitrationRef != null)
                    //{
                    //    lst.Add(step.TitrationRef);
                    //}
                    AnalyzeRef analyzeRef = Load_AnalyzeRef(PreDef.Path_Recipe_AnalyzeRef, step.AnalyzeRefFileName);
                    if (analyzeRef != null)
                    {
                        lst.Add(analyzeRef);
                    }
                }
            }));
            return lst;
        }

        // PreDefined Sequence
        public static bool Load_PreDefinedSeq()
        {
            PreDefinedSeqDic.Clear();

            List<string> fileNames = Directory.GetFiles($@"{PreDef.Path_PreFixSequence}").ToList();
            fileNames.ForEach(filename =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Sequence));
                StreamReader sr = new StreamReader(filename);
                Sequence loaded = (Sequence)xmlSerializer.Deserialize(sr);
                if (loaded != null)
                {
                    if (loaded.Steps?.Count > 0)
                    {
                        if (loaded.Steps[0].IsAnalyzeStep == true)
                        {
                            loaded.Steps[0].Load_AnalyzeRef(PreDef.Path_PreFixAnalyzeRef);
                        }
                    }
                    PreDefinedSeqDic.Add(loaded.Name, loaded);
                }
                sr.Close();
                sr.Dispose();
            });
            return true;
        }

        public static bool Get_PreDefSeqNames(out List<string> seqNames)
        {
            seqNames = null;
            if (PreDefinedSeqDic == null || PreDefinedSeqDic.Count == 0)
            {
                return false;
            }
            seqNames = PreDefinedSeqDic.Keys.ToList();

            return true;
        }

        public static bool Get_CopiedPreDefSeq(string seqName, out Sequence copiedSeq)
        {
            copiedSeq = null;
            if (PreDefinedSeqDic.ContainsKey(seqName) == false)
            {
                return false;
            }
            if (PreDefinedSeqDic[seqName] == null)
            {
                return false;
            }

            copiedSeq = (Sequence)PreDefinedSeqDic[seqName].Clone();

            if (copiedSeq.Steps.Where(step => step.IsAnalyzeStep == true).Count() > 0)
            {
                // Analyze Seq.일 경우 PreFix Ref 파일을 Load한 뒤에 파일명을 바꿔주어 실제 사용할 경로에 저장되도록 한다.
                copiedSeq.Steps[0].Load_AnalyzeRef(PreDef.Path_PreFixAnalyzeRef);

                string fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{copiedSeq.Steps[0].AnalyzeRefObj.Type}.xml";
                copiedSeq.Steps[0].AnalyzeRefFileName = fileName;
            }

            return true; 
        }

        public static AnalyzeRef Load_AnalyzeRef(string filePath, string fileName)
        {
            AnalyzeRef loaded = null;
            string fileFullName = Path.Combine(filePath, fileName);
            if (File.Exists(fileFullName) == true)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AnalyzeRef));
                StreamReader sr = new StreamReader(fileFullName);
                loaded = (AnalyzeRef)xmlSerializer.Deserialize(sr);
                sr.Close();
                sr.Dispose();
            }
            return loaded;
        }

        public static Step Get_LastStateOfSequence(int rcpNo, int seqNo)
        {
            if (Get_RecipeObj(rcpNo, out var rcpObj) == false)
            {
                return null;
            }

            // Create Step
            Step LastState = new Step();
            LastState.Valves = new List<Valve>();
            LastState.Mixers = new List<Mixer>();

            // Init. Base State
            var BaseSeq = rcpObj.Get_Sequence(0);
            LastState = (Step)BaseSeq.Get_Step(0).Clone();
            for (int i = 1; i < BaseSeq.Steps.Count; i++)
            {
                Step step = BaseSeq.Get_Step(i);

                MergeStates(step, LastState);
            }

            if (seqNo == 0)
            {
                return LastState;
            }

            for (int i = 1; i <= seqNo; i++)
            {
                var SeqObj = rcpObj.Get_Sequence(1);
                for (int j = 0; j < SeqObj.Steps.Count; j++)
                {
                    Step step = SeqObj.Get_Step(j);
                    if (step.IsAnalyzeStep == true)
                    {
                        break;  // Skip Titration condition
                    }

                    MergeStates(step, LastState);
                }
            }

            return LastState;
        }

        public static Step Get_StateOfJustBefore(int rcpNo, int seqNo, int stepNoCur)
        {
            if (Get_RecipeObj(rcpNo, out var rcpObj) == false)
            {
                return null;
            }

            Step LastState = new Step();
            if (seqNo == 0)
            {
                var BaseSeq = rcpObj.Get_Sequence(0);
                LastState = (Step)BaseSeq.Get_Step(0).Clone();
                for (int i = 1; i < stepNoCur; i++)
                {
                    Step step = BaseSeq.Get_Step(i);

                    MergeStates(step, LastState);
                }
            }
            else
            {
                LastState = Get_LastStateOfSequence(rcpNo, seqNo - 1);

                var curSeq = rcpObj.Get_Sequence(seqNo);

                for (int i = 0; i < stepNoCur; i++)
                {
                    Step step = curSeq.Get_Step(i);

                    MergeStates(step, LastState);
                }
            }

            return LastState;
        }

        private static void MergeStates(Step curStep, Step lastStep)
        { 
            // Valves
            if (curStep.Control_Valve == true)
            {
                curStep.Valves.ForEach(ctrlValve =>
                {
                    for (int i = 0; i < lastStep.Valves.Count; i++)
                    {
                        var tgtValve = lastStep.Valves[i];
                        if (tgtValve.Name == ctrlValve.Name)
                        {
                            if (tgtValve.Get_Condition() != ctrlValve.Get_Condition())
                            {
                                tgtValve.Set_Condition(ctrlValve.Get_Condition());
                            }
                        }
                    }
                });
            }

            // Mixers
            if (curStep.Control_Mixer == true)
            {
                curStep.Mixers.ForEach(ctrlMixer =>
                {
                    for (int i = 0; i < lastStep.Mixers.Count; i++)
                    {
                        var tgtMixer = lastStep.Mixers[i];
                        if (tgtMixer.Name == ctrlMixer.Name)
                        {
                            if (tgtMixer.Get_Duty() != ctrlMixer.Get_Duty())
                            {
                                tgtMixer.Set_Duty(ctrlMixer.Get_Duty());
                            }
                        }
                    }
                });
            }
        }
    }
}
