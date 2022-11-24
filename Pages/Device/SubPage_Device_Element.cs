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
using ATIK.Device.ATIK_MainBoard;
using L_Titrator.Controls;

namespace L_Titrator.Pages
{
    public partial class SubPage_Device_Element : UserControl, IPage, IAuthority
    {
        private Dictionary<DrvMB_L_Titrator.LineOrder, Panel> Dic_BitPanels = new Dictionary<DrvMB_L_Titrator.LineOrder, Panel>();
        private Dictionary<DrvMB_L_Titrator.LineOrder, Panel> Dic_AnalogPanels = new Dictionary<DrvMB_L_Titrator.LineOrder, Panel>();
        private Dictionary<DrvMB_L_Titrator.LineOrder, Panel> Dic_SyringePanels = new Dictionary<DrvMB_L_Titrator.LineOrder, Panel>();

        public SubPage_Device_Element()
        {
            InitializeComponent();

            Init_PanelDictionary();

            chk_Syringe_1.Checked = true;
        }

        private void Init_PanelDictionary()
        {
            Dic_BitPanels.Add(DrvMB_L_Titrator.LineOrder.Relay_Input, pnl_Relay_Input);
            Dic_BitPanels.Add(DrvMB_L_Titrator.LineOrder.Relay_Output, pnl_Relay_Output);
            Dic_BitPanels.Add(DrvMB_L_Titrator.LineOrder.Parallel_Input, pnl_Parallel_Input);
            Dic_BitPanels.Add(DrvMB_L_Titrator.LineOrder.Parallel_Output, pnl_Parallel_Output);
            Dic_BitPanels.Add(DrvMB_L_Titrator.LineOrder.Alarm_Input, pnl_Alarm);
            Dic_BitPanels.Add(DrvMB_L_Titrator.LineOrder.Solenoid_Output, pnl_Solenoid);

            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch0, pnl_Analog_Input_Ch0);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch1, pnl_Analog_Input_Ch1);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch2, pnl_Analog_Input_Ch2);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch3, pnl_Analog_Input_Ch3);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Output_Ch0, pnl_Analog_Output_Ch0);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Output_Ch1, pnl_Analog_Output_Ch1);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Output_Ch2, pnl_Analog_Output_Ch2);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Analog_Output_Ch3, pnl_Analog_Output_Ch3);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Temperature_RTD, pnl_Temperature_RTD);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Temperature_TC, pnl_Temperature_TC);

            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Mixer_Duty, pnl_Mixer_Duty);
            Dic_AnalogPanels.Add(DrvMB_L_Titrator.LineOrder.Mixer_RPM, pnl_Mixer_RPM);

            Dic_SyringePanels.Add(DrvMB_L_Titrator.LineOrder.Syringe_1, pnl_Syringe_1);
            Dic_SyringePanels.Add(DrvMB_L_Titrator.LineOrder.Syringe_2, pnl_Syringe_2);
        }

        public void Set_Page()
        {
            if (ATIK_MainBoard.IsInitialized(DefinedMainBoards.L_Titrator) == false)
            {
                this.Enabled = false;
            }
            Init_BitIO_Panels();
            Init_AnalogIO_Panels();
            Init_Syringe_Panels();
        }

        private void Init_BitIO_Panels()
        {
            Dic_BitPanels.Keys.ToList().ForEach(lineContent =>
            {
                int cmp_W = pnl_Relay_Input.Width - 2;
                int cmp_H = 38;
                int loc_Left = 1;
                int loc_Top = 1;
                int gap = 3;

                IMB_Driver mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
                DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;

                var lst = drvLT.Get_Bits((int)lineContent);
                if (lst == null || lst.Count == 0)
                {
                    return;
                }

                int totalHeight = cmp_H * lst.Count + 1 + lst.Count;
                if (totalHeight > Dic_BitPanels[lineContent].Height)
                {
                    cmp_W = pnl_Relay_Input.Width - 20;
                }

                for (int bitIdx = 0; bitIdx < lst.Count; bitIdx++)
                {
                    UsrCtrl_Bit cmp = new UsrCtrl_Bit(lst[bitIdx]);
                    cmp.Size = new Size(cmp_W, cmp_H);
                    if (bitIdx == 0)
                    {
                        loc_Top = 1;
                    }
                    else
                    {
                        loc_Top = cmp_H * bitIdx + gap * bitIdx;
                    }
                    cmp.Set_SplitterDistance(cmp_H);
                    cmp.Location = new Point(loc_Left, loc_Top);
                    Dic_BitPanels[lineContent].Controls.Add(cmp);
                }
            });
        }

        private void Init_AnalogIO_Panels()
        {
            Dic_AnalogPanels.Keys.ToList().ForEach(lineContent =>
            {
                int cmp_W = Dic_AnalogPanels[lineContent].Width - 2;
                int cmp_H = Dic_AnalogPanels[lineContent].Height - 2;

                IMB_Driver mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
                DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;

                MB_Elem_Analog elem = drvLT.Get_Analog((int)lineContent);
                UsrCtrl_Analog cmp = new UsrCtrl_Analog(elem);
                cmp.Size = new Size(cmp_W, cmp_H);
                cmp.Location = new Point(1, 1);
                Dic_AnalogPanels[lineContent].Controls.Add(cmp);
            });
        }

        private void Init_Syringe_Panels()
        {
            Dic_SyringePanels.Keys.ToList().ForEach(lineContent =>
            {
                int cmp_W = Dic_SyringePanels[lineContent].Width - 2;
                int cmp_H = Dic_SyringePanels[lineContent].Height - 2;

                IMB_Driver mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
                DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;

                MB_Elem_Syringe elem = drvLT.Get_Syringe((int)lineContent);
                MB_Cmp_Syringe cmp = new MB_Cmp_Syringe(elem);
                cmp.Size = new Size(cmp_W, cmp_H);
                cmp.Location = new Point(1, 1);
                cmp.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                Dic_SyringePanels[lineContent].Controls.Add(cmp);
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

        private void tmr_UpdateState_Tick(object sender, EventArgs e)
        {
            Dic_BitPanels.Values.ToList().ForEach(pnl =>
            {
                pnl.Controls.OfType<UsrCtrl_Bit>().ToList().ForEach(ctrl =>
                {
                    ctrl.UpdateState();
                });
            });

            Dic_AnalogPanels.Values.ToList().ForEach(pnl =>
            {
                pnl.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl =>
                {
                    ctrl.UpdateState();
                });
            });

            Dic_SyringePanels.Values.ToList().ForEach(pnl =>
            {
                pnl.Controls.OfType<MB_Cmp_Syringe>().ToList().ForEach(ctrl =>
                {
                    ctrl.UpdateState();
                });
            });

            if (chk_AnalogBypass.Checked == true)
            {
                IMB_Driver mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
                DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;

                drvLT.Set_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Output_Ch0, drvLT.Get_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Input_Ch0));
                drvLT.Set_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Output_Ch1, drvLT.Get_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Input_Ch1));
                drvLT.Set_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Output_Ch2, drvLT.Get_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Input_Ch2));
                drvLT.Set_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Output_Ch3, drvLT.Get_AnalogValueRaw((int)DrvMB_L_Titrator.LineOrder.Analog_Input_Ch3));
            }
        }

        private void SelectedSyringeChanged(object sender, EventArgs e)
        {
            CheckBox clicked = (CheckBox)sender;

            if (clicked.Checked == true)
            {
                if (clicked == chk_Syringe_1)
                {
                    pnl_Syringe_2.Visible = false;

                    tbl_Syringes.ColumnStyles[0].Width = 100;
                    tbl_Syringes.ColumnStyles[1].Width = 0;

                    pnl_Syringe_1.Visible = true;
                    chk_Syringe_2.Checked = false;
                }
                else
                {
                    pnl_Syringe_1.Visible = false;

                    tbl_Syringes.ColumnStyles[0].Width = 0;
                    tbl_Syringes.ColumnStyles[1].Width = 100;

                    pnl_Syringe_2.Visible = true;
                    chk_Syringe_1.Checked = false;
                }
            }
            else
            {
                if (clicked == chk_Syringe_1)
                {
                    chk_Syringe_2.Checked = true;
                }
                else
                {
                    chk_Syringe_1.Checked = true;
                }
            }
        }

        private void chk_AnalogBypass_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_AnalogBypass.Checked == true)
            {
                chk_AnalogBypass.Text = "▼ Bypass ▼";
            }
            else
            {
                chk_AnalogBypass.Text = "User Input";
            }
            pnl_Analog_Output_Ch0.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl => ctrl.EnableUserInput(chk_AnalogBypass.Checked == false));
            pnl_Analog_Output_Ch1.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl => ctrl.EnableUserInput(chk_AnalogBypass.Checked == false));
            pnl_Analog_Output_Ch2.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl => ctrl.EnableUserInput(chk_AnalogBypass.Checked == false));
            pnl_Analog_Output_Ch3.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl => ctrl.EnableUserInput(chk_AnalogBypass.Checked == false));
        }

        public void UserAuthorityIsChanged()
        {
            this.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
        }

        private void SubPage_Device_Element_VisibleChanged(object sender, EventArgs e)
        {
            if (ATIK_MainBoard.IsInitialized(DefinedMainBoards.L_Titrator) == true)
            {
                tmr_UpdateState.Enabled = this.Visible;
                if (this.Visible == true)
                {
                    if (GlbVar.CurrentMainState == MainState.Run)
                    {
                        this.Enabled = false;
                    }
                    else
                    {
                        UserAuthorityIsChanged();
                    }
                }
            }
            else
            {
                tmr_UpdateState.Enabled = false;
            }
        }
    }
}
