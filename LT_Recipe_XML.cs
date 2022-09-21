using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Diagnostics;

using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator
{
    [Serializable]
    [XmlRoot("Recipe")]
    public class RecipeObj : ICloneable, IEquatable<RecipeObj>
    {
        [XmlElement("No")]
        public int No { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlArray("SequenceList")]
        [XmlArrayItem("Sequence", typeof(Sequence))]
        public List<Sequence> Sequences;

        //public List<TitrationRef> TitrationRefList = new List<TitrationRef>();

        public Sequence Get_Sequence(int seqNo)
        {
            if (Sequences != null && Sequences.Count > 0)
            {
                List<Sequence> seqs = Sequences.Where(seq => seq.No == seqNo).ToList();
                if (seqs.Count == 1)
                {
                    return seqs[0];
                }
            }
            return null;
        }

        public void Init_TitrationRef()
        {
            if (Sequences == null || Sequences.Count == 0)
            {
                return;
            }

            Sequences.ForEach(seq =>
            {
                if (seq.Steps == null || seq.Steps.Count == 0)
                {
                    return;
                }
                seq.Steps.ForEach(step =>
                {
                    if (step.IsTitration == true)
                    {
                        step.Load_TitrationRef();
                        //TitrationRefList.Add(step.TitrationRef);
                    }
                });
            });
        }

        public object Clone()
        {
            RecipeObj clone = new RecipeObj();
            clone.No = No;
            clone.Name = Name;
            clone.Sequences = new List<Sequence>();
            if (Sequences != null && Sequences.Count > 0)
            {
                for (int i = 0; i < Sequences.Count; i++)
                {
                    clone.Sequences.Add((Sequence)Sequences[i].Clone());
                }
            }
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RecipeObj);
        }

        public bool Equals(RecipeObj other)
        {
            if (other == null)
            {
                return false;
            }

            /*
            //Equality Test            
            bool bSeqSame = false;
            if (Sequences.Count == other.Sequences.Count)
            {
                // sequence
                bool[] seqSame = new bool[Sequences.Count];
                for (int i = 0; i < Sequences.Count; i++)
                {
                    if (Sequences[i].Steps.Count == other.Sequences[i].Steps.Count)
                    {
                        // step
                        bool[] stepSame = new bool[Sequences[i].Steps.Count];
                        for (int j = 0; j < Sequences[i].Steps.Count; j++)
                        {
                            stepSame[j] = Sequences[i].Steps[j] == other.Sequences[i].Steps[j];                            

                            // Mixer
                            if (Sequences[i].Steps[j].Mixers.Count == other.Sequences[i].Steps[j].Mixers.Count)
                            {
                                bool[] mixerSame = new bool[Sequences[i].Steps[j].Mixers.Count];
                                for (int k = 0; k < Sequences[i].Steps[j].Mixers.Count; k++)
                                {
                                    mixerSame[k] = Sequences[i].Steps[j].Mixers[k] == other.Sequences[i].Steps[j].Mixers[k];
                                }
                                if (mixerSame.Contains(false) == true)
                                { 
                                }

                                bool stependSame = Sequences[i].Steps[j].StepEndCheck == other.Sequences[i].Steps[j].StepEndCheck;

                                bool[] syringeSame = new bool[Sequences[i].Steps[j].Syringes.Count];
                                for (int k = 0; k < Sequences[i].Steps[j].Syringes.Count; k++)
                                {
                                    syringeSame[k] = Sequences[i].Steps[j].Syringes[k] == other.Sequences[i].Steps[j].Syringes[k];
                                }
                                if (syringeSame.Contains(false) == true)
                                {
                                }

                                bool[] valveSame = new bool[Sequences[i].Steps[j].Valves.Count];
                                for (int k = 0; k < Sequences[i].Steps[j].Valves.Count; k++)
                                {
                                    valveSame[k] = Sequences[i].Steps[j].Valves[k] == other.Sequences[i].Steps[j].Valves[k];
                                }
                                if (valveSame.Contains(false) == true)
                                {
                                }

                                Dictionary<string, bool> varSame = new Dictionary<string, bool>();
                                bool bControl_Mixer = Sequences[i].Steps[j].Control_Mixer == other.Sequences[i].Steps[j].Control_Mixer;
                                varSame.Add("Control_Mixer", bControl_Mixer);
                                bool bControl_Syringe = Sequences[i].Steps[j].Control_Syringe == other.Sequences[i].Steps[j].Control_Syringe;
                                varSame.Add("Control_Syringe", bControl_Syringe);
                                bool bControl_Valve = Sequences[i].Steps[j].Control_Valve == other.Sequences[i].Steps[j].Control_Valve;
                                varSame.Add("Control_Valve", bControl_Valve);
                                bool bEnabled = Sequences[i].Steps[j].Enabled == other.Sequences[i].Steps[j].Enabled;
                                varSame.Add("Enabled", bEnabled);
                                bool bEndCheckInThisStep = Sequences[i].Steps[j].EndCheckInThisStep == other.Sequences[i].Steps[j].EndCheckInThisStep;
                                varSame.Add("EndCheckInThisStep", bEndCheckInThisStep);
                                bool bStepEndCheck_TimeDelay_Enabled = Sequences[i].Steps[j].StepEndCheck.TimeDelay.Enabled == other.Sequences[i].Steps[j].StepEndCheck.TimeDelay.Enabled;
                                varSame.Add("bStepEndCheck_TimeDelay_Enabled", bStepEndCheck_TimeDelay_Enabled);
                                bool bStepEndCheck_TimeDelay_Time = Sequences[i].Steps[j].StepEndCheck.TimeDelay.Time == other.Sequences[i].Steps[j].StepEndCheck.TimeDelay.Time;
                                varSame.Add("bStepEndCheck_TimeDelay_Time", bStepEndCheck_TimeDelay_Time);
                                bool bStepEndCheck_Sensor_Enabled = Sequences[i].Steps[j].StepEndCheck.SensorDetect.Enabled == other.Sequences[i].Steps[j].StepEndCheck.SensorDetect.Enabled;
                                varSame.Add("bStepEndCheck_Sensor_Enabled", bStepEndCheck_Sensor_Enabled);
                                string A_Sensor = Sequences[i].Steps[j].StepEndCheck.SensorDetect.SensorNames;
                                string B_Sensor = other.Sequences[i].Steps[j].StepEndCheck.SensorDetect.SensorNames;
                                bool bStepEndCheck_Sensor_Names = ((A_Sensor == null || A_Sensor == "") && (B_Sensor == null || B_Sensor == "")) || (A_Sensor == B_Sensor);
                                varSame.Add("bStepEndCheck_Sensor_Names", bStepEndCheck_Sensor_Names);
                                bool bStepEndCheck_Syringe_Enabled = Sequences[i].Steps[j].StepEndCheck.PositionSync.Enabled == other.Sequences[i].Steps[j].StepEndCheck.PositionSync.Enabled;
                                varSame.Add("bStepEndCheck_Syringe_Enabled", bStepEndCheck_Syringe_Enabled);
                                bool bIsTitration = Sequences[i].Steps[j].IsTitration == other.Sequences[i].Steps[j].IsTitration;
                                varSame.Add("IsTitration", bIsTitration);
                                bool bName = Sequences[i].Steps[j].Name == other.Sequences[i].Steps[j].Name;
                                varSame.Add("Name", bName);
                                bool bNo = Sequences[i].Steps[j].No == other.Sequences[i].Steps[j].No;
                                varSame.Add("No", bNo);
                                bool bTitrationRef = Sequences[i].Steps[j].TitrationRef == other.Sequences[i].Steps[j].TitrationRef;
                                varSame.Add("TitrationRef", bTitrationRef);

                                if (varSame.ContainsValue(false) == true)
                                { 
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }

                        if (stepSame.Contains(false) == true)
                        {
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            bool bOtherNull = other != null;
            bool bNo1 = No == other.No;
            bool bName1 = Name == other.Name;
            bool bSeq = Sequences.SequenceEqual(other.Sequences);            
            */

            return No == other.No &&
                   Name == other.Name &&
                   Sequences.SequenceEqual(other.Sequences);    // #. valid? Mixer 객체에 IEquatable을 구현했기 때문에 가능하다. (확인필요)
        }

        public override int GetHashCode()
        {
            int hashCode = -1337920303;
            hashCode = hashCode * -1521134295 + No.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Sequence>>.Default.GetHashCode(Sequences);
            return hashCode;
        }

        public static bool operator ==(RecipeObj left, RecipeObj right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(RecipeObj left, RecipeObj right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    [XmlRoot("Sequence")]
    public class Sequence : ICloneable, IEquatable<Sequence>
    {
        [XmlElement("No")]
        public int No { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlArray("StepList")]
        [XmlArrayItem("Step", typeof(Step))]
        public List<Step> Steps;

        public Step Get_Step(int stepNo)
        {
            if (Steps != null && Steps.Count > 0)
            {
                List<Step> steps = Steps.Where(step => step.No == stepNo).ToList();
                if (steps.Count == 1)
                {
                    return steps[0];
                }
            }
            return null;
        }

        public object Clone()
        {
            Sequence clone = new Sequence();
            clone.No = No;
            clone.Name = Name;
            clone.Steps = new List<Step>();
            if (Steps != null && Steps.Count > 0)
            {
                for (int i = 0; i < Steps.Count; i++)
                {
                    clone.Steps.Add((Step)Steps[i].Clone());
                }
            }
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Sequence);
        }

        public bool Equals(Sequence other)
        {
            if (other == null)
            {
                return false;
            }
            return No == other.No &&
                   Name == other.Name &&
                   Steps.SequenceEqual(other.Steps);   // #. valid? Mixer 객체에 IEquatable을 구현했기 때문에 가능하다. (확인필요)
        }

        public override int GetHashCode()
        {
            int hashCode = -667868540;
            hashCode = hashCode * -1521134295 + No.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Step>>.Default.GetHashCode(Steps);
            return hashCode;
        }

        public static bool operator ==(Sequence left, Sequence right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Sequence left, Sequence right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class Step : ICloneable, IEquatable<Step>
    {
        [XmlElement("No")]
        public int No { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Enabled")]
        public bool Enabled { get; set; }

        [XmlElement("EndCheckInThisStep")]
        public bool EndCheckInThisStep { get; set; }

        [XmlElement("TitrationRefFileName")]
        public string TitrationRefFileName { get; set; }

        [XmlArray("ValveList")]
        [XmlArrayItem("Valve", typeof(Valve))]
        public List<Valve> Valves;

        [XmlArray("SyringeList")]
        [XmlArrayItem("Syringe", typeof(Syringe))]
        public List<Syringe> Syringes;

        [XmlArray("MixerList")]
        [XmlArrayItem("Mixer", typeof(Mixer))]
        public List<Mixer> Mixers;

        [XmlElement("StepEndCheck")]
        public StepEndCheck StepEndCheck;

        public TitrationRef TitrationRef = new TitrationRef();

        public bool Control_Valve
        {
            get
            {
                if (Valves != null && Valves.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Control_Syringe
        {
            get
            {
                if (Syringes != null && Syringes.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Control_Mixer
        {
            get
            {
                if (Mixers != null && Mixers.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsTitration
        {
            get
            {
                return TitrationRefFileName != null && TitrationRefFileName != "";
            }
        }

        public Step()
        {
        }

        public void Create(int stepNo, string titrationRef)
        {
            No = stepNo;
            Name = $"Step {stepNo}";
            TitrationRefFileName = titrationRef;

            Enabled = true;

            var mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
            DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;

            if (string.IsNullOrEmpty(TitrationRefFileName) == true)
            {
                // Valves (Solenoids)
                Valves = new List<Valve>();
                var Elems_BitList = drvLT.Get_Bits((int)DrvMB_L_Titrator.LineOrder.Solenoid_Output);
                for (int i = 0; i < Elems_BitList.Count; i++)
                {
                    Valve valve = new Valve();
                    valve.Name = Elems_BitList[i].LogicalName;
                    valve.Set_Condition(false);

                    Valves.Add(valve);
                }

                // Syringes
                Syringes = new List<Syringe>();
                var Elem_Syringe_1 = drvLT.Get_Syringe((int)DrvMB_L_Titrator.LineOrder.Syringe_1);
                Syringe syringe_1 = new Syringe();
                syringe_1.Name = Elem_Syringe_1.LogicalName;
                syringe_1.Condition = "None,None,0,0";
                Syringes.Add(syringe_1);
                var Elem_Syringe_2 = drvLT.Get_Syringe((int)DrvMB_L_Titrator.LineOrder.Syringe_2);
                Syringe syringe_2 = new Syringe();
                syringe_2.Name = Elem_Syringe_2.LogicalName;
                syringe_2.Condition = "None,None,0,0";
                Syringes.Add(syringe_2);

                // Mixer
                Mixers = new List<Mixer>();
                var Elem_Mixer = drvLT.Get_Analog((int)DrvMB_L_Titrator.LineOrder.Mixer_Duty);
                Mixer mixer = new Mixer();
                mixer.Name = Elem_Mixer.LogicalName;
                mixer.Condition = "0";
                Mixers.Add(mixer);
            }
            else
            {
                TitrationRef = new TitrationRef();
            }

            StepEndCheck = new StepEndCheck();
        }

        public void Load_TitrationRef()
        {
            TitrationRef = LT_Recipe.Load_TitrationRef(this);
        }

        public object Clone()
        {
            Step clone = new Step();
            clone.No = No;
            clone.Name = Name;
            clone.Enabled = Enabled;
            clone.EndCheckInThisStep = EndCheckInThisStep;
            clone.TitrationRefFileName = TitrationRefFileName;
            clone.Valves = new List<Valve>();
            if (Valves != null && Valves.Count > 0)
            {
                for (int i = 0; i < Valves.Count; i++)
                {
                    clone.Valves.Add((Valve)Valves[i].Clone());
                }
            }
            clone.Syringes = new List<Syringe>();
            if (Syringes != null && Syringes.Count > 0)
            {
                for (int i = 0; i < Syringes.Count; i++)
                {
                    clone.Syringes.Add((Syringe)Syringes[i].Clone());
                }
            }
            clone.Mixers = new List<Mixer>();
            if (Mixers != null && Mixers.Count > 0)
            {
                for (int i = 0; i < Mixers.Count; i++)
                {
                    clone.Mixers.Add((Mixer)Mixers[i].Clone());
                }
            }
            if (StepEndCheck != null)
            {
                clone.StepEndCheck = (StepEndCheck)StepEndCheck.Clone();
            }
            if (TitrationRef != null)
            {
                clone.TitrationRef = (TitrationRef)TitrationRef.Clone();
            }
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Step);
        }

        public bool Equals(Step other)
        {
            if (other == null)
            {
                return false;
            }
            return No == other.No &&
                   Name == other.Name &&
                   Enabled == other.Enabled &&
                   EndCheckInThisStep == other.EndCheckInThisStep &&
                   TitrationRefFileName == other.TitrationRefFileName &&
                   Valves.SequenceEqual(other.Valves) &&
                   Syringes.SequenceEqual(other.Syringes) &&
                   Mixers.SequenceEqual(other.Mixers) &&
                   ((StepEndCheck == null && other.StepEndCheck == null) || (StepEndCheck != null && other.StepEndCheck != null && StepEndCheck.Equals(other.StepEndCheck))) &&
                   Control_Valve == other.Control_Valve &&
                   Control_Syringe == other.Control_Syringe &&
                   Control_Mixer == other.Control_Mixer &&
                   IsTitration == other.IsTitration &&
                    TitrationRef == other.TitrationRef;
        }

        public override int GetHashCode()
        {
            int hashCode = -1881431569;
            hashCode = hashCode * -1521134295 + No.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EndCheckInThisStep.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TitrationRefFileName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Valve>>.Default.GetHashCode(Valves);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Syringe>>.Default.GetHashCode(Syringes);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Mixer>>.Default.GetHashCode(Mixers);
            hashCode = hashCode * -1521134295 + EqualityComparer<StepEndCheck>.Default.GetHashCode(StepEndCheck);
            hashCode = hashCode * -1521134295 + Control_Valve.GetHashCode();
            hashCode = hashCode * -1521134295 + Control_Syringe.GetHashCode();
            hashCode = hashCode * -1521134295 + Control_Mixer.GetHashCode();
            hashCode = hashCode * -1521134295 + IsTitration.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Step left, Step right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Step left, Step right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class Valve : ICloneable, IEquatable<Valve>
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Condition")]
        public string Condition { get; set; }

        public bool Get_Condition()
        {
            bool bCondition = false;
            if (Condition != "" && Condition == "ON")
            {
                bCondition = true;
            }
            return bCondition;
        }

        public void Set_Condition(bool bCondition)
        {
            Condition = bCondition == true ? "ON" : "OFF";
        }

        public object Clone()
        {
            Valve clone = new Valve();
            clone.Name = Name;
            clone.Condition = Condition;
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Valve);
        }

        public bool Equals(Valve other)
        {
            if (other == null)
            {
                return false;
            }
            return Name == other.Name &&
                   Condition == other.Condition;
        }

        public override int GetHashCode()
        {
            int hashCode = -322663852;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Condition);
            return hashCode;
        }

        public static bool operator ==(Valve left, Valve right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Valve left, Valve right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class Syringe : ICloneable, IEquatable<Syringe>
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Condition")]
        public string Condition { get; set; }

        private int TargetVolume_Raw;
        private double TargetVolume_mL;

        public MB_SyringeFlow Get_Flow()
        {
            if (IsValidCondition() == false)
            {
                return MB_SyringeFlow.None;
            }

            MB_SyringeFlow flow = MB_SyringeFlow.None;
            string sFlow = Condition.Split(',')[0].ToUpper();
            switch (sFlow)
            {
                case "PICK":
                    flow = MB_SyringeFlow.Pick;
                    break;

                case "DISPENSE":
                    flow = MB_SyringeFlow.Dispense;
                    break;
            }

            return flow;
        }

        public MB_SyringeDirection Get_Direction()
        {
            if (IsValidCondition() == false)
            {
                return MB_SyringeDirection.None;
            }

            MB_SyringeDirection dir = MB_SyringeDirection.None;
            string sDir = Condition.Split(',')[1].ToUpper();
            switch (sDir)
            {
                case "IN":
                    dir = MB_SyringeDirection.In;
                    break;

                case "OUT":
                    dir = MB_SyringeDirection.Out;
                    break;

                case "EXT":
                    dir = MB_SyringeDirection.Ext;
                    break;
            }

            return dir;
        }

        public bool IsValidCondition()
        {
            if (string.IsNullOrEmpty(Condition) == true)
            {
                return false;
            }
            if (Condition.Split(',').Length != 4)
            {
                return false;
            }
            return true;
        }

        public int Get_Speed()
        {
            if (IsValidCondition() == false)
            {
                return 0;
            }

            if (int.TryParse(Condition.Split(',')[2], out int speed) == true)
            {
                return speed;
            }
            return 0;
        }

        public double Get_Volume_mL()
        {
            if (IsValidCondition() == false)
            {
                return 0;
            }

            if (double.TryParse(Condition.Split(',')[3], out double volume) == true)
            {
                return volume;
            }

            return 0;
        }

        public void Set_TargetVolume_mL(double vol)
        {
            TargetVolume_mL = vol;
        }

        public double Get_TargetVolume_mL()
        {
            return TargetVolume_mL;
        }

        public void Set_TargetVolume_Raw(int vol)
        {
            TargetVolume_Raw = vol;
        }

        public int Get_TargetVolume_Raw()
        {
            return TargetVolume_Raw;
        }

        public object Clone()
        {
            Syringe clone = new Syringe();
            clone.Name = Name;
            clone.Condition = Condition;
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Syringe);
        }

        public bool Equals(Syringe other)
        {
            if (other == null)
            {
                return false;
            }
            return Name == other.Name &&
                   Condition == other.Condition &&
                   TargetVolume_Raw == other.TargetVolume_Raw &&
                   TargetVolume_mL == other.TargetVolume_mL;
        }

        public override int GetHashCode()
        {
            int hashCode = 1362768905;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Condition);
            hashCode = hashCode * -1521134295 + TargetVolume_Raw.GetHashCode();
            hashCode = hashCode * -1521134295 + TargetVolume_mL.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Syringe left, Syringe right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Syringe left, Syringe right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class Mixer : ICloneable, IEquatable<Mixer>
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Condition")]
        public string Condition { get; set; }

        public int Get_Duty()
        {
            if (Condition != null && Condition != "" && int.TryParse(Condition, out int duty) == true)
            {
                return duty;
            }
            return 0;
        }

        public void Set_Duty(int duty)
        {
            Condition = duty.ToString();
        }

        public object Clone()
        {
            Mixer clone = new Mixer();
            clone.Name = Name;
            clone.Condition = Condition;
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Mixer);
        }

        public bool Equals(Mixer other)
        {
            if (other == null)
            {
                return false;
            }
            return Name == other.Name &&
                   Condition == other.Condition;
        }

        public override int GetHashCode()
        {
            int hashCode = -322663852;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Condition);
            return hashCode;
        }

        public static bool operator ==(Mixer left, Mixer right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Mixer left, Mixer right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class StepEndCheck : ICloneable, IEquatable<StepEndCheck>
    {
        [XmlElement("TimeDelay")]
        public StepEndCheck_TimeDelay TimeDelay { get; set; }

        [XmlElement("SensorDetect")]
        public StepEndCheck_Sensor SensorDetect { get; set; }

        [XmlElement("PositionSync")]
        public StepEndCheck_PositionSync PositionSync { get; set; }

        public StepEndCheck()
        {
            TimeDelay = new StepEndCheck_TimeDelay();
            SensorDetect = new StepEndCheck_Sensor();
            PositionSync = new StepEndCheck_PositionSync();
        }

        public bool Get_TimeDelay(out int delay)
        {
            delay = 0;
            if (TimeDelay != null)
            {
                if (TimeDelay.Enabled == true)
                {
                    delay = TimeDelay.Time;
                    return true;
                }
            }
            return false;
        }

        public bool Get_Sensors(out List<string> sensorNames)
        {
            sensorNames = new List<string>();
            if (SensorDetect != null)
            {
                if (SensorDetect.Enabled == true)
                {
                    sensorNames = SensorDetect.SensorNames.Split(',').ToList();
                    return true;
                }
            }
            return false;
        }

        public bool Get_PositionSync(out bool sync)
        {
            sync = true;
            if (PositionSync != null)
            {
                return PositionSync.Enabled;
            }
            return false;
        }

        public object Clone()
        {
            StepEndCheck clone = new StepEndCheck();
            clone.TimeDelay = (StepEndCheck_TimeDelay)TimeDelay?.Clone();
            clone.SensorDetect = (StepEndCheck_Sensor)SensorDetect?.Clone();
            clone.PositionSync = (StepEndCheck_PositionSync)PositionSync?.Clone();
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StepEndCheck);
        }

        public bool Equals(StepEndCheck other)
        {
            if (other == null)
            {
                return false;
            }
            return TimeDelay.Equals(other.TimeDelay) &&
                   SensorDetect.Equals(other.SensorDetect) &&
                   PositionSync.Equals(other.PositionSync);
        }

        public override int GetHashCode()
        {
            int hashCode = 1121910608;
            hashCode = hashCode * -1521134295 + EqualityComparer<StepEndCheck_TimeDelay>.Default.GetHashCode(TimeDelay);
            hashCode = hashCode * -1521134295 + EqualityComparer<StepEndCheck_Sensor>.Default.GetHashCode(SensorDetect);
            hashCode = hashCode * -1521134295 + EqualityComparer<StepEndCheck_PositionSync>.Default.GetHashCode(PositionSync);
            return hashCode;
        }

        public static bool operator ==(StepEndCheck left, StepEndCheck right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(StepEndCheck left, StepEndCheck right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class StepEndCheck_TimeDelay : ICloneable, IEquatable<StepEndCheck_TimeDelay>
    {
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }
        [XmlElement("Time")]
        public int Time { get; set; }

        public object Clone()
        {
            StepEndCheck_TimeDelay clone = new StepEndCheck_TimeDelay();
            clone.Enabled = Enabled;
            clone.Time = Time;
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StepEndCheck_TimeDelay);
        }

        public bool Equals(StepEndCheck_TimeDelay other)
        {
            if (other == null)
            {
                return false;
            }
            return Enabled == other.Enabled &&
                   Time == other.Time;
        }

        public override int GetHashCode()
        {
            int hashCode = 2044594162;
            hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + Time.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StepEndCheck_TimeDelay left, StepEndCheck_TimeDelay right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(StepEndCheck_TimeDelay left, StepEndCheck_TimeDelay right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class StepEndCheck_Sensor : ICloneable, IEquatable<StepEndCheck_Sensor>
    {
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }
        [XmlElement("SensorNames")]
        public string SensorNames { get; set; }
        public object Clone()
        {
            StepEndCheck_Sensor clone = new StepEndCheck_Sensor();
            clone.Enabled = Enabled;
            clone.SensorNames = SensorNames;
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StepEndCheck_Sensor);
        }

        public bool Equals(StepEndCheck_Sensor other)
        {
            if (other == null)
            {
                return false;
            }
            return ((SensorNames == null || SensorNames == "") && (other.SensorNames == null || other.SensorNames == "")) || (SensorNames == other.SensorNames);
        }

        public override int GetHashCode()
        {
            int hashCode = -460372585;
            hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SensorNames);
            return hashCode;
        }

        public static bool operator ==(StepEndCheck_Sensor left, StepEndCheck_Sensor right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(StepEndCheck_Sensor left, StepEndCheck_Sensor right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class StepEndCheck_PositionSync : ICloneable, IEquatable<StepEndCheck_PositionSync>
    {
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }

        public object Clone()
        {
            StepEndCheck_PositionSync clone = new StepEndCheck_PositionSync();
            clone.Enabled = Enabled;
            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StepEndCheck_PositionSync);
        }

        public bool Equals(StepEndCheck_PositionSync other)
        {
            if (other == null)
            {
                return false;
            }
            return Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            return -1711725586 + Enabled.GetHashCode();
        }

        public static bool operator ==(StepEndCheck_PositionSync left, StepEndCheck_PositionSync right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(StepEndCheck_PositionSync left, StepEndCheck_PositionSync right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    [XmlRoot("TitrationRef")]
    public class TitrationRef : ICloneable, IEquatable<TitrationRef>
    {
        [XmlElement("SampleName")]
        public string SampleName { get; set; }

        [XmlElement("MaxIterationCount")]
        public int MaxIterationCount { get; set; }

        [XmlElement("ScaleFactor")]
        public double ScaleFactor;

        [XmlElement("ResultUnit")]
        public string ResultUnit;

        [XmlElement("MixingTime_AfterOffset")]
        public int MixingTime_AfterOffset { get; set; }

        [XmlElement("MixingTime_General")]
        public int MixingTime_General { get; set; }

        [XmlElement("AnalogInfo")]
        public AnalogInfo AnalogInfo;

        [XmlElement("InjectionInfo")]
        public InjectionInfo InjectionInfo;

        private int IterationCount = 0;
        private double TotalInjectionVolume_mL = 0;

        private Stopwatch MixingStopWatch = new Stopwatch();
        public bool IsMixing { get { return MixingStopWatch.IsRunning; } }
        public int MixingTimeElapsed { get { return (int)MixingStopWatch.ElapsedMilliseconds; } }

        private bool IsInjecting = false;
        private bool Flag_RefillSyringe = false;

        private int TargetVolume_Digit = 0;
        private double TargetVolume_mL = 0;
        private double RecentInjected_mL = 0;

        private bool Flag_NotifyResult = false;

        public TitrationRef()
        {
            AnalogInfo = new AnalogInfo();
            InjectionInfo = new InjectionInfo();
        }

        public void IncrementInterationCount()
        {
            ++IterationCount;
        }

        public int Get_IterationCount()
        {
            return IterationCount;
        }

        public void AddInjectionVolume(double injVol)
        {
            RecentInjected_mL = injVol;
            TotalInjectionVolume_mL += injVol;
        }

        public double Get_TotalInjectionVolume_mL()
        {
            return TotalInjectionVolume_mL;
        }

        public double Get_RecentInjectedVolume_mL()
        {
            return RecentInjected_mL;
        }

        public void StartTimer()
        {
            MixingStopWatch.Reset();
            MixingStopWatch.Start();
        }

        public void StopTimer()
        {
            MixingStopWatch.Stop();
        }

        public void StartRun()
        {
            IsInjecting = true;
        }

        public void StopRun()
        {
            IsInjecting = false;
        }

        public bool Get_IsInjecting()
        {
            return IsInjecting;
        }

        public void Set_RefillSyringeFlag()
        {
            Flag_RefillSyringe = true;
        }

        public void Reset_RefileSyringeFlag()
        {
            Flag_RefillSyringe = false;
        }

        public bool Get_Flag_RefillSyringe()
        {
            return Flag_RefillSyringe;
        }

        public void Set_TargetVolume_Digit(int vol_Digit)
        {
            TargetVolume_Digit = vol_Digit;
        }

        public int Get_TargetVolume_Digit()
        {
            return TargetVolume_Digit;
        }

        public void Set_TargetVolume_mL(double vol_mL)
        {
            TargetVolume_mL = vol_mL;
        }

        public double Get_TargetVolume_mL()
        {
            return TargetVolume_mL;
        }

        public void Set_Flag_NotifyResult()
        {
            Flag_NotifyResult = true;
        }

        public bool Get_NotifyResultFlag()
        {
            return Flag_NotifyResult;
        }

        public double Get_Concentration()
        {
            return ScaleFactor * TotalInjectionVolume_mL;
        }

        public object Clone()
        {
            TitrationRef clone = new TitrationRef();

            clone.SampleName = SampleName;
            clone.ScaleFactor = ScaleFactor;
            clone.ResultUnit = ResultUnit;

            clone.MixingTime_AfterOffset = MixingTime_AfterOffset;
            clone.MixingTime_General = MixingTime_General;

            clone.MaxIterationCount = MaxIterationCount;

            clone.AnalogInfo = (AnalogInfo)AnalogInfo.Clone();
            clone.InjectionInfo = (InjectionInfo)InjectionInfo.Clone();

            return clone;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TitrationRef);
        }

        public bool Equals(TitrationRef other)
        {
            if (other == null)
            {
                return false;
            }
            return SampleName == other.SampleName &&
                   MaxIterationCount == other.MaxIterationCount &&
                   ScaleFactor == other.ScaleFactor &&
                   ResultUnit == other.ResultUnit &&
                   MixingTime_AfterOffset == other.MixingTime_AfterOffset &&
                   MixingTime_General == other.MixingTime_General &&
                   AnalogInfo.Equals(other.AnalogInfo) &&
                   InjectionInfo.Equals(other.InjectionInfo) &&
                   IterationCount == other.IterationCount &&
                   TotalInjectionVolume_mL == other.TotalInjectionVolume_mL &&
                   //MixingStopWatch.Equals(other.MixingStopWatch) &&
                   IsMixing == other.IsMixing &&
                   MixingTimeElapsed == other.MixingTimeElapsed &&
                   IsInjecting == other.IsInjecting &&
                   Flag_RefillSyringe == other.Flag_RefillSyringe &&
                   TargetVolume_Digit == other.TargetVolume_Digit &&
                   TargetVolume_mL == other.TargetVolume_mL &&
                   RecentInjected_mL == other.RecentInjected_mL &&
                   Flag_NotifyResult == other.Flag_NotifyResult;
        }

        public override int GetHashCode()
        {
            int hashCode = 553409122;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SampleName);
            hashCode = hashCode * -1521134295 + MaxIterationCount.GetHashCode();
            hashCode = hashCode * -1521134295 + ScaleFactor.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ResultUnit);
            hashCode = hashCode * -1521134295 + MixingTime_AfterOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + MixingTime_General.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<AnalogInfo>.Default.GetHashCode(AnalogInfo);
            hashCode = hashCode * -1521134295 + EqualityComparer<InjectionInfo>.Default.GetHashCode(InjectionInfo);
            hashCode = hashCode * -1521134295 + IterationCount.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalInjectionVolume_mL.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Stopwatch>.Default.GetHashCode(MixingStopWatch);
            hashCode = hashCode * -1521134295 + IsMixing.GetHashCode();
            hashCode = hashCode * -1521134295 + MixingTimeElapsed.GetHashCode();
            hashCode = hashCode * -1521134295 + IsInjecting.GetHashCode();
            hashCode = hashCode * -1521134295 + Flag_RefillSyringe.GetHashCode();
            hashCode = hashCode * -1521134295 + TargetVolume_Digit.GetHashCode();
            hashCode = hashCode * -1521134295 + TargetVolume_mL.GetHashCode();
            hashCode = hashCode * -1521134295 + RecentInjected_mL.GetHashCode();
            hashCode = hashCode * -1521134295 + Flag_NotifyResult.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(TitrationRef left, TitrationRef right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(TitrationRef left, TitrationRef right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class AnalogInfo : ICloneable, IEquatable<AnalogInfo>
    {
        [XmlElement("AnalogInputLogicalName")]
        public string AnalogInputLogicalName { get; set; }

        [XmlElement("TargetValue")]
        public double TargetValue { get; set; }

        [XmlElement("EndValue")]
        public double EndValue { get; set; }

        [XmlElement("Unit")]
        public string Unit { get; set; }

        public object Clone()
        {
            AnalogInfo aInfo = new AnalogInfo();
            aInfo.AnalogInputLogicalName = AnalogInputLogicalName;
            aInfo.TargetValue = TargetValue;
            aInfo.EndValue = EndValue;
            aInfo.Unit = Unit;

            return aInfo;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnalogInfo);
        }

        public bool Equals(AnalogInfo other)
        {
            if (other == null)
            {
                return false;
            }
            return AnalogInputLogicalName == other.AnalogInputLogicalName &&
                   TargetValue == other.TargetValue &&
                   EndValue == other.EndValue &&
                   Unit == other.Unit;
        }

        public override int GetHashCode()
        {
            int hashCode = -479150584;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AnalogInputLogicalName);
            hashCode = hashCode * -1521134295 + TargetValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EndValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Unit);
            return hashCode;
        }

        public static bool operator ==(AnalogInfo left, AnalogInfo right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(AnalogInfo left, AnalogInfo right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class InjectionInfo : ICloneable, IEquatable<InjectionInfo>
    {
        [XmlElement("ReagentName")]
        public string ReagentName { get; set; }

        [XmlElement("ReagentSyringeLogicalName")]
        public string ReagentSyringeLogicalName { get; set; }

        [XmlElement("Offset")]
        public double Offset { get; set; }

        [XmlElement("IncThreshold_ChangeToMiddle")]
        public double IncThreshold_ChangeToMiddle { get; set; }

        [XmlElement("IncThreshold_ChangeToSmall")]
        public double IncThreshold_ChangeToSmall { get; set; }

        [XmlElement("Inc_Large")]
        public double Inc_Large { get; set; }

        [XmlElement("Inc_Middle")]
        public double Inc_Middle { get; set; }

        [XmlElement("Inc_Small")]
        public double Inc_Small { get; set; }

        public object Clone()
        {
            InjectionInfo iInfo = new InjectionInfo();
            iInfo.ReagentName = ReagentName;
            iInfo.ReagentSyringeLogicalName = ReagentSyringeLogicalName;
            iInfo.Offset = Offset;
            iInfo.IncThreshold_ChangeToMiddle = IncThreshold_ChangeToMiddle;
            iInfo.IncThreshold_ChangeToSmall = IncThreshold_ChangeToSmall;
            iInfo.Inc_Large = Inc_Large;
            iInfo.Inc_Middle = Inc_Middle;
            iInfo.Inc_Small = Inc_Small;

            return iInfo;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InjectionInfo);
        }

        public bool Equals(InjectionInfo other)
        {
            if (other == null)
            {
                return false;
            }
            return ReagentName == other.ReagentName &&
                   ReagentSyringeLogicalName == other.ReagentSyringeLogicalName &&
                   Offset == other.Offset &&
                   IncThreshold_ChangeToMiddle == other.IncThreshold_ChangeToMiddle &&
                   IncThreshold_ChangeToSmall == other.IncThreshold_ChangeToSmall &&
                   Inc_Large == other.Inc_Large &&
                   Inc_Middle == other.Inc_Middle &&
                   Inc_Small == other.Inc_Small;
        }

        public override int GetHashCode()
        {
            int hashCode = -598802579;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReagentName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReagentSyringeLogicalName);
            hashCode = hashCode * -1521134295 + Offset.GetHashCode();
            hashCode = hashCode * -1521134295 + IncThreshold_ChangeToMiddle.GetHashCode();
            hashCode = hashCode * -1521134295 + IncThreshold_ChangeToSmall.GetHashCode();
            hashCode = hashCode * -1521134295 + Inc_Large.GetHashCode();
            hashCode = hashCode * -1521134295 + Inc_Middle.GetHashCode();
            hashCode = hashCode * -1521134295 + Inc_Small.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(InjectionInfo left, InjectionInfo right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(InjectionInfo left, InjectionInfo right)
        {
            return !(left == right);
        }
    }
}
