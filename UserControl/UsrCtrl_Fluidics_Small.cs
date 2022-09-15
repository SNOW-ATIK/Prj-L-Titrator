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
using ATIK.Common.ComponentEtc.Fluidics;

namespace L_Titrator_Alpha.Controls
{
    public partial class UsrCtrl_Fluidics_Small : UserControl
    {
        System.Windows.Forms.Timer tmr_CheckFluidics;
        public UsrCtrl_Fluidics_Small()
        {
            InitializeComponent();

            tmr_CheckFluidics = new Timer();
            tmr_CheckFluidics.Interval = 1000;
            tmr_CheckFluidics.Tick += Tmr_CheckFluidics_Tick;
        }

        public void Init()
        {
            if (this.BackgroundImage == null)
            {
                this.BackgroundImage = Image.FromFile(@"Resource\Image\Fluidics\L-Titrator\L_TItrator_Fluidics.bmp");
            }
        }

        public void EnableFluidicsUpdate(bool enb)
        {
            if (this.BackgroundImage == null)
            {
                this.BackgroundImage = Image.FromFile(@"Resource\Image\Fluidics\L-Titrator\L_TItrator_Fluidics.bmp");
            }

            if (ATIK_MainBoard.IsInitialized(DefinedMainBoards.L_Titrator) == false)
            {
                tmr_CheckFluidics.Enabled = false;
                return;
            }
            tmr_CheckFluidics.Enabled = enb;
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
            bool valve3Way_DIW_Right = MB_Elem_Bit.GetElem("DIW_To_6Way").Get_State();
            bool valve3Way_DIW_Bottom = MB_Elem_Bit.GetElem("DIW_To_Vessel").Get_State();
            if (valve3Way_DIW_Right == valve3Way_DIW_Bottom)
            {
                Cmp3Way_DIW.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, false);
            }
            else
            {
                Cmp3Way_DIW.Draw_State(Valve_Port.Right, valve3Way_DIW_Right, Valve_Port.Bottom, valve3Way_DIW_Bottom);
            }

            // 3Way-Sample
            bool valve3Way_Sample_Bottom = MB_Elem_Bit.GetElem("Slurry_To_3Way").Get_State();
            Cmp3Way_Sample.Draw_State(Valve_Port.Left, true, Valve_Port.Bottom, valve3Way_Sample_Bottom);

            // 3Way-Reagent
            bool valve3Way_Reagent_Bottom = MB_Elem_Bit.GetElem("Ceric_To_3Way").Get_State();
            Cmp3Way_Reagent.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, valve3Way_Reagent_Bottom);

            // 6Way
            bool valve6Way_Sample_To_Vessel = MB_Elem_Bit.GetElem("6WayPair_Sample_To_Vessel").Get_State();
            bool valve6Way_Sample_To_Loop = MB_Elem_Bit.GetElem("6WayPair_Sample_To_Loop").Get_State();
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
                        CmpSyringe1_Head.Valve_Open = Valve_Port.Left;
                        break;

                    case MB_SyringeDirection.Out:
                        CmpSyringe1_Head.Valve_Open = Valve_Port.Right;
                        break;

                    case MB_SyringeDirection.Ext:
                        CmpSyringe1_Head.Valve_Open = Valve_Port.Top;
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
                        CmpSyringe2_Head.Valve_Open = Valve_Port.Right;
                        break;

                    case MB_SyringeDirection.Out:
                        CmpSyringe2_Head.Valve_Open = Valve_Port.Left;
                        break;

                    default:
                        break;
                }
            }

            // Vessel
            bool vesselFull = MB_Elem_Bit.GetElem("Level_1").Get_State();
            CmpVessel.Remains = vesselFull == true ? 100 : 0;
        }

        int idxTest = 0;
        private void CmpTest()
        {
            int testValve = idxTest % 4;
            switch (testValve)
            {
                case 0:
                    Cmp3Way_DIW.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, false);
                    Cmp3Way_Sample.Draw_State(Valve_Port.Left, false, Valve_Port.Bottom, false);
                    Cmp3Way_Reagent.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, false);
                    Cmp2Way_Drain.Draw_State(Valve_Port.Top, false, Valve_Port.Bottom, false);
                    Cmp6Way.Draw_State(Valve_6Way_State.Link_None);
                    break;

                case 1:
                    Cmp3Way_DIW.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, false);
                    Cmp3Way_Sample.Draw_State(Valve_Port.Left, true, Valve_Port.Bottom, false);
                    Cmp3Way_Reagent.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, false);
                    break;

                case 2:
                    Cmp3Way_DIW.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, true);
                    Cmp3Way_Sample.Draw_State(Valve_Port.Left, false, Valve_Port.Bottom, true);
                    Cmp3Way_Reagent.Draw_State(Valve_Port.Right, false, Valve_Port.Bottom, true);
                    Cmp2Way_Drain.Draw_State(Valve_Port.Top, true, Valve_Port.Bottom, true);
                    Cmp6Way.Draw_State(Valve_6Way_State.Link_12);
                    break;

                case 3:
                    Cmp3Way_DIW.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, true);
                    Cmp3Way_Sample.Draw_State(Valve_Port.Left, true, Valve_Port.Bottom, true);
                    Cmp3Way_Reagent.Draw_State(Valve_Port.Right, true, Valve_Port.Bottom, true);
                    Cmp6Way.Draw_State(Valve_6Way_State.Link_23);
                    break;
            }

            int testSyringeHead = idxTest % 6;
            switch (testSyringeHead)
            {
                case 0:
                    CmpSyringe1_Head.Valve_Open = Valve_Port.Left;
                    CmpSyringe2_Head.Valve_Open = Valve_Port.Left;
                    break;

                case 1:
                    break;

                case 2:
                    CmpSyringe1_Head.Valve_Open = Valve_Port.Top;
                    break;

                case 3:
                    CmpSyringe2_Head.Valve_Open = Valve_Port.Right;
                    break;

                case 4:
                    CmpSyringe1_Head.Valve_Open = Valve_Port.Right;
                    break;

                case 5:
                    break;
            }

            int testReagent = idxTest * 10;
            CmpReagent1.Remains = 100 - testReagent;
            CmpReagent2.Remains = 100 - testReagent;

            CmpSyringe1.Remains = testReagent;
            CmpSyringe2.Remains = testReagent;

            int testVesset = 100 - idxTest % 6 * 20;
            CmpVessel.Remains = testVesset;


            if (idxTest == 10)
            {
                idxTest = 0;
            }
            else
            {
                idxTest++;
            }
        }
    }
}
