using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace L_Titrator_Alpha.Pages
{
    public partial class Page_History : UserControl, IPage
    {
        private Page_Alarm PageAlarm = new Page_Alarm();
        private Page_Data PageData = new Page_Data();
        private Dictionary<string, IPage> Dic_SettingSubPages = new Dictionary<string, IPage>();

        public Page_History()
        {
            InitializeComponent();

            Dic_SettingSubPages.Add("ALARM", PageAlarm);
            Dic_SettingSubPages.Add("DATA", PageData);

            Dic_SettingSubPages.Values.ToList().ForEach(subPage =>
            {
                subPage.SetMargin(new Padding(1, 1, 1, 1));
                subPage.SetDock(DockStyle.Fill);
                subPage.SetVisible(false);
                pnl_View.Controls.Add((UserControl)subPage);
            });
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
                }
                else
                {
                    Dic_SettingSubPages[key].SetVisible(false);
                } 
            });
        }

        public void PagingNext()
        {
            throw new NotImplementedException();
        }

        public void PagingPrev()
        {
            throw new NotImplementedException();
        }
    }
}
