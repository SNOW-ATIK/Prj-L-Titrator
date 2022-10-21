using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK;

namespace L_Titrator.Pages
{
    public partial class Page_Option : UserControl, IPage, IParamSetting
    {
        public Page_Option()
        {
            InitializeComponent();
        }

        public void Init()
        {
            CmpCol_Language.Init(LT_Config.GenPrm_Language, "Language", Enum.GetValues(typeof(Language)), LT_Config.GenPrm_Language.Value);
            CmpCol_Language.SelectedUserItemChangedEvent += CmpCol_LanguageChangedEvent;

            CmpCol_BootUpOnlineMode.Init(LT_Config.GenPrm_BootupOnlineMode, "Boot-Up Online Mode", Enum.GetValues(typeof(OnlineMode)), LT_Config.GenPrm_BootupOnlineMode.Value);
            CmpVal_AutoLogOutTime.Init(LT_Config.GenPrm_LogOutCheckTime, "Log-Out Check Time [sec]");

            CmpCol_Manual_Enabled.Init(LT_Config.GenPrm_Manual_Enabled, "Enabled");
            CmpCol_Manual_AvailableOnRemote.Init(LT_Config.GenPrm_Manual_AvailableOnRemote, "Available On Remote");
            CmpCol_Manual_NotifyResult.Init(LT_Config.GenPrm_Manual_NotifyResult, "Notify Result");
            CmpCol_Manual_SaveHistory.Init(LT_Config.GenPrm_Manual_SaveHistory, "Save History");

            CmpCol_Periodic_Enabled.Init(LT_Config.GenPrm_Periodic_Enabled, "Enabled");
            CmpCol_Periodic_AvailableOnRemote.Init(LT_Config.GenPrm_Periodic_AvailableOnRemote, "Available On Remote");
            CmpCol_Periodic_NotifyResult.Init(LT_Config.GenPrm_Periodic_NotifyResult, "Notify Result");
            CmpCol_Periodic_SaveHistory.Init(LT_Config.GenPrm_Periodic_SaveHistory, "Save History");
            CmpCol_Periodic_RecipeNo.Init(LT_Config.GenPrm_Periodic_RecipeNo, "Recipe No.", (new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).ToArray());
            CmpCol_Periodic_UseValidation.Init(LT_Config.GenPrm_Periodic_UseValidation, "Use Validation");
            CmpVal_Periodic_Period.Init(LT_Config.GenPrm_Periodic_Period, "Period [min]");
            CmpVal_Periodic_NextMeasureTime.Init(LT_Config.GenPrm_Periodic_NextMeasureTime, "Next Measure Time");
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
        }

        private void CmpCol_LanguageChangedEvent(object sender, object changedValue)
        {
            //throw new NotImplementedException();
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void ShowSubPage(string subPageName)
        {
            throw new NotImplementedException();
        }

        public void PagingNext()
        {
            throw new NotImplementedException();
        }

        public void PagingPrev()
        {
            throw new NotImplementedException();
        }

        public void Restore()
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.Restore());
        }

        public void UpdateChangedStatus()
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.UpdateNamePlate());
        }

        public void SaveAllParams(bool askSave)
        {
            ParamPageUtil.GetAll_IParams(this).ForEach(prm => prm?.Save(true));
            UpdateChangedStatus();
        }

        public bool CheckParamChanged()
        {
            int changedCounts = 0;
            ParamPageUtil.GetAll_IParams(this).ForEach(prm =>
            {
                if (prm != null && prm.ValueObject.Equals(prm.ValueObject_Original) == false)
                {
                    ++changedCounts;
                }
            });
            return (changedCounts != 0);
        }
    }
}
