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
    public partial class Page_Config : UserControl, IPage, IParamSetting, IAuthority
    {
        public Page_Config()
        {
            InitializeComponent();
        }

        public void Init()
        {
            CmpVal_AutoLogOutTime.Init(LT_Config.GenPrm_LogOutCheckTime, "Log-Out Check Time [sec]");
            CmpCol_InterlockEnabled.Init(LT_Config.GenPrm_InterlockEnabled, "Interlock Enabled");

            CmpCol_VLD_Enabled.Init(LT_Config.GenPrm_Validation_Enabled, "Enabled");
            CmpCol_VLD_NotifyResult.Init(LT_Config.GenPrm_Validation_NotifyResult, "Notify Result");

            CmpCol_VLD_1st_Enabled.Init(LT_Config.ValidationList[0].Enabled, "Enabled");
            CmpVal_VLD_1st_RepeatCounts.Init(LT_Config.ValidationList[0].RepeatCounts, "Repeat Counts");
            CmpVal_VLD_1st_RecipeInfo.Init(LT_Config.ValidationList[0].RecipeNo, "Recipe No");
            if (LT_Recipe.Get_RecipeObj(LT_Config.ValidationList[0].RecipeNo.Value, out var vld1st) == true)
            {
                lbl_VLD_1st_RecipeName.Text = vld1st.Name;
            }
            else
            {
                lbl_VLD_1st_RecipeName.Text = $"Invalid";
            }

            CmpCol_VLD_2nd_Enabled.Init(LT_Config.ValidationList[1].Enabled, "Enabled");
            CmpVal_VLD_2nd_RepeatCounts.Init(LT_Config.ValidationList[1].RepeatCounts, "Repeat Counts");
            CmpVal_VLD_2nd_RecipeInfo.Init(LT_Config.ValidationList[1].RecipeNo, "Recipe No");
            if (LT_Recipe.Get_RecipeObj(LT_Config.ValidationList[1].RecipeNo.Value, out var vld2nd) == true)
            {
                lbl_VLD_2nd_RecipeName.Text = vld2nd.Name;
            }
            else
            {
                lbl_VLD_2nd_RecipeName.Text = $"Invalid";
            }

            CmpCol_VLD_3rd_Enabled.Init(LT_Config.ValidationList[2].Enabled, "Enabled");
            CmpVal_VLD_3rd_RepeatCounts.Init(LT_Config.ValidationList[2].RepeatCounts, "Repeat Counts");
            CmpVal_VLD_3rd_RecipeInfo.Init(LT_Config.ValidationList[2].RecipeNo, "Recipe No");
            if (LT_Recipe.Get_RecipeObj(LT_Config.ValidationList[2].RecipeNo.Value, out var vld3rd) == true)
            {
                lbl_VLD_3rd_RecipeName.Text = vld3rd.Name;
            }
            else
            {
                lbl_VLD_3rd_RecipeName.Text = $"Invalid";
            }

        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
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

        public bool IsParamChanged()
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

        public void UserAuthorityIsChanged()
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.EnableModifying(true, GlbVar.CurrentAuthority == UserAuthority.Admin));
        }

        private void Page_Config_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                UserAuthorityIsChanged();
            }
        }

        private void CmpVal_VLD_1st_RecipeInfo_ValueClickedEvent(object sender, object oldValue)
        {
            Frm_ManualStart frm = new Frm_ManualStart();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.RecipeNo != (int)oldValue)
                {
                    CmpVal_VLD_1st_RecipeInfo.GenParam.Set_ValueObject(frm.RecipeNo, false);
                    LT_Recipe.Get_RecipeObj(frm.RecipeNo, out var rcpObj);
                    lbl_VLD_1st_RecipeName.Text = rcpObj.Name;
                }
            }
        }
    }
}
