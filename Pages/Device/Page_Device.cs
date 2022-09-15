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
    public partial class Page_Device : UserControl, IPage
    {
        public enum SubPage
        { 
            COMMUNICATION,
            IO,
            OVERVIEW,
        }

        private SubPage ClickedPage = SubPage.COMMUNICATION;
        private Dictionary<SubPage, IPage> DicSubPages = new Dictionary<SubPage, IPage>();
        private Dictionary<SubPage, Button> DicSubPageChangeBtn = new Dictionary<SubPage, Button>();

        private SubPage_Device_Communication SubPage_Communication = new SubPage_Device_Communication();
        private SubPage_Device_Element SubPage_Element = new SubPage_Device_Element();

        public Page_Device()
        {
            InitializeComponent();
            Init_SubPages();
        }

        private void Init_SubPages()
        {
            SubPage_Communication = new SubPage_Device_Communication();
            SubPage_Communication.SetMargin(new Padding(0, 0, 0, 0));
            SubPage_Communication.SetDock(DockStyle.Fill);

            SubPage_Element = new SubPage_Device_Element();
            SubPage_Element.SetMargin(new Padding(0, 0, 0, 0));
            SubPage_Element.SetDock(DockStyle.Fill);

            DicSubPages = new Dictionary<SubPage, IPage>();
            DicSubPages.Add(SubPage.COMMUNICATION, SubPage_Communication);
            DicSubPages.Add(SubPage.IO, SubPage_Element);

            pnl_View.Controls.Add(SubPage_Communication);
            pnl_View.Controls.Add(SubPage_Element);

            DicSubPageChangeBtn.Add(SubPage.COMMUNICATION, btn_Communication);
            DicSubPageChangeBtn.Add(SubPage.IO, btn_IO);
            DicSubPageChangeBtn.Add(SubPage.OVERVIEW, btn_Overview);

            btn_Communication.PerformClick();
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
            ClickedPage = (SubPage)Enum.Parse(typeof(SubPage), subPageName);
            DicSubPages.Keys.ToList().ForEach(subPage =>
            {
                DicSubPages[subPage].SetVisible(subPage == ClickedPage);
                DicSubPageChangeBtn[subPage].BackColor = subPage == ClickedPage ? PreDef.MenuBG_Select : PreDef.MenuBG_Unselect;
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

        private void Request_ChangeSubPage(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ShowSubPage((string)btn.Tag);
        }

        public void Set_SubPages()
        {
            SubPage_Communication.Set_Page();
            SubPage_Element.Set_Page();
        }
    }
}
