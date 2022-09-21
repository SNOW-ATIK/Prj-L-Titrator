using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Titrator
{
    public class PreDef
    {
        public const string Path_Log = "Log";

        public const string Path_Config = "Config";
        public const string Path_Config_Device = @"Config\Device";
        public const string Path_Recipe = "Recipe";
        public const string Path_PreDefinedSeq = @"Recipe\PreDefinedSeq";
        public const string Path_Recipe_TitrationRef = @"Recipe\TitrationRef";
        public const string Path_History = "History";
        public const string FileName_Elem_SerialPort = "Element_SerialPort.xml";

        public static Color MenuBG_Unselect = Color.White;
        public static Color MenuBG_Select = Color.MediumSeaGreen;
        public static Color Light_Green = Color.FromArgb(200, Color.MediumSeaGreen.G + 50, 200);
        public static Color Light_Red = Color.Pink;
    }

    public enum FluidicsMsg
    { 
        None,
        Initialize,
        Measure,
        Pause,
        Resume,
        Stop
    }

    public enum FluidicsState
    { 
        None,
        Idle,
        Run
    }

    public enum FluidicsRunState
    { 
        None,
        Initializing,
        Measuring
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

        public static bool PEnd_Raw(int cur_raw, int dst_raw, double band_mL, double scaleFactor)
        {
            int nDiff = Math.Abs(cur_raw - dst_raw);
            double dBand_raw = band_mL * scaleFactor;
            bool bPEnd = nDiff < dBand_raw;

            ATIK.Log.WriteLog("Seq", $"SyringePEND> Cur_Raw={cur_raw}, Dst_Raw={dst_raw}, Bandwidth_Raw={dBand_raw}, Diff_Raw={nDiff}, PEND={bPEnd}");

            return bPEnd;
        }
    }
}
