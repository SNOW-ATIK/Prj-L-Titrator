﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK;
using ATIK.Device;
using ATIK.Device.ATIK_MainBoard;
using ATIK.Common.ComponentEtc.Fluidics;

namespace L_Titrator.Controls
{
    public partial class UsrCtrl_Fluidics_Large : UserControl
    {
        System.Windows.Forms.Timer tmr_CheckFluidics;
        public UsrCtrl_Fluidics_Large()
        {
            InitializeComponent();

            Cmp3Way_DIW.Init("3Way-DIW");
            Cmp3Way_Sample.Init("3Way-Sample");
            Cmp3Way_Validation.Init("3Way-Validation");

            Cmp2Way_Drain.Init("2Way-Drain");

            Cmp6Way.Init("6Way-Capture");

            tmr_CheckFluidics = new Timer();
            tmr_CheckFluidics.Interval = 1000;
            tmr_CheckFluidics.Tick += Tmr_CheckFluidics_Tick;
        }

        public void EnableFluidicsUpdate(bool enb)
        {
            if (ATIK_MainBoard.IsInitialized(DefinedMainBoards.L_Titrator) == false)
            {
                tmr_CheckFluidics.Enabled = false;
                return;
            }
            tmr_CheckFluidics.Enabled = enb;
        }

        public void EnableFluidicsControl(bool enb)
        {
            this.Enabled = enb;
        }

        private void Tmr_CheckFluidics_Tick(object sender, EventArgs e)
        {
            //CmpTest();
            CheckFluidics();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void CheckFluidics()
        {
            // 3Way-DIW
            bool valve3Way_DIW_Right = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_6Way).Get_State();
            bool valve3Way_DIW_Bottom = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_Vessel).Get_State();
            if (valve3Way_DIW_Right == valve3Way_DIW_Bottom)
            {
                Cmp3Way_DIW.Draw_State(Valve_PortDirection.Right, false, Valve_PortDirection.Bottom, false);
            }
            else
            {
                Cmp3Way_DIW.Draw_State(Valve_PortDirection.Right, valve3Way_DIW_Right, Valve_PortDirection.Bottom, valve3Way_DIW_Bottom);
            }

            // 3Way-Sample
            bool valve3Way_Sample_Left = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_Sample_6Way).Get_State();
            Cmp3Way_Sample.Draw_State(Valve_PortDirection.Left, valve3Way_Sample_Left, Valve_PortDirection.Top, !valve3Way_Sample_Left);

