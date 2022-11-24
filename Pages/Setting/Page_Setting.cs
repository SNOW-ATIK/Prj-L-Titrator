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
    public partial class Page_Setting : UserControl, IPage
    {
        private Page_Config PageConfig = new Page_Config();
        private Page_Option PageOption = new Page_Option();
        private Page_Recipe PageRecipe = new Page_Recipe();
        private Page_Alarm PageAlarm = new Page_Alarm();
        private Dictionary<string, IPage> Dic_SettingSubPages = new Dictionary<string, IPage>();
        private string ShownSubPage = string.Empty;

        public Page_Setting()
        {
            InitializeComponent();

            Dic_SettingSubPages.Add("CONFIG", PageConfig);
            Dic_SettingSubPages.Add("OPTION", PageOption);
            Dic_SettingSubPages.Add("RECIPE", PageRecipe);
            Dic_SettingSubPages.Add("ALARM", PageAlarm);

            Dic_SettingSubPages.Values.ToList().ForEach(subPage =>
            {
                subPage.SetMargin(new Padding(1, 1, 1, 1));
                subPage.SetDock(DockStyle.Fill);
                subPage.SetVisible(false);
                pnl_View.Controls.Add((UserControl)subPage);
            });

            PageAlarm.RequestAlarmTestModeDisable += PageAlarm_RequestAlarmTestModeDisable;
        }

        private void PageAlarm_RequestAlarmTestModeDisable()
        {
            chk_AlarmTestMode.Checked = false;
        }

        public void Init()
        {
            PageConfig.Init();
            PageOption.Init();
            PageAlarm.Init();
            PageAlarm.ShowPagingButtonsEvent += PageAlarm_ShowPagingButtonsEvent;
        }

        private void PageAlarm_ShowPagingButtonsEvent(bool show)
        {
            Show_RightMenu(show);
        }

        public void Show_BottomMenu(bool show, bool alarmTestMode)
        {
            if (show == true)
            {
                tableLayoutPanel1.RowStyles[0].Height = 100;
                tableLayoutPanel1.RowStyles[1].Height = 50;

                if (alarmTestMode == true)
                {
                    tableLayoutPanel2.ColumnStyles[0].Width = 33;
                    tableLayoutPanel2.ColumnStyles[1].Width = 33;
                    tableLayoutPanel2.ColumnStyles[2].Width = 1;
                    tableLayoutPanel2.ColumnStyles[3].Width = 33;
                }
                else
                {
                    tableLayoutPanel2.ColumnStyles[0].Width = 50;
                    tableLayoutPanel2.ColumnStyles[1].Width = 50;
                    tableLayoutPanel2.ColumnStyles[2].Width = 0;
                    tableLayoutPanel2.ColumnStyles[3].Width = 0;
                }
            }
            else
            {
                tableLayoutPanel1.RowStyles[0].Height = 100;
                tableLayoutPanel1.RowStyles[1].Height = 0;
            }
        }

        public void Show_RightMenu(bool show)
        {
            if (show == true)
            {
                tbl_View_PageUpDown.ColumnStyles[0].Width = 100;
                tbl_View_PageUpDown.ColumnStyles[1].Width = 50;
            }
            else
            {
                tbl_View_PageUpDown.ColumnStyles[0].Width = 100;
                tbl_View_PageUpDown.ColumnStyles[1].Width = 0;
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
            Dic_SettingSubPages.Keys.ToList().ForEach(key =>
            {
                if (key == subPageName)
                {
                    Dic_SettingSubPages[key].SetVisible(true);
                    ShownSubPage = key;
                }
                else
                {
                    Dic_SettingSubPages[key].SetVisible(false);
                } 
            });
        }

        public void PagingNext()
        {
        }

        public void PagingPrev()
        {
        }

        private void btn_PagingNext_Click(object sender, EventArgs e)
        {
            Dic_SettingSubPages[ShownSubPage].PagingNext();
        }

        private void btn_PagingPrev_Click(object sender, EventArgs e)
        {
            Dic_SettingSubPages[ShownSubPage].PagingPrev();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            IParamSetting iPrmSetting = (IParamSetting)Dic_SettingSubPages[ShownSubPage];
            if (iPrmSetting.IsParamChanged() == true)
            {
                // Ask Save
                MsgFrm_AskYesNo msgAsk = new MsgFrm_AskYesNo("Do you want to save all changes?");
                if (msgAsk.ShowDialog() == DialogResult.Yes)
                {
                    iPrmSetting.SaveAllParams(true);
                }
                else
                {
                    // Ask Restore
                    msgAsk = new MsgFrm_AskYesNo("Do you want to restore all changes?");
                    if (msgAsk.ShowDialog() == DialogResult.Yes)
                    {
                        iPrmSetting.Restore();
                    }
                }
            }
        }

        private void btn_Restore_Click(object sender, EventArgs e)
        {
            IParamSetting iPrmSetting = (IParamSetting)Dic_SettingSubPages[ShownSubPage];
            // Ask Restore
            MsgFrm_AskYesNo msgAsk = new MsgFrm_AskYesNo("Do you want to restore all changes?");
            if (msgAsk.ShowDialog() == DialogResult.Yes)
            {
                iPrmSetting.Restore();
            }
        }

        private void chk_AlarmTestMode_CheckedChanged(object sender, EventArgs e)
        {
            LT_Alarm.EnableTestMode(chk_AlarmTestMode.Checked);
            PageAlarm.EnableTestMode(chk_AlarmTestMode.Checked);
        }
    }
}
