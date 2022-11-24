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

namespace L_Titrator
{
    public partial class UsrCtrl_AlarmInfo : UserControl, IParamSetting
    {
        public const int FixedHeight = 55;
        public bool IsTestAlarmSet { get => chk_Set.Checked; }

        private AlarmObject MyAlarm;

        public UsrCtrl_AlarmInfo()
        {
            InitializeComponent();
        }

        public UsrCtrl_AlarmInfo(AlarmObject alarmInfo)
        {
            MyAlarm = alarmInfo;

            InitializeComponent();

            lbl_Name.Text = alarmInfo.Name.ToString();
            //CmpCol_Level.Init(alarmInfo.Gen_Level, "Level", Enum.GetValues(typeof(AlarmLevel)));
            CmpCol_Level.Init_WithOutGenPrm("Level", Enum.GetValues(typeof(AlarmLevel)), alarmInfo.Gen_Level.Value);
            CmpVal_Description.Init_WithOutGenPrm("Description", alarmInfo.Description);
            CmpCol_Code.Init_WithOutGenPrm("Code", LT_Alarm.AlarmCodes.ToArray(), alarmInfo.Gen_Code.Value);
            CmpCol_OutputRelay.Init_WithOutGenPrm("Output Relay", new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, alarmInfo.Gen_OutRelayChannelNo.Value);
        }

        public AlarmObject GetAlarm()
        {
            return MyAlarm;
        }


        public void ChangeLanguage()
        {
            switch (GlbVar.CurrentLanguage)
            {
                case Language.ENG:
                    break;

                case Language.KOR:
                    break;
            }
            // TBD
            //string sLang = SigmaLanguage.CurrentLanguage.ToString();
            //CmpVal_Description.ChangeLanguage_Title(sLang, SigmaLanguage.View_Main.Page_Setting.SubPage_Alarm.Cmp_Alarm.Item_Desc);
            //CmpCol_Level.ChangeLanguage_Title(sLang, SigmaLanguage.View_Main.Page_Setting.SubPage_Alarm.Cmp_Alarm.Item_Level);
            //CmpVal_Category.ChangeLanguage_Title(sLang, SigmaLanguage.View_Main.Page_Setting.SubPage_Alarm.Cmp_Alarm.Item_Category);
            //CmpVal_Code.ChangeLanguage_Title(sLang, SigmaLanguage.View_Main.Page_Setting.SubPage_Alarm.Cmp_Alarm.Item_Code);
            //CmpVal_InputRelay.ChangeLanguage_Title(sLang, SigmaLanguage.View_Main.Page_Setting.SubPage_Alarm.Cmp_Alarm.Item_InputRelay);
            //CmpCol_OutputRelay.ChangeLanguage_Title(sLang, SigmaLanguage.View_Main.Page_Setting.SubPage_Alarm.Cmp_Alarm.Item_InputRelay);
        }

        public void EnabledAlarmTestMode(bool enb)
        {
            tableLayoutPanel1.ColumnStyles[0].Width = enb == true ? 55 : 0;
        }

        public void EnableParameter(bool enb)
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.EnableModifying(true, GlbVar.CurrentAuthority == UserAuthority.Admin));
        }

        public void EnableModifying(bool enb)
        {
            tableLayoutPanel1.Controls.OfType<PrmCmp_Collection>().ToList().ForEach(cmp => cmp.EnableModifying(true, enb));
        }

        public List<IPrmCmp> GetAllIComp()
        {
            return tableLayoutPanel1.Controls.OfType<IPrmCmp>().ToList();
        }

        private void CmpCol_Level_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            if (MyAlarm.Gen_Level.Value == (AlarmLevel)changedValue)
            {
                CmpCol_Level.Color_Name = Color.LemonChiffon;
            }
            else
            {
                CmpCol_Level.Color_Name = Color.DarkOrange;
            }
        }

        private void CmpCol_Code_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            if (MyAlarm.Gen_Code.Value == (string)changedValue)
            {
                CmpCol_Code.Color_Name = Color.LemonChiffon;
            }
            else
            {
                CmpCol_Code.Color_Name = Color.DarkOrange;
            }
        }

        private void CmpCol_OutputRelay_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            if (MyAlarm.Gen_OutRelayChannelNo.Value == (int)changedValue)
            {
                CmpCol_OutputRelay.Color_Name = Color.LemonChiffon;
            }
            else
            {
                CmpCol_OutputRelay.Color_Name = Color.DarkOrange;
            }
        }
        
        public void Restore()
        {
            CmpCol_Level.Prm_Value = MyAlarm.Gen_Level.Value;
            CmpCol_Code.Prm_Value = MyAlarm.Gen_Code.Value;
            CmpCol_OutputRelay.Prm_Value = MyAlarm.Gen_OutRelayChannelNo.Value;
        }

        public void UpdateChangedStatus()
        {
            CmpCol_Level.Color_Name = Color.LemonChiffon;
            CmpCol_Code.Color_Name = Color.LemonChiffon;
            CmpCol_OutputRelay.Color_Name = Color.LemonChiffon;
        }

        public void SaveAllParams(bool askSave)
        {
            MyAlarm.Gen_Level.Set_Value((AlarmLevel)CmpCol_Level.Prm_Value);
            MyAlarm.Gen_Code.Set_Value((string)CmpCol_Code.Prm_Value);
            MyAlarm.Gen_OutRelayChannelNo.Set_Value((int)CmpCol_OutputRelay.Prm_Value);

            UpdateChangedStatus();
        }

        public bool IsParamChanged()
        {
            if ((AlarmLevel)CmpCol_Level.Prm_Value != MyAlarm.Gen_Level.Value)
            {
                return true;
            }
            if ((string)CmpCol_Code.Prm_Value != MyAlarm.Gen_Code.Value)
            {
                return true;
            }
            if ((int)CmpCol_OutputRelay.Prm_Value != MyAlarm.Gen_OutRelayChannelNo.Value)
            {
                return true;
            }
            return false;
        }


        private void chk_Set_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Set.Checked == true)
            {
                LT_Alarm.Set_Alarm(MyAlarm.Name, true);
            }
            else
            {
                LT_Alarm.Reset_Alarm(MyAlarm.Name, true);
            }
        }
    }
}
