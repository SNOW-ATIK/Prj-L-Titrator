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
    public partial class SubPage_Device_Communication : UserControl, IPage
    {
        private bool IsDataGridSet = false;

        public SubPage_Device_Communication()
        {
            InitializeComponent();
        }

        public void Set_Page()
        {
            if (ATIK_MainBoard.IsInitialized(DefinedMainBoards.L_Titrator) == false)
            {
                this.Enabled = false;
                return;
            }
            var mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
            MB_Protocol BoardProtocol = mbDrv.Get_Protocol();

            for (int i = 0; i < BoardProtocol.TotalLines; i++)
            {
                dgv_InternalLog.Rows.Add(i, BoardProtocol.Get_LineName(i));
            }
            IsDataGridSet = true;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;            
            tmr_UpdateFrame.Enabled = visible;
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

        private void tmr_UpdateFrame_Tick(object sender, EventArgs e)
        {
            if (IsDataGridSet == false)
            {
                return;
            }

            var mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
            if (mbDrv.IsInitialized == false)
            {
                return;
            }

            DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;
            var TxFrame = drvLT.Get_TxFrame();
            var RxFrame = drvLT.Get_RxFrame();

            for (int i = 0; i < dgv_InternalLog.Rows.Count; i++)
            {
                if (TxFrame.Count == dgv_InternalLog.Rows.Count)
                {
                    dgv_InternalLog.Rows[i].Cells[2].Value = TxFrame[i];
                }
                if (RxFrame.Count == dgv_InternalLog.Rows.Count)
                {
                    dgv_InternalLog.Rows[i].Cells[3].Value = RxFrame[i];
                }
            }
        }
    }
}
