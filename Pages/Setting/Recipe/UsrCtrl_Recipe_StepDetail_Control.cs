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
    public partial class UsrCtrl_Recipe_StepDetail_Control : UserControl, IAuthority
    {
        private List<MB_Elem_Bit> Elems_BitList = new List<MB_Elem_Bit>();
        private MB_Elem_Syringe Elem_Syringe1;
        private MB_Elem_Syringe Elem_Syringe2;
        private MB_Elem_Analog Elem_Mixer;

        private TableLayoutPanel Tbl_StepEnd_Time;
        private TableLayoutPanel Tbl_StepEnd_Sensor;
        private TableLayoutPanel Tbl_StepEnd_SyringeEnd;

        private PrmCmp_Collection CmpCol_StepEndEnabled_TimeDelay;
        private PrmCmp_Value CmpVal_StepEndValue_TimeDelay;

        private PrmCmp_Collection CmpCol_StepEndEnabled_Sensor;
        private PrmCmp_Collection CmpCol_StepEndValues_Sensor;

        private PrmCmp_Collection CmpCol_StepEndEnabled_Syringe;

        private Step RefStep;
        private Step PrvStep;

        public UsrCtrl_Recipe_StepDetail_Control()
        {
            InitializeComponent();
        }

        private void btn_SelectedStepEnd_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            Tbl_StepEnd_Time.Visible = clicked == btn_StepEnd_Time;
            Tbl_StepEnd_Sensor.Visible = clicked == btn_StepEnd_Sensor;
            Tbl_StepEnd_SyringeEnd.Visible = clicked == btn_StepEnd_SyringeEnd;

            clicked.BackColor = Color.MediumSeaGreen;
            var except = tbl_StepEndMenu.Controls.OfType<Button>().ToList().Where(btn => btn != clicked).ToList();
            except.ForEach(btn =>
            {
                if (btn == btn_StepEnd_Time)
                {
                    btn.BackColor = RefStep.StepEndCheck.TimeDelay.Enabled == true ? PreDef.Light_Green : Color.White;
                }
                else if (btn == btn_StepEnd_Sensor)
                {
                    btn.BackColor = RefStep.StepEndCheck.SensorDetect.Enabled == true ? PreDef.Light_Green : Color.White;
                }
                else if (btn == btn_StepEnd_SyringeEnd)
                {
                    btn.BackColor = RefStep.StepEndCheck.PositionSync.Enabled == true ? PreDef.Light_Green : Color.White;
                }
            });
        }

        public void Parse(Step stepRef)
        {
            // TBD.
            /*
            if (stepNo == 0)
            {
            }
            else
            { 
                // Merge with previous step info
            }
            */

            RefStep = stepRef;

            // Solenoids
            if (stepRef.Control_Valve == true)
            {
                List<UsrCtrl_Bit> ctrlCmps = new List<UsrCtrl_Bit>();
                stepRef.Valves.ForEach(valve =>
                {
                    pnl_Solenoids.Controls.OfType<UsrCtrl_Bit>().ToList().ForEach(ctrl =>
                    {
                        if (valve.Name == ctrl.LogicalName)
                        {
                            ctrl.Set_State(valve.Get_Condition());
                            ctrlCmps.Add(ctrl);
                        }
                    });
                });
                pnl_Solenoids.Controls.OfType<UsrCtrl_Bit>().Except(ctrlCmps).ToList().ForEach(except => except.Set_State(UsrCtrl_Bit.BitState.Unknown));
            }
            else
            {
                // legacy
                //pnl_Solenoids.Controls.OfType<UsrCtrl_Bit>().ToList().ForEach(ctrl => ctrl.Set_State(UsrCtrl_Bit.BitState.Unknown));
            }

            // Syringes
            if (stepRef.Control_Syringe == true)
            {
                List<MB_Cmp_Syringe> ctrlCmps = new List<MB_Cmp_Syringe>();
                stepRef.Syringes.ForEach(syringe =>
                {
                    tbl_Syringes.Controls.OfType<MB_Cmp_Syringe>().ToList().ForEach(ctrl =>
                    {
                        if (syringe.Name == ctrl.LogicalName)
                        {
                            ctrl.Set_State(syringe.Get_Flow(), syringe.Get_Direction(), syringe.Get_Speed(), syringe.Get_Volume_mL());
                            ctrlCmps.Add(ctrl);
                        }
                    });
                });
                tbl_Syringes.Controls.OfType<MB_Cmp_Syringe>().Except(ctrlCmps).ToList().ForEach(except => except.Set_Unknown());
            }
            else
            {
                tbl_Syringes.Controls.OfType<MB_Cmp_Syringe>().ToList().ForEach(ctrl => ctrl.Set_Unknown());
            }

            // Mixer
            if (stepRef.Control_Mixer == true)
            {
                List<UsrCtrl_Analog> ctrlCmps = new List<UsrCtrl_Analog>();
                stepRef.Mixers.ForEach(mixer =>
                {
                    pnl_Mixer.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl =>
                    {
                        ctrl.Set_State(mixer.Get_Duty());
                        ctrlCmps.Add(ctrl);
                    });
                });
                pnl_Mixer.Controls.OfType<UsrCtrl_Analog>().Except(ctrlCmps).ToList().ForEach(except => except.Set_Unknown());
            }
            else
            {
                pnl_Mixer.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl => ctrl.Set_Unknown());
            }

            if (RefStep.IsTitration == true)
            {
                tbl_StepEndCheck.Enabled = false;
            }
            else
            {
                tbl_StepEndCheck.Enabled = true;

                // StepEnd - End Check in this Step
                CmpCol_StepEndCheckInThisStep.Prm_Value = stepRef.EndCheckInThisStep;

                // StepEnd - TimeDelay
                CmpCol_StepEndEnabled_TimeDelay.Prm_Value = stepRef.StepEndCheck.TimeDelay.Enabled;
                btn_StepEnd_Time.BackColor = stepRef.StepEndCheck.TimeDelay.Enabled == true ? PreDef.Light_Green : Color.White;
                if (stepRef.StepEndCheck.TimeDelay.Enabled == true)
                {
                    CmpVal_StepEndValue_TimeDelay.Prm_Value = stepRef.StepEndCheck.TimeDelay.Time;
                }
                else
                {
                    CmpVal_StepEndValue_TimeDelay.Prm_Value = 0;
                }
                // StepEnd - Sensor
                CmpCol_StepEndEnabled_Sensor.Prm_Value = stepRef.StepEndCheck.SensorDetect.Enabled;
                btn_StepEnd_Sensor.BackColor = stepRef.StepEndCheck.SensorDetect.Enabled == true ? PreDef.Light_Green : Color.White;
                if (stepRef.StepEndCheck.SensorDetect.Enabled == true)
                {
                    CmpCol_StepEndValues_Sensor.Prm_Value = stepRef.StepEndCheck.SensorDetect.SensorNames;
                }
                else
                {
                    CmpCol_StepEndValues_Sensor.Prm_Value = "";
                }
                // StepEnd - Syringe (Position Sync)
                CmpCol_StepEndEnabled_Syringe.Prm_Value = stepRef.StepEndCheck.PositionSync.Enabled;
                btn_StepEnd_SyringeEnd.BackColor = stepRef.StepEndCheck.PositionSync.Enabled == true ? PreDef.Light_Green : Color.White;

                if (stepRef.StepEndCheck.TimeDelay.Enabled == true)
                {
                    btn_StepEnd_Time.PerformClick();
                }
                else if (stepRef.StepEndCheck.SensorDetect.Enabled == true)
                {
                    btn_StepEnd_Sensor.PerformClick();
                }
                else if (stepRef.StepEndCheck.PositionSync.Enabled == true)
                {
                    btn_StepEnd_SyringeEnd.PerformClick();
                }
                else
                {
                    Tbl_StepEnd_Time.Visible = false;
                    Tbl_StepEnd_Sensor.Visible = false;
                    Tbl_StepEnd_SyringeEnd.Visible = false;
                    tbl_StepEndMenu.Controls.OfType<Button>().ToList().ForEach(btn => btn.BackColor = Color.White);
                }
            }
        }

        public void SetMergedInfo(Step step)
        {
            PrvStep = step;

            step.Valves.ForEach(valve =>
            {
                pnl_Solenoids.Controls.OfType<UsrCtrl_Bit>().ToList().ForEach(ctrl =>
                {
                    if (valve.Name == ctrl.LogicalName)
                    {
                        ctrl.Set_MergedState(valve.Get_Condition() == true ? UsrCtrl_Bit.BitState.On : UsrCtrl_Bit.BitState.Off);
                    }
                });
            });

            step.Mixers.ForEach(mixer =>
            {
                pnl_Mixer.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl =>
                {
                    if (mixer.Name == ctrl.LogicalName)
                    {
                        ctrl.Set_MergedState(mixer.Get_Duty());
                    }
                });
            });
        }

        public void SetSameAsPrvStep()
        {
            if (PrvStep == null)
            {
                return;
            }

            //RefStep = (Step)PrvStep.Clone();

            // Valves
            PrvStep.Valves.ForEach(valve =>
            {
                RefStep.Valves.Where(refValve => refValve.Name == valve.Name).ToList()[0].Set_Condition(valve.Get_Condition());

                pnl_Solenoids.Controls.OfType<UsrCtrl_Bit>().ToList().ForEach(ctrl =>
                {
                    if (valve.Name == ctrl.LogicalName)
                    {
                        ctrl.Set_State(valve.Get_Condition() == true ? UsrCtrl_Bit.BitState.On : UsrCtrl_Bit.BitState.Off);
                    }
                });
            });

            // Mixers
            PrvStep.Mixers.ForEach(mixer =>
            {
                RefStep.Mixers.Where(refMixer => refMixer.Name == mixer.Name).ToList()[0].Set_Duty(mixer.Get_Duty());

                pnl_Mixer.Controls.OfType<UsrCtrl_Analog>().ToList().ForEach(ctrl =>
                {
                    if (mixer.Name == ctrl.LogicalName)
                    {
                        ctrl.Set_State(mixer.Get_Duty());
                    }
                });
            });

            // StepEnd - End Check in this Step
            CmpCol_StepEndCheckInThisStep.Prm_Value = PrvStep.EndCheckInThisStep;
            RefStep.EndCheckInThisStep = PrvStep.EndCheckInThisStep;

            // StepEnd - TimeDelay
            CmpCol_StepEndEnabled_TimeDelay.Prm_Value = PrvStep.StepEndCheck.TimeDelay.Enabled;
            RefStep.StepEndCheck.TimeDelay.Enabled = PrvStep.StepEndCheck.TimeDelay.Enabled;
            if (PrvStep.StepEndCheck.TimeDelay.Enabled == true)
            {
                CmpVal_StepEndValue_TimeDelay.Prm_Value = PrvStep.StepEndCheck.TimeDelay.Time;
            }
            else
            {
                CmpVal_StepEndValue_TimeDelay.Prm_Value = 0;
                PrvStep.StepEndCheck.TimeDelay.Time = 0;
            }
            RefStep.StepEndCheck.TimeDelay.Time = PrvStep.StepEndCheck.TimeDelay.Time;

            // StepEnd - Sensor
            CmpCol_StepEndEnabled_Sensor.Prm_Value = PrvStep.StepEndCheck.SensorDetect.Enabled;
            RefStep.StepEndCheck.SensorDetect.Enabled = PrvStep.StepEndCheck.SensorDetect.Enabled;
            if (PrvStep.StepEndCheck.SensorDetect.Enabled == true)
            {
                CmpCol_StepEndValues_Sensor.Prm_Value = PrvStep.StepEndCheck.SensorDetect.SensorNames;
            }
            else
            {
                CmpCol_StepEndValues_Sensor.Prm_Value = "";
                PrvStep.StepEndCheck.SensorDetect.SensorNames = "";
            }
            RefStep.StepEndCheck.SensorDetect.SensorNames = PrvStep.StepEndCheck.SensorDetect.SensorNames;

            // StepEnd - Syringe (Position Sync)
            CmpCol_StepEndEnabled_Syringe.Prm_Value = PrvStep.StepEndCheck.PositionSync.Enabled;
            RefStep.StepEndCheck.PositionSync.Enabled = PrvStep.StepEndCheck.PositionSync.Enabled;

            if (PrvStep.StepEndCheck.TimeDelay.Enabled == true)
            {
                btn_StepEnd_Time.PerformClick();
            }
            else if (PrvStep.StepEndCheck.SensorDetect.Enabled == true)
            {
                btn_StepEnd_Sensor.PerformClick();
            }
            else if (PrvStep.StepEndCheck.PositionSync.Enabled == true)
            {
                btn_StepEnd_SyringeEnd.PerformClick();
            }
            else
            {
                Tbl_StepEnd_Time.Visible = false;
                Tbl_StepEnd_Sensor.Visible = false;
                Tbl_StepEnd_SyringeEnd.Visible = false;
                tbl_StepEndMenu.Controls.OfType<Button>().ToList().ForEach(btn => btn.BackColor = Color.White);
            }
        }

        public void SetBackground()
        {
            var mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
            DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;

            // Set Solenoids
            Elems_BitList = drvLT.Get_Bits((int)DrvMB_L_Titrator.LineOrder.Solenoid_Output);
            for (int i = 0; i < Elems_BitList.Count; i++)
            {
                int ctrlHeight = 36;
                UsrCtrl_Bit ctrl = new UsrCtrl_Bit(Elems_BitList[i], false);
                //ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                ctrl.Width = pnl_Solenoids.Width - 2;
                ctrl.Height = ctrlHeight;
                int h_loc = i == 0 ? 1 : pnl_Solenoids.Controls[i - 1].Bottom + 1;
                ctrl.Location = new Point(1, h_loc);
                ctrl.Margin = new Padding(1, 1, 1, 1);
                ctrl.BitStateChangedEvent += Ctrl_BitStateChangedEventHandler;
                pnl_Solenoids.Controls.Add(ctrl);
            }

            // Set Syringes
            Elem_Syringe1 = drvLT.Get_Syringe((int)DrvMB_L_Titrator.LineOrder.Syringe_1);
            MB_Cmp_Syringe ctrl_Syringe1 = new MB_Cmp_Syringe(Elem_Syringe1, false);
            ctrl_Syringe1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            ctrl_Syringe1.Margin = new Padding(1, 1, 1, 1);
            ctrl_Syringe1.SyringeConditionChangedEvent += Ctrl_SyringeConditionChangedEventHandler;
            tbl_Syringes.Controls.Add(ctrl_Syringe1, 0, 0);

            Elem_Syringe2 = drvLT.Get_Syringe((int)DrvMB_L_Titrator.LineOrder.Syringe_2);
            MB_Cmp_Syringe ctrl_Syringe2 = new MB_Cmp_Syringe(Elem_Syringe2, false);
            ctrl_Syringe2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            ctrl_Syringe2.Margin = new Padding(1, 1, 1, 1);
            ctrl_Syringe2.SyringeConditionChangedEvent += Ctrl_SyringeConditionChangedEventHandler;
            tbl_Syringes.Controls.Add(ctrl_Syringe2, 1, 0);

            // Set Mixers
            Elem_Mixer = drvLT.Get_Analog((int)DrvMB_L_Titrator.LineOrder.Mixer_Duty);
            UsrCtrl_Analog ctrl_Mixer = new UsrCtrl_Analog(Elem_Mixer, false, true);
            ctrl_Mixer.Location = new Point(1, 1);
            ctrl_Mixer.Size = new Size(pnl_Mixer.Width - 2, 59);
            ctrl_Mixer.AnalogValueChangedEvent += Ctrl_AnalogValueChangedEventHandler;
            pnl_Mixer.Controls.Add(ctrl_Mixer);

            // Set StepEnds
            CmpCol_StepEndCheckInThisStep.Init_WithOutGenPrm("End Check in Step", new bool[] { false, true }, true);
            CmpCol_StepEndCheckInThisStep.SelectedUserItemChangedEvent += StepEndCheckInThisStep_SelectedUserItemChangedEvent;

            int cmpWidth = pnl_StepEndInfo.Width - 2;
            // Time
            Tbl_StepEnd_Time = CreateTbl_StepEnd();
            CmpCol_StepEndEnabled_TimeDelay = CreatePrmCmp_StepEnd_Enabled(cmpWidth);
            CmpCol_StepEndEnabled_TimeDelay.SelectedUserItemChangedEvent += StepEnd_EnabledChangedEvent;
            CmpVal_StepEndValue_TimeDelay = CreatePrmVal_StepEnd_Value(cmpWidth, "Time Delay [sec]", 0); // TBD. Load value from selected step
            CmpVal_StepEndValue_TimeDelay.ValueChangedEvent += StepEnd_TimeDelay_ValueChangedEvent;
            Tbl_StepEnd_Time.Controls.Add(CmpCol_StepEndEnabled_TimeDelay, 0, 0);
            Tbl_StepEnd_Time.Controls.Add(CmpVal_StepEndValue_TimeDelay, 0, 1);
            pnl_StepEndInfo.Controls.Add(Tbl_StepEnd_Time);

            // Sensor
            Tbl_StepEnd_Sensor = CreateTbl_StepEnd();
            CmpCol_StepEndEnabled_Sensor = CreatePrmCmp_StepEnd_Enabled(cmpWidth);
            CmpCol_StepEndEnabled_Sensor.SelectedUserItemChangedEvent += StepEnd_EnabledChangedEvent;
            CmpCol_StepEndValues_Sensor = CreatePrmCmp_StepEnd_Collections(cmpWidth, "Select Sensor", new string[] { PreDef.ElemLogicalName.AlarmInput.Level_1, PreDef.ElemLogicalName.AlarmInput.Level_2 }); // TBD. Load value from selected step
            CmpCol_StepEndValues_Sensor.SelectedUserItemChangedEvent += StepEnd_Sensor_SelectedUserItemChangedEvent;
            Tbl_StepEnd_Sensor.Controls.Add(CmpCol_StepEndEnabled_Sensor, 0, 0);
            Tbl_StepEnd_Sensor.Controls.Add(CmpCol_StepEndValues_Sensor, 0, 1);
            pnl_StepEndInfo.Controls.Add(Tbl_StepEnd_Sensor);

            // Syringe
            Tbl_StepEnd_SyringeEnd = CreateTbl_StepEnd();
            CmpCol_StepEndEnabled_Syringe = CreatePrmCmp_StepEnd_Enabled(cmpWidth);
            CmpCol_StepEndEnabled_Syringe.SelectedUserItemChangedEvent += StepEnd_EnabledChangedEvent;
            Tbl_StepEnd_SyringeEnd.Controls.Add(CmpCol_StepEndEnabled_Syringe, 0, 0);
            pnl_StepEndInfo.Controls.Add(Tbl_StepEnd_SyringeEnd);
        }

        private TableLayoutPanel CreateTbl_StepEnd()
        {
            TableLayoutPanel tbl = new TableLayoutPanel();
            tbl.Margin = new Padding(0, 0, 0, 0);
            tbl.Location = new Point(0, 0);
            tbl.Dock = DockStyle.Fill;
            tbl.RowStyles.Clear();
            tbl.ColumnStyles.Clear();
            tbl.RowStyles.Add(new RowStyle());
            tbl.RowStyles.Add(new RowStyle());
            tbl.RowStyles[0].SizeType = SizeType.Absolute;
            tbl.RowStyles[1].SizeType = SizeType.Percent;
            tbl.RowStyles[0].Height = 29;
            tbl.RowStyles[1].Height = 100;
            tbl.Visible = false;
            return tbl;
        }

        private PrmCmp_Collection CreatePrmCmp_StepEnd_Enabled(int width)
        {
            PrmCmp_Collection cmpCol = new PrmCmp_Collection();
            cmpCol.Init_WithOutGenPrm("Enabled", new bool[] { false, true }, false);
            cmpCol.SelectedUserItemChangedEvent += StepEnd_EnabledChangedEvent;
            cmpCol.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            cmpCol.Color_Name = Color.LemonChiffon;
            cmpCol.Orientation = Orientation.Vertical;
            cmpCol.SplitterDistance = 110;
            cmpCol.Margin = new Padding(1, 1, 1, 1);
            cmpCol.Location = new Point(1, 1);
            cmpCol.Width = width;
            return cmpCol;
        }

        private PrmCmp_Collection CreatePrmCmp_StepEnd_Collections(int width, string name, string[] values)
        {
            PrmCmp_Collection cmpCol = new PrmCmp_Collection();
            cmpCol.Init_WithOutGenPrm(name, values, values[0]);
            cmpCol.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmpCol.Color_Name = Color.LemonChiffon;
            cmpCol.Orientation = Orientation.Horizontal;
            cmpCol.Height = 54;
            cmpCol.Margin = new Padding(1, 1, 1, 1);
            cmpCol.Location = new Point(1, 1);
            cmpCol.Width = width;
            return cmpCol;
        }

        private PrmCmp_Value CreatePrmVal_StepEnd_Value(int width, string name, object value)
        {
            PrmCmp_Value cmpVal = new PrmCmp_Value();
            cmpVal.Init_WithOutGenPrm(name, value);
            cmpVal.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmpVal.Color_Name = Color.LemonChiffon;
            cmpVal.Color_Value = Color.White;
            cmpVal.Orientation = Orientation.Horizontal;
            cmpVal.Height = 54;
            cmpVal.Margin = new Padding(1, 1, 1, 1);
            cmpVal.Location = new Point(1, 1);
            cmpVal.Width = width;
            cmpVal.UseKeyPadUI = true;
            return cmpVal;
        }

        public void EnableControl(bool enb)
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(ctrl => ctrl.EnableModifying(true, enb));
            Handle_UI.GetAllControlsRecursive(this).ForEach(ctrl =>
            {
                if (ctrl.GetType() == typeof(UsrCtrl_Analog))
                {
                    UsrCtrl_Analog analog = (UsrCtrl_Analog)ctrl;
                    analog.EnableControl(enb);
                }
                else if (ctrl.GetType() == typeof(UsrCtrl_Bit))
                {
                    UsrCtrl_Bit bit = (UsrCtrl_Bit)ctrl;
                    bit.EnableControl(enb);
                }
                else if (ctrl.GetType() == typeof(MB_Cmp_Syringe))
                {
                    MB_Cmp_Syringe syringe = (MB_Cmp_Syringe)ctrl;
                    syringe.EnableEdit(enb);
                    //syringe.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
                }
            });
        }

        public void UserAuthorityIsChanged()
        {
            EnableControl(GlbVar.CurrentAuthority == UserAuthority.Admin);
        }
    }
}
