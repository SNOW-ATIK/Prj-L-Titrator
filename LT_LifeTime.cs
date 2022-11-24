using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ATIK;

namespace L_Titrator
{
    public class LT_LifeTime
    {
        private static List<LifeTimeObj> LifeTimeObjs;

        public static bool Load()
        {
            if (PartsLifeTimeManager.Load("Config", "LifeTime.xml", "LifeTime") == true)
            {
                LifeTimeObjs = PartsLifeTimeManager.GetAllMaintParts();
                if (LifeTimeObjs != null && LifeTimeObjs.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<LifeTimeObj> GetAllParts()
        {
            return LifeTimeObjs;
        }

        public static bool IsExpiredPartExist()
        {
            if (LifeTimeObjs == null || LifeTimeObjs.Count == 0)
            {
                return false;
            }

            bool expiredExist = LifeTimeObjs.Count(obj => obj.IsExpired == true) > 0;
            return expiredExist;
        }

        public static bool GetPart(string linkedPartLogicalName, out LifeTimeObj lifeTimeObj)
        {
            lifeTimeObj = null;
            List<LifeTimeObj> objs = null;
            switch (linkedPartLogicalName)
            {
                case PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_6Way:
                case PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_Vessel:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.DualPort_3Way_DIW).ToList();
                    break;

                case PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_Sample_6Way:
                case PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_Sample_Vessel:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.DualPort_3Way_Sample).ToList();
                    break;

                case PreDef.ElemLogicalName.SolenoidOutput.Ceric_To_3Way:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.SiglePort_3Way_VLD).ToList();
                    break;

                case PreDef.ElemLogicalName.SolenoidOutput.DrainPair_Open:
                case PreDef.ElemLogicalName.SolenoidOutput.DrainPair_Close:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.ValveDrain).ToList();
                    break;

                case PreDef.ElemLogicalName.SolenoidOutput.Valve6Way_Sample_To_Loop:
                case PreDef.ElemLogicalName.SolenoidOutput.Valve6Way_Sample_To_Vessel:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.Valve6Way).ToList();
                    break;

                case PreDef.ElemLogicalName.Syringe.Syringe_1:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.Syringe1).ToList();
                    break;

                case PreDef.ElemLogicalName.Syringe.Syringe_2:
                    objs = GetAllParts().Where(part => part.Gen_Name.Value == PreDef.LifeTimeParts.Syringe2).ToList();
                    break;
            }

            if (objs != null && objs.Count == 1)
            {
                lifeTimeObj = objs[0];
                return true;
            }
            return false;
        }

        public static void IncreaseCount(string logicalName)
        {
            if (GetPart(logicalName, out var lifeTimeObj) == true)
            {
                lifeTimeObj.IncreaseCount();
            }
        }
    }
}
