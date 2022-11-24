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
using ATIK.Communication.SerialPort;

using L_Titrator.Pages;
using L_Titrator.Controls;

namespace L_Titrator
{
    public partial class Frm_Main : Form
    {
        public Frm_Main()
        {
            Init_Logs();

            InitializeComponent();

            System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            lbl_AppVersion.Text = "S/W Ver: " + assemblyName.Version.ToString();

            bool loadAlarm = LT_Alarm.Load();
            if (loadAlarm == true)
            {
                LT_Alarm.AlarmUpdateEvent += AlarmUpdated;
            }

            bool loadCfg = LT_Config.Load();

            bool loadRcp = LT_Recipe.Load();

            bool loadLifeTime = LT_LifeTime.Load();

            Init_Devices();

            InitMenuAndPage();

            Init_Sequence();
                        
            tmr_StateCheck.Enabled = true;

            if (Enum.TryParse(LT_Config.GenPrm_LogInAuthority.Value, out UserAuthority logInAuthority) == true)
            {
                GlbVar.CurrentAuthority = logInAuthority;
                usrCtrl_Login1.SetAuthorityText();
            }

            //AlarmUpdated();

            /*********************************************************************************************************************/
            //Frm_StrKeyPad frm = new Frm_StrKeyPad("Test", "Test");
            //frm.ShowDialog();

            /*********************************************************************************************************************/
            //HistoryObj test = new HistoryObj(0, DateTime.Now);

            //test.File_AddKey(HistoryObj.Section.Titration, HistoryObj.TitrationKey.Offset_mL, 2.5);

            //test.File_AddKey(HistoryObj.Section.Result, HistoryObj.ResultKey.IterationCount, 5);
            //List<InjectedObj> injObjs = new List<InjectedObj>();
            //DateTime start = DateTime.Now - TimeSpan.FromMinutes(5);
            //double injTotal = 0;
            //for (int i = 0; i < 5; i++)
            //{
            //    InjectedObj injObj = new InjectedObj()
            //    {
            //        No = i,
            //        Time = start + TimeSpan.FromMinutes(i),
            //        InjVol_Single = 0.02 * i,
            //        InjVol_Accum = injTotal + 0.02 * i,
            //        Analog = i * 100 + 100,
            //        Concentration = 0.1 * i
            //    };
            //    test.File_AddKey(HistoryObj.Section.Result, HistoryObj.ResultKey.InjectionInfo, injObj.ToString());
            //}
            //test.File_Save();

            /*********************************************************************************************************************/
            //PageMain.MeasureResult_Init(5);
            //PageMain.MeasureResult_SetResult(BoardDef.LineOrder.Analog_Input_Ch1, 11.1, 848, 12);

            //PageMain.TitrationGraph_Init(5);

            //PageMain.TitrationGrapgh_AddPoint("H2O2", 0, 320);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.5, 533);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.52, 529);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.54, 529);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.56, 531);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.58, 531);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.60, 535);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.62, 539);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.64, 545);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.66, 554);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.68, 575);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.69, 625);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.70, 720);
            //PageMain.TitrationGrapgh_AddPoint("H2O2", 2.71, 833);
        }

        private void Init_Logs()
        {
            Array logSections = Enum.GetValues(typeof(LogSection));
            for (int i = 0; i < logSections.Length; i++)
            {
                Log.Init_Log(PreDef.Path_Log, logSections.GetValue(i));
            }
        }

        private bool Init_Devices()
        {
            if (Element_SerialPort.Load_Config($@"Config\Device\", "Element_SerialPort.xml", "L_Titrator", "Log") == true)
            {
                if (ATIK_MainBoard.Initialize(@"Config\Device\ATIK_MainBoard.xml", LT_Config.GenPrm_InterlockEnabled.Value) == true)
                {
                    return true;
                }
                Log.WriteLog("Error", "Failed to Init. ATIK_MainBoard.");
                return false;
            }

            Log.WriteLog("Error", "Failed to Load ElemSerialPort");
            return false;
        }

        private bool Init_Sequence()
        {
            bool bInit = FluidicsControl.Initialize();
            FluidicsControl.RunEnd += Fluidics_RunEnd;
            FluidicsControl.NotifyProcessInfo += Fluidics_ProcessChanged;

            return bInit;
        }

        private void Fluidics_RunEnd(RunEndState runEndState)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Fluidics_RunEnd(runEndState)));
                return;
            }

            PageMain.CloseAbortMsgFrm();

            if (runEndState == RunEndState.Success)
            {
                MsgFrm_NotifyOnly notify = new MsgFrm_NotifyOnly("Run End");
                notify.ShowDialog();
            }
            else
            {
                MsgFrm_NotifyOnly notify = new MsgFrm_NotifyOnly("Run Cancel");
                notify.ShowDialog();
            }

