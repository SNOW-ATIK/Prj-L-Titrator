using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace L_Titrator.Pages
{
    public partial class Page_Setting : UserControl, IPage
    {
        private Page_Config PageConfig = new Page_Config();
        private Page_Option PageOption = new Page_Option();
        private Page_Recipe PageRecipe = new Page_Recipe();
        private Dictionary<string, IPage> Dic_SettingSubPages = new Dictionary<string, IPage>();

        public Page_Setting()
        {
            InitializeComponent();

            Dic_SettingSubPages.Add("CONFIG", PageConfig);
            Dic_SettingSubPages.Add("OPTION", PageOption);
            Dic_SettingSubPages.Add("RECIPE", PageRecipe);

            Dic_SettingSubPages.Values.ToList().ForEach(subPage =>
            {
                subPage.SetMargin(new Padding(1, 1, 1, 1));
                subPage.SetDock(DockStyle.Fill);
                subPage.SetVisible(false);
                pnl_View.Controls.Add((UserControl)subPage);
            });
        }

        public void Show_BottomMenu(bool show)
        {
            if (show == true)
            {
                tableLayoutPanel1.RowStyles[0].Height = 100;
                tableLayoutPanel1.RowStyles[1].Height = 50;
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
                    //if (key == "RECIPE")
                    //{
                    //    tbl_View_PageUpDown.ColumnStyles[1].Width = 0;
                    //}
                }
                else
                {
                    Dic_SettingSubPages[key].SetVisible(false);
                    //if (key == "RECIPE")
                    //{
                    //    tbl_View_PageUpDown.ColumnStyles[1].Width = 50;
                    //}
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
            PagingNext();
        }

        private void btn_PagingPrev_Click(object sender, EventArgs e)
        {
            PagingPrev();
        }
    }
}
