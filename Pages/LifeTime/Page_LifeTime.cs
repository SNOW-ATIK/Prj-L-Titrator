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
    public partial class Page_LifeTime : UserControl, IPage, IAuthority
    {
        private Dictionary<int, Panel> SubPages = new Dictionary<int, Panel>();
        private int SelectedSubPage = -1; 
        private List<LifeTimeObj> LifeTimeList;

        public Page_LifeTime()
        {
            InitializeComponent();
        }

        public void Init()
        {
            pnl_View.Controls.Clear();

            Show_BottomMenu(false);

            LifeTimeList = LT_LifeTime.GetAllParts();

            int height = 57;
            int height_Margin = 5;
            //if (tableLayoutPanel4.RowStyles[1].Height != 0)
            //{
            //    height_Margin = 4;
            //}
            int eachHeight = height + height_Margin;
            int ctrlsPerPage = pnl_View.Height / eachHeight;
            int pages = LifeTimeList.Count / ctrlsPerPage;
            if (pages > 1)
            {
                Show_RightMenu(true);
            }
            else
            {
                Show_RightMenu(false);
            }

            int width_Margin = 5;
            int width = pnl_View.Width - (2 * width_Margin);

            if (LifeTimeList.Count % ctrlsPerPage > 0)
            {
                ++pages;
            }

            for (int pageIdx = 0; pageIdx < pages; pageIdx++)
            {
                Panel pnl = new Panel();
                pnl.Width = pnl_View.Width;
                pnl.Height = pnl_View.Height;
                pnl.Visible = false;

                int idx_Start = ctrlsPerPage * pageIdx;
                int putInCnt = 0;
                for (int ctrlIdx = idx_Start; ctrlIdx < idx_Start + ctrlsPerPage; ctrlIdx++)
                {
                    if (ctrlIdx == LifeTimeList.Count)
                    {
                        break;
                    }
                    UsrCtrl_LifeTime ctrl = new UsrCtrl_LifeTime(LifeTimeList[ctrlIdx], true, false, false);
                    ctrl.Size = new Size(width, height);
                    ctrl.Location = new Point(width_Margin, height_Margin + (height + height_Margin) * putInCnt);
                    pnl.Controls.Add(ctrl);
                    pnl.VisibleChanged += Pnl_VisibleChanged;

                    ++putInCnt;
                }

                SubPages.Add(pageIdx, pnl);
                pnl_View.Controls.Add(pnl);
            }

            if (SubPages.Count > 0)
            {
                SubPages[0].Visible = true;
                SelectedSubPage = 0;
            }
        }

        private void Pnl_VisibleChanged(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            if (pnl.Visible == true)
            {
                UserAuthorityIsChanged();

                List<UsrCtrl_LifeTime> cmps = pnl.Controls.OfType<UsrCtrl_LifeTime>().ToList();
                cmps.ForEach(cmp =>
                {
                    cmp.UpdateStatus();
                    cmp.ChangeLanguage(GlbVar.CurrentLanguage);
                    cmp.CheckAuthority(GlbVar.CurrentAuthority, GlbVar.CurrentMainState != MainState.Run);
                });
            }
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

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
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

        public void UserAuthorityIsChanged()
        {
        }

        private void Page_LifeTime_VisibleChanged(object sender, EventArgs e)
        {
        }
    }
}
