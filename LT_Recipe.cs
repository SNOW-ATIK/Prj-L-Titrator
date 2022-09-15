using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator_Alpha
{
    public class LT_Recipe
    {
        public const int RecipeMaxCount = 10;

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

        public static bool Load_Recipes()
        {
            RecipeDic.Clear();

            List<string> fileNames = Directory.GetFiles(PreDef.Path_Recipe).ToList();
            fileNames.ForEach(filename =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeObj));
                StreamReader sr = new StreamReader(filename);
                RecipeObj loaded = (RecipeObj)xmlSerializer.Deserialize(sr);
                if (loaded != null)
                {
                    loaded.Init_TitrationRef();
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
                string saveFileName = $@"{PreDef.Path_Recipe}\{rcpObj.Name}.xml";
                if (File.Exists(saveFileName) == true)
                { 
                    // TBD
                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeObj));
                StreamWriter sw = new StreamWriter(saveFileName);
                xmlSerializer.Serialize(sw, rcpObj);
                sw.Close();
                sw.Dispose();

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

            string rcpFileName = $@"{PreDef.Path_Recipe}\{rcpObj.Name}.xml";
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

        public static List<TitrationRef> Get_AllTitrationRef(int rcpNo)
        {
            if (RecipeExist(rcpNo) == false)
            {
                return null;
            }

            List<TitrationRef> lst = new List<TitrationRef>();
            RecipeDic[rcpNo].Sequences.ToList().ForEach(seq => seq.Steps.ForEach(step =>
            {
                if (step.IsTitration == true && step.Enabled == true)
                {
                    TitrationRef titrationRef = Load_TitrationRef(step);
                    if (titrationRef != null)
                    {
                        lst.Add(titrationRef);
                    }
                }
            }));
            return lst;
        }

        // PreDefined Sequence
        public static bool Load_PreDefinedSeq()
        {
            PreDefinedSeqDic.Clear();

            List<string> fileNames = Directory.GetFiles($@"{PreDef.Path_PreDefinedSeq}").ToList();
            fileNames.ForEach(filename =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Sequence));
                StreamReader sr = new StreamReader(filename);
                Sequence loaded = (Sequence)xmlSerializer.Deserialize(sr);
                if (loaded != null)
                {
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

            return true; 
        }

        // Titration Reference
        public static TitrationRef Load_TitrationRef(Step curStep)
        {
            return Load_TitrationRef($@"{PreDef.Path_Recipe_TitrationRef}\{curStep.TitrationRefFileName}");
        }

        public static TitrationRef Load_TitrationRef(string fileName)
        {
            TitrationRef loaded = null;
            if (File.Exists(fileName) == true)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TitrationRef));
                StreamReader sr = new StreamReader(fileName);
                loaded = (TitrationRef)xmlSerializer.Deserialize(sr);
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
                    if (step.IsTitration == true)
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