            PageMain.RunEnd();
        }

        private void Fluidics_ProcessChanged(FluidicsControl.NotifyProcess notifyProcess, object info)
        {
            object[] multiInfo = null;
            switch (notifyProcess)
            {
                case FluidicsControl.NotifyProcess.Clear:
                    PageMain.Set_SeqStepInfo("None", "None");
                    PageMain.MeasureStatus_SetContent(UsrCtrl_MeasureStatus.Contents.Clear, null);
                    PageMain.MeasureResult_Clear();
                    break;

                case FluidicsControl.NotifyProcess.Start:
                    multiInfo = (object[])info;
                    
                    PageMain.MeasureStatus_SetContent(UsrCtrl_MeasureStatus.Contents.StartTime, multiInfo);
                    PageMain.MeasureResult_Init((int)multiInfo[1]);
                    PageMain.TitrationGraph_Init((int)multiInfo[1]);
                    break;

                case FluidicsControl.NotifyProcess.ChangeSeqStep:
                    multiInfo = (object[])info;
                    PageMain.Set_SeqStepInfo((string)multiInfo[0], (string)multiInfo[1]);
                    break;

                case FluidicsControl.NotifyProcess.Result:
                    multiInfo = (object[])info;
                    PageMain.MeasureResult_SetResult((DrvMB_L_Titrator.LineOrder)multiInfo[0], (double)multiInfo[1], (double)multiInfo[2], (double)multiInfo[3]);
                    break;

                case FluidicsControl.NotifyProcess.AddPoint:
                    multiInfo = (object[])info;
                    PageMain.TitrationGrapgh_AddPoint((string)multiInfo[0], (double)multiInfo[1], (double)multiInfo[2]);
                    break;

                case FluidicsControl.NotifyProcess.Log:
                    PageMain.MeasureStatus_SetContent(UsrCtrl_MeasureStatus.Contents.Log, info);
                    break;

                case FluidicsControl.NotifyProcess.TitrationEnd:
                    PageMain.MeasureStatus_SetContent(UsrCtrl_MeasureStatus.Contents.TitrationEndTime, info);
                    break;

                case FluidicsControl.NotifyProcess.SequenceEnd:
                    PageMain.MeasureStatus_SetContent(UsrCtrl_MeasureStatus.Contents.SequenceEndTime, info);
                    break;

                case FluidicsControl.NotifyProcess.Error:
                    break;
            }
        }

        private void tmr_StateCheck_Tick(object sender, EventArgs e)
        {
            Update_MainState();
        }

        private void Update_MainState()
        {
            // Com.Internal(Mainboard)
            var mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
            DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;
            if (drvLT.IsInitialized == true)
            {
                if (drvLT.ComStatus == true)
                {
                    LT_Alarm.Reset_Alarm(AlarmName.Com_Internal);
                }
                else
                {
                    LT_Alarm.Set_Alarm(AlarmName.Com_Internal);
                }
            }
            else
            {
                LT_Alarm.Set_Alarm(AlarmName.Com_Internal);
            }

            // TBD: Com.External

            // Leak
            var elem_Leak1 = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Leak_1);
            if (elem_Leak1.Get_State() == PreDef.Interlock_Occur_BooleanValue)
            {
                LT_Alarm.Set_Alarm(AlarmName.Leak);
            }
            else
            {
                LT_Alarm.Reset_Alarm(AlarmName.Leak);
            }

            // OverFlow
            var elem_Overflow = MB_Elem_Bit.GetElem(PreDef.ElemLogicalName.AlarmInput.Level_2);
            if (elem_Overflow.Get_State() == PreDef.Interlock_Occur_BooleanValue)
            {
                LT_Alarm.Set_Alarm(AlarmName.OverFlow);
            }
            else
            {
                LT_Alarm.Reset_Alarm(AlarmName.OverFlow);
            }

            // LifeTime
            if (LT_LifeTime.IsExpiredPartExist() == true)
            {
                LT_Alarm.Set_Alarm(AlarmName.LifeTimeExpired);
            }
            else
            {
                LT_Alarm.Reset_Alarm(AlarmName.LifeTimeExpired);
            }

            // Update MainState
            if (LT_Alarm.IsAlarmExist() == true)
            {
                if (LT_Alarm.IsCriticalAlarmExist() == true)
                {
                    usrCtrl_Status1.Set_State(MainState.Error);
                }
                else
                {
                    usrCtrl_Status1.Set_State(MainState.Warning);
                }
            }
            else
            {
                switch (FluidicsControl.MainState)
                {
                    case FluidicsState.None:
                        usrCtrl_Status1.Set_State(MainState.Unknown);
                        break;

                    case FluidicsState.Idle:
                        usrCtrl_Status1.Set_State(MainState.Idle);
                        break;

                    case FluidicsState.Run:
                        usrCtrl_Status1.Set_State(MainState.Run);
                        break;

                    case FluidicsState.Error:
                        usrCtrl_Status1.Set_State(MainState.Error);
                        break;
                }
            }
        }

        private void AlarmUpdated()
        {
            PageMain.Update_AlarmStatus();
        }
    }
}
