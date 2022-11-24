using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ATIK;

namespace L_Titrator
{
    public class LT_Config
    {
        private static XmlCfgPrm Cfg_LT;

        public static GenericParam<Language> GenPrm_Language;
        public static GenericParam<OnlineMode> GenPrm_BootupOnlineMode;
        public static GenericParam<int> GenPrm_LogOutCheckTime;
        public static GenericParam<bool> GenPrm_InterlockEnabled;
        public static GenericParam<string> GenPrm_LogInAuthority;

        public static GenericParam<bool> GenPrm_Manual_Enabled;
        public static GenericParam<bool> GenPrm_Manual_AvailableOnRemote;
        public static GenericParam<bool> GenPrm_Manual_NotifyResult;
        public static GenericParam<bool> GenPrm_Manual_SaveHistory;

        public static GenericParam<bool> GenPrm_Periodic_Enabled;
        public static GenericParam<bool> GenPrm_Periodic_AvailableOnRemote;
        public static GenericParam<bool> GenPrm_Periodic_NotifyResult;
        public static GenericParam<bool> GenPrm_Periodic_SaveHistory;
        public static GenericParam<int> GenPrm_Periodic_RecipeNo;
        public static GenericParam<bool> GenPrm_Periodic_UseValidation;
        public static GenericParam<int> GenPrm_Periodic_Period;
        public static GenericParam<string> GenPrm_Periodic_NextMeasureTime;

        public static GenericParam<bool> GenPrm_Validation_Enabled;
        public static GenericParam<bool> GenPrm_Validation_NotifyResult;
        public const int MaxValidations = 3;
        public static Dictionary<int, (GenericParam<bool> Enabled, GenericParam<int> RecipeNo, GenericParam<int> RepeatCounts)> ValidationList = new Dictionary<int, (GenericParam<bool> Enabled, GenericParam<int> RecipeNo, GenericParam<int> RepeateCounts)>();

        public static bool Load()
        {
            bool bLoadSuccess = true;

            Cfg_LT = new XmlCfgPrm("", "SystemInfo.xml", "L_Titrator");
            if (Cfg_LT.XmlLoaded == false)
            {
                return false;
            }

            GenPrm_Language = new GenericParam<Language>(Cfg_LT, "Options", "Language");
            GenPrm_BootupOnlineMode = new GenericParam<OnlineMode>(Cfg_LT, "Options", "BootUp_OnlineMode");
            GenPrm_LogOutCheckTime = new GenericParam<int>(Cfg_LT, "Options", "LogOutCheckTime");
            GenPrm_InterlockEnabled = new GenericParam<bool>(Cfg_LT, "Options", "InterlockEnabled");
            GenPrm_LogInAuthority = new GenericParam<string>(Cfg_LT, "Options", "LogInAuthority");

            GenPrm_Manual_Enabled = new GenericParam<bool>(Cfg_LT, "Manual", "Enabled");
            GenPrm_Manual_AvailableOnRemote = new GenericParam<bool>(Cfg_LT, "Manual", "AvailableOnRemote");
            GenPrm_Manual_NotifyResult = new GenericParam<bool>(Cfg_LT, "Manual", "NotifyResult");
            GenPrm_Manual_SaveHistory = new GenericParam<bool>(Cfg_LT, "Manual", "SaveHistory");

            GenPrm_Periodic_Enabled = new GenericParam<bool>(Cfg_LT, "Periodic", "Enabled");
            GenPrm_Periodic_AvailableOnRemote = new GenericParam<bool>(Cfg_LT, "Periodic", "AvailableOnRemote");
            GenPrm_Periodic_NotifyResult = new GenericParam<bool>(Cfg_LT, "Periodic", "NotifyResult");
            GenPrm_Periodic_SaveHistory = new GenericParam<bool>(Cfg_LT, "Periodic", "SaveHistory");
            GenPrm_Periodic_RecipeNo = new GenericParam<int>(Cfg_LT, "Periodic", "RecipeNo");
            GenPrm_Periodic_UseValidation = new GenericParam<bool>(Cfg_LT, "Periodic", "UseValidation");
            GenPrm_Periodic_Period = new GenericParam<int>(Cfg_LT, "Periodic", "Period");
            GenPrm_Periodic_NextMeasureTime = new GenericParam<string>(Cfg_LT, "Periodic", "NextMeasureTime");

            GenPrm_Validation_Enabled = new GenericParam<bool>(Cfg_LT, "Validation", "Enabled");
            GenPrm_Validation_NotifyResult = new GenericParam<bool>(Cfg_LT, "Validation", "NotifyResult");
            //GenPrm_Validation_1st_Enabled = new GenericParam<bool>(Cfg_LT, "Validation", "First", "Enabled");
            //GenPrm_Validation_1st_RepeatCounts = new GenericParam<int>(Cfg_LT, "Validation", "First", "RepeatCounts");
            //GenPrm_Validation_1st_RecipeNo = new GenericParam<int>(Cfg_LT, "Validation", "First", "RecipeNo");
            //GenPrm_Validation_2nd_Enabled = new GenericParam<bool>(Cfg_LT, "Validation", "Second", "Enabled");
            //GenPrm_Validation_2nd_RepeatCounts = new GenericParam<int>(Cfg_LT, "Validation", "Second", "RepeatCounts");
            //GenPrm_Validation_2nd_RecipeNo = new GenericParam<int>(Cfg_LT, "Validation", "Second", "RecipeNo");
            //GenPrm_Validation_3rd_Enabled = new GenericParam<bool>(Cfg_LT, "Validation", "Third", "Enabled");
            //GenPrm_Validation_3rd_RepeatCounts = new GenericParam<int>(Cfg_LT, "Validation", "Third", "RepeatCounts");
            //GenPrm_Validation_3rd_RecipeNo = new GenericParam<int>(Cfg_LT, "Validation", "Third", "RecipeNo");
            for (int i = 0; i < MaxValidations; i++)
            {
                string sOrder = string.Empty;
                switch (i)
                {
                    case 0:
                        sOrder = "First";
                        break;

                    case 1:
                        sOrder = "Second";
                        break;

                    case 2:
                        sOrder = "Third";
                        break;
                }

                GenericParam<bool> enabled = new GenericParam<bool>(Cfg_LT, "Validation", sOrder, "Enabled");
                GenericParam<int> rcpNo = new GenericParam<int>(Cfg_LT, "Validation", sOrder, "RecipeNo");
                GenericParam<int> rptCnt = new GenericParam<int>(Cfg_LT, "Validation", sOrder, "RepeatCounts");
                ValidationList.Add(i, (enabled, rcpNo, rptCnt));                
            }

            GlbVar.OnlineMode = GenPrm_BootupOnlineMode.Value;

            return bLoadSuccess;
        }
    }
}
