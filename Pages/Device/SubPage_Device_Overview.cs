using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator.Pages
{
    public partial class SubPage_Device_Overview : UserControl, IPage
    {

        public SubPage_Device_Overview()
        {
            InitializeComponent();
        }

        public void Set_Page()
        {

        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
            UsrCtrl_Fluidics.EnableFluidicsUpdate(visible);
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void PagingNext()
        {
            throw new NotImplementedException();
        }

        public void PagingPrev()
        {
            throw new NotImplementedException();
        }

        public void ShowSubPage(string subPageName)
        {
            throw new NotImplementedException();
        }
    }
}