            // 3Way-Reagent
            bool valve3Way_Reagent_Bottom = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Ceric_To_3Way).Get_State();
            Cmp3Way_Validation.Draw_State(Valve_PortDirection.Top, !valve3Way_Reagent_Bottom, Valve_PortDirection.Bottom, valve3Way_Reagent_Bottom);

            // 6Way
            bool valve6Way_Sample_To_Vessel = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Valve6Way_Sample_To_Vessel).Get_State();
            bool valve6Way_Sample_To_Loop = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Valve6Way_Sample_To_Loop).Get_State();
            if (valve6Way_Sample_To_Vessel == valve6Way_Sample_To_Loop)
            {
                Cmp6Way.Draw_State(Valve_6Way_State.Link_None);
            }
            else
            {
                if (valve6Way_Sample_To_Vessel == true)
                {
                    Cmp6Way.Draw_State(Valve_6Way_State.Link_12);
                }
                else
                {
                    Cmp6Way.Draw_State(Valve_6Way_State.Link_23);
                }
            }

            // 2Way
            bool valve2Way_Drain_Open = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DrainPair_Open).Get_State();
            bool valve2Way_Drain_Close = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DrainPair_Close).Get_State();
            if (valve2Way_Drain_Open == true && valve2Way_Drain_Close == false)
            {
                Cmp2Way_Drain.Draw_State(Valve_PortDirection.Top, true, Valve_PortDirection.Bottom, true);
            }
            if (valve2Way_Drain_Open == false && valve2Way_Drain_Close == true)
            {
                Cmp2Way_Drain.Draw_State(Valve_PortDirection.Top, false, Valve_PortDirection.Bottom, false);
            }

            // Syringe1
            var syringe1 = MB_Elem_Syringe.GetElem("Syringe_1");
            if (syringe1 != null)
            {
                var vol_rtn = syringe1.Get_Volume_mL();
                if (vol_rtn.IsValid == true)
                {
                    int remains = (int)(vol_rtn.Volume_mL / syringe1.MaxVolume_mL * 100);
                    remains = remains / 10 * 10;
                    CmpSyringe1.Remains = remains;
                }

                var dir_rtn = syringe1.Get_PortDirection();
                // TBD: 매끄럽지 않음.
                // Cmp 프로젝트에서 Device를 참조하지 않아서, Device로부터 리턴받은 PortDirection과 Cmp내에서 표시할 Valve 방향을 맞춰줘야함.
                // 이것도 Syring1과 Syringe2가 다름
                switch (dir_rtn)
                {
                    case MB_SyringeDirection.In:
                        CmpSyringe1_Head.Valve_Open = Valve_PortDirection.Left;
                        break;

                    case MB_SyringeDirection.Out:
                        CmpSyringe1_Head.Valve_Open = Valve_PortDirection.Right;
                        break;

                    case MB_SyringeDirection.Ext:
                        CmpSyringe1_Head.Valve_Open = Valve_PortDirection.Top;
                        break;

                    case MB_SyringeDirection.None:
                        break;
                }                
            }

            // Syringe2
            var syringe2 = MB_Elem_Syringe.GetElem("Syringe_2");
            if (syringe2 != null)
            {
                var vol_rtn = syringe2.Get_Volume_mL();
                if (vol_rtn.IsValid == true)
                {
                    int remains = (int)(vol_rtn.Volume_mL / syringe2.MaxVolume_mL * 100);
                    remains = remains / 10 * 10;
                    CmpSyringe2.Remains = remains;
                }

                var dir_rtn = syringe2.Get_PortDirection();
                // TBD: 매끄럽지 않음.
                // Cmp 프로젝트에서 Device를 참조하지 않아서, Device로부터 리턴받은 PortDirection과 Cmp내에서 표시할 Valve 방향을 맞춰줘야함.
                // 이것도 Syring1과 Syringe2가 다름
                switch (dir_rtn)
                {
                    case MB_SyringeDirection.In:
                        CmpSyringe2_Head.Valve_Open = Valve_PortDirection.Right;
                        break;

                    case MB_SyringeDirection.Out:
                        CmpSyringe2_Head.Valve_Open = Valve_PortDirection.Left;
                        break;

                    default:
                        break;
                }
            }

            // Vessel
            bool vesselFull = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Level_1).Get_State() == false;
            CmpVessel.Remains = vesselFull == true ? 100 : 0;
        }

        int idxTest = 0;
        private void CmpTest()
        {
            //int testValve = idxTest % 4;
            //switch (testValve)
            //{
            //    case 0:
            //        Cmp3Way_DIW.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, false);
            //        Cmp3Way_Sample.Draw_State(Valve_Port.Left, false, Valve_Port.Bottom, false);
            //        Cmp3Way_Validation.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, false);
            //        Cmp2Way_Drain.Draw_State(Valve_Port.Top, false, Valve_Port.Bottom, false);
            //        Cmp6Way.Draw_State(Valve_6Way_State.Link_None);
            //        break;

            //    case 1:
            //        Cmp3Way_DIW.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, false);
            //        Cmp3Way_Sample.Draw_State(Valve_Port.Left, true, Valve_Port.Bottom, false);
            //        Cmp3Way_Validation.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, false);
            //        break;

            //    case 2:
            //        Cmp3Way_DIW.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, true);
            //        Cmp3Way_Sample.Draw_State(Valve_Port.Left, false, Valve_Port.Bottom, true);
            //        Cmp3Way_Validation.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, true);
            //        Cmp2Way_Drain.Draw_State(Valve_Port.Top, true, Valve_Port.Bottom, true);
            //        Cmp6Way.Draw_State(Valve_6Way_State.Link_12);
            //        break;

            //    case 3:
            //        Cmp3Way_DIW.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, true);
            //        Cmp3Way_Sample.Draw_State(Valve_Port.Left, true, Valve_Port.Bottom, true);
            //        Cmp3Way_Validation.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, true);
            //        Cmp6Way.Draw_State(Valve_6Way_State.Link_23);
            //        break;
            //}

            //int testSyringeHead = idxTest % 6;
            //switch (testSyringeHead)
            //{
            //    case 0:
            //        CmpSyringe1_Head.Valve_Open = Valve_Port.Left;
            //        CmpSyringe2_Head.Valve_Open = Valve_Port.Left;
            //        break;

            //    case 1:
            //        break;

            //    case 2:
            //        CmpSyringe1_Head.Valve_Open = Valve_Port.Top;
            //        break;

            //    case 3:
            //        CmpSyringe2_Head.Valve_Open = Valve_Port.Right;
            //        break;

            //    case 4:
            //        CmpSyringe1_Head.Valve_Open = Valve_Port.Right;
            //        break;

            //    case 5:
            //        break;
            //}

            //int testReagent = idxTest * 10;
            //CmpReagent1.Remains = 100 - testReagent;
            //CmpReagent2.Remains = 100 - testReagent;

            //CmpSyringe1.Remains = testReagent;
            //CmpSyringe2.Remains = testReagent;

            //int testVesset = 100 - idxTest % 6 * 20;
            //CmpVessel.Remains = testVesset;


            //if (idxTest == 10)
            //{
            //    idxTest = 0;
            //}
            //else
            //{
            //    idxTest++;
            //}
        }

        public bool IsVesselFull()
        {
            var vesselFull = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Level_1);
            var OverflowDetected = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Level_2);
            if (vesselFull.Get_State() == false || OverflowDetected.Get_State() == false)
            {
                return true;
            }
            return false;
        }

        private void Cmp3Way_DIW_Click(object sender, EventArgs e)
        {
            Cmp_Valve_3Way_Bmp cmp = (Cmp_Valve_3Way_Bmp)sender;
            Point point = this.PointToScreen(new Point(cmp.Right + 5, cmp.Top + cmp.Height / 2));
            Frm_ValveControl frm = new Frm_ValveControl(cmp, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var port_DIW_To_6Way = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_6Way);
                port_DIW_To_6Way.Set_State(frm.ApplyPortState[Valve_PortDirection.Right] == Valve_State.Open);

                var port_DIW_To_Vessel = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_DIW_Vessel);
                port_DIW_To_Vessel.Set_State(frm.ApplyPortState[Valve_PortDirection.Bottom] == Valve_State.Open);
            }
        }

        private void Cmp3Way_Sample_Click(object sender, EventArgs e)
        {
            Cmp_Valve_3Way_Bmp cmp = (Cmp_Valve_3Way_Bmp)sender;
            Point point = this.PointToScreen(new Point(cmp.Right + 5, cmp.Top + cmp.Height / 2));
            Frm_ValveControl frm = new Frm_ValveControl(cmp, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var port_Slurry_To_3Way = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DualPort_3Way_Sample_6Way);
                port_Slurry_To_3Way.Set_State(frm.ApplyPortState[Valve_PortDirection.Top] == Valve_State.Open);
            }
        }

        private void Cmp3Way_Validation_Click(object sender, EventArgs e)
        {
            Cmp_Valve_3Way_Bmp cmp = (Cmp_Valve_3Way_Bmp)sender;
            Point point = this.PointToScreen(new Point(cmp.Right + 5, cmp.Top + cmp.Height / 2));
            Frm_ValveControl frm = new Frm_ValveControl(cmp, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var port_Ceric_To_3Way = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Ceric_To_3Way);
                port_Ceric_To_3Way.Set_State(frm.ApplyPortState[Valve_PortDirection.Top] == Valve_State.Open);
            }
        }

        private void Cmp2Way_Drain_Click(object sender, EventArgs e)
        {
            Cmp_Valve_2Way_Bmp cmp = (Cmp_Valve_2Way_Bmp)sender;
            Point point = this.PointToScreen(new Point(cmp.Right + 5, cmp.Bottom - cmp.Height / 2 - 303));
            Frm_ValveControl frm = new Frm_ValveControl(cmp, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.ApplyPortState[Valve_PortDirection.Top] == frm.ApplyPortState[Valve_PortDirection.Bottom])
                {
                    MsgFrm_NotifyOnly msgFrm = new MsgFrm_NotifyOnly("Invalid Operation.");
                    msgFrm.ShowDialog();
                    return;
                }

                var port_DrainOpen = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DrainPair_Open);
                var port_DrainClose = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.DrainPair_Close);

                port_DrainOpen.Set_State(false);
                port_DrainClose.Set_State(false);

                System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                st.Start();
                while (st.ElapsedMilliseconds < 2000)
                {
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();
                }
                st.Stop();

                port_DrainOpen.Set_State(frm.ApplyPortState[Valve_PortDirection.Top] == Valve_State.Open);
                port_DrainClose.Set_State(frm.ApplyPortState[Valve_PortDirection.Bottom] == Valve_State.Open);
            }
        }

        private void Cmp6Way_Click(object sender, EventArgs e)
        {
            Cmp_Valve_6Way_Bmp cmp = (Cmp_Valve_6Way_Bmp)sender;
            Point point = this.PointToScreen(new Point(cmp.Left - 5 - 320, cmp.Top + cmp.Height / 2));
            Frm_ValveControl frm = new Frm_ValveControl(cmp, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.ApplyPortState[Valve_PortDirection.Link_12] == frm.ApplyPortState[Valve_PortDirection.Link_23])
                {
                    MsgFrm_NotifyOnly msgFrm = new MsgFrm_NotifyOnly("Invalid Operation.");
                    msgFrm.ShowDialog();
                    return;
                }

                MB_Elem_Bit port_6Way_ToVessel = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Valve6Way_Sample_To_Vessel);
                port_6Way_ToVessel.Set_State(frm.ApplyPortState[Valve_PortDirection.Link_12] == Valve_State.Open);
                MB_Elem_Bit port_6Way_ToLoop = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Valve6Way_Sample_To_Loop);
                port_6Way_ToLoop.Set_State(frm.ApplyPortState[Valve_PortDirection.Link_23] == Valve_State.Open);
            }
        }

        private void CmpSyringe1_Click(object sender, EventArgs e)
        {            
            var elemSyringe = MB_Elem_Syringe.GetElem("Syringe_1");
            Point point = this.PointToScreen(new Point(CmpSyringe1_Head.Left - 5 - 240, CmpSyringe1_Head.Top));
            Frm_SyringeControl frm = new Frm_SyringeControl(elemSyringe, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var port_Ceric_To_3Way = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.SolenoidOutput.Ceric_To_3Way);
                if (port_Ceric_To_3Way.Get_State() == false && frm.ApplySyringeState.Direction == MB_SyringeDirection.Ext)
                {
                    MsgFrm_NotifyOnly msgFrm = new MsgFrm_NotifyOnly("Can not transfer.\r\nBlocked by Validation Valve");
                    msgFrm.ShowDialog();
                    return;
                }
                elemSyringe.Run_mL(frm.ApplySyringeState);
            }
        }

        private void CmpSyringe2_Click(object sender, EventArgs e)
        {
            var elemSyringe = MB_Elem_Syringe.GetElem("Syringe_2");
            Point point = this.PointToScreen(new Point(CmpSyringe2_Head.Right + 5, CmpSyringe2_Head.Top));
            Frm_SyringeControl frm = new Frm_SyringeControl(elemSyringe, point);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                elemSyringe.Run_mL(frm.ApplySyringeState);
            }
        }
    }
}