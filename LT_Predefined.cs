using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ATIK;

namespace L_Titrator
{
    public static class PreDef
    {
        public const string Path_Log = "Log";

        public const string Path_Config = "Config";
        public const string Path_Config_Device = @"Config\Device";
        public const string Path_Recipe = "Recipe";
        public const string Path_Recipe_HotKey = @"Recipe\HotKey";
        public const string Path_PreFixSequence = @"Recipe\PreFixSequence";
        public const string Path_PreFixAnalyzeRef = @"Recipe\PreFixAnalyzeRef";
        public const string Path_Recipe_AnalyzeRef = @"Recipe\AnalyzeRef";
        public const string Path_History_Alarm = @"History\Alarm";
        public const string Path_History_Data = @"History\Data";
        public const string Path_History = @"History\Backup";

        public const string FileName_AlarmHistory = "AlarmHistory.csv";
        public const string FileName_Elem_SerialPort = "Element_SerialPort.xml";

        public static Color MenuBG_Unselect = Color.White;
        public static Color MenuBG_Select = Color.MediumSeaGreen;
        public static Color Light_Green = Color.FromArgb(200, Color.MediumSeaGreen.G + 50, 200);
        public static Color Light_Red = Color.Pink;

        public const bool Interlock_Occur_BooleanValue = false;

        public static class ElemLogicalName
        {
            public static class Syringe
            {
                public const string Syringe_1 = "Syringe_1";
                public const string Syringe_2 = "Syringe_2";
            }

            public static class AlarmInput
            {
                public const string Leak_1 = "Leak_1";
                public const string Level_1 = "Level_1";
                public const string Level_2 = "Level_2";
                public const string Door = "Door";
                public const string Exhaust = "Exhaust";
                public const string Interlock = "Interlock";
                public const string EMG_Stop = "EMG_Stop";
                public const string Fan_1 = "Fan_1";
                public const string Fan_2 = "Fan_2";
                public const string Fan_3 = "Fan_3";
                public const string Fan_4 = "Fan_4";
            }

            public static class SolenoidOutput
            {
                public const string DualPort_3Way_DIW_6Way = "3Way_DIW_6Way";
                public const string DualPort_3Way_DIW_Vessel = "3Way_DIW_Vessel";
                public const string DualPort_3Way_Sample_6Way = "3Way_Sample_6Way";
                public const string Ceric_To_3Way = "Ceric_To_3Way";
                public const string DrainPair_Open = "DrainPair_Open";
                public const string DrainPair_Close = "DrainPair_Close";
                public const string Valve6Way_Sample_To_Vessel = "6WayPair_Sample_To_Vessel";
                public const string Valve6Way_Sample_To_Loop = "6WayPair_Sample_To_Loop";
                public const string Fluidics_Purge = "Fluidics_Purge";
                public const string ElecBox_Purge = "ElecBox_Purge";
                public const string DualPort_3Way_Sample_Vessel = "3Way_Sample_Vessel";
            }

            public static class AnalogInput
            {
                public const string Temperature_RTD = "RTD";
                public const string Temperature_TC = "TC";
                public const string Mixer_RPM = "RPM";
            }

            public static class AnalogOutput
            {
                public const string Result_1 = "Result_1";
                public const string Result_2 = "Result_2";
                public const string Result_3 = "Result_3";
                public const string Result_4 = "Result_4";
                public const string Mixer_Duty = "Duty";
            }
        }

        public static class LifeTimeParts
        {
            public const string ORP_Electrode = "ORP Electrode";
            public const string Vessel = "Vessel";
            public const string DualPort_3Way_DIW = "3Way-DIW";
            public const string DualPort_3Way_Sample = "3Way-Sample";
            public const string SiglePort_3Way_VLD = "3Way-Validation";
            public const string Valve6Way = "6Way-Capture";
            public const string ValveDrain = "Drain Valve";
            public const string Syringe1 = "Syringe1";
            public const string Syringe2 = "Syringe2";
        }
    }

    public class GlbVar
    {
        private static object objLock_Authority = new object();
        private static UserAuthority _Authority = UserAuthority.User;
        public static UserAuthority CurrentAuthority
        {
            get
            {
                lock (objLock_Authority)
                {
                    return _Authority;
                }
            }
            set
            {
                lock (objLock_Authority)
                {
                    _Authority = value;
                }
            }
        }

        private static object objLock_OnlineMode = new object();
        private static OnlineMode _OnlineMode = OnlineMode.Local;
        public static OnlineMode OnlineMode
        {
            get
            {
                lock (objLock_OnlineMode)
                {
                    return _OnlineMode;
                }
            }
            set
            {
                lock (objLock_OnlineMode)
                {
                    _OnlineMode = value;
                }
            }
        }

        private static object objLock_StartSignalReceived = new object();
        private static bool _StartSignalReceived = false;
        public static bool StartSignalReceived
        {
            get
            {
                lock (objLock_StartSignalReceived)
                {
                    return _StartSignalReceived;
                }
            }
            set
            {
                lock (objLock_StartSignalReceived)
                {
                    _StartSignalReceived = value;
                }
            }
        }

        private static object objLock_Language = new object();
        private static Language _CurrentLanguage = Language.ENG;
        public static Language CurrentLanguage
        {
            get
            {
                lock (objLock_Language)
                {
                    return _CurrentLanguage;
                }
            }
            set
            {
                lock (objLock_Language)
                {
                    _CurrentLanguage = value;
                }
            }
        }

        private static object objLock_MainState = new object();
        private static MainState _CurrentMainState = MainState.Unknown;
        public static MainState CurrentMainState
        {
            get
            {
                lock (objLock_MainState)
                {
                    return _CurrentMainState;
                }
            }
            set
            {
                lock (objLock_Language)
                {
                    _CurrentMainState = value;
                }
            }
        }
    }

    public enum FluidicsMsg
    { 
        None,
        Initialize,
        Measure,
        Abort,
        HotKey,
        Pause,
        Resume,
        Stop
    }

    public enum FluidicsState
    { 
        None,
        Idle,
        Run,
        Error,
    }

    public enum FluidicsRunState
    { 
        None,
        Measuring,
        HotKey_Running,
    }

    public enum UI_Msg
    { 
    }

    public class Util
    {
        public static string GetStringFromDateTime(DateTime dt)
        {
            return $"{dt.Year:0000}-{dt.Month:00}-{dt.Day:00} {dt.Hour:00}:{dt.Minute:00}:{dt.Second:00}";
        }

        public static DateTime GetDateTimeFromString(string str)
        {
            return DateTime.Parse(str);
        }

        public static string GetStringFromTimeSpan(TimeSpan sp)
        {
            return $"{sp.Hours:00}:{sp.Minutes:00}:{sp.Seconds:00}";
        }

        public static bool PEnd_Raw(string syringeName, int cur_raw, int dst_raw, double band_mL, double scaleFactor)
        {
            int nDiff = Math.Abs(cur_raw - dst_raw);
            double dBand_raw = band_mL * scaleFactor;
            bool bPEnd = nDiff < dBand_raw;

            ATIK.Log.WriteLog("Seq", $"{syringeName}:PEND > Cur_Raw={cur_raw}, Dst_Raw={dst_raw}, Bandwidth_Raw={dBand_raw}, Diff_Raw={nDiff}, PEND={bPEnd}");

            return bPEnd;
        }

        public static double GetInterpolatedValue(double refVal, double pX1, double pY1, double pX2, double pY2)
        {
            double dX = pX2 - pX1;
            double dY = pY2 - pY1;
            double A = dY / dX;
            double B = pY1 - A * pX1;
            double result = (refVal - B) / A;
            return result;
        }
    }
}
