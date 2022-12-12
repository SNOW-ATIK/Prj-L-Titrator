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
    public partial class Page_Main : UserControl, IPage, IAuthority
    {
        public enum StatusCategory
        { 
            Com_Internal,
            Com_External,
            Leak,
            OverFlow,
            LifeTime
        }

        public delegate void OnlineModeChanged(OnlineMode onlineMode);
        public event OnlineModeChanged OnlineModeChangedEvent;

        private Dictionary<DrvMB_L_Titrator.LineOrder, UsrCtrl_MeasureResult> MeasResultDic = new Dictionary<DrvMB_L_Titrator.LineOrder, UsrCtrl_MeasureResult>();

        private MsgFrm_AskYesNo MsgFrm_Abort = new MsgFrm_AskYesNo("Do you want to Abort");

        public Page_Main()
        {
            InitializeComponent();

            Init_ResultControls();
        }

        public void Init()
        {
            btn_OnlineMode.Text = GlbVar.OnlineMode.ToString().ToUpper();
            btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;

            btn_Flush.Tag = LT_Recipe.HotKeyRecipeNo.Flush;
            btn_VesselEmpty.Tag = LT_Recipe.HotKeyRecipeNo.VesselEmpty;
            btn_RefillSyringe1.Tag = LT_Recipe.HotKeyRecipeNo.RefillSyringe1;
            btn_RefillSyringe2.Tag = LT_Recipe.HotKeyRecipeNo.RefillSyringe2;
            btn_VLD1.Tag = LT_Recipe.HotKeyRecipeNo.Validation1;
            btn_VLD2.Tag = LT_Recipe.HotKeyRecipeNo.Validation2;
        }

        public void Init_ResultControls()
        {
            MeasResultDic.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch0, usrCtrl_MeasureResult1);
            MeasResultDic.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch1, usrCtrl_MeasureResult2);
            MeasResultDic.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch2, usrCtrl_MeasureResult3);
            MeasResultDic.Add(DrvMB_L_Titrator.LineOrder.Analog_Input_Ch3, usrCtrl_MeasureResult4);
        }

        public void Set_SeqStepInfo(string seqName, string stepName)
        {
            Log.WriteLog("Debug", $"SeqStep Info: {seqName}, {stepName}");
        }

        public void MeasureStatus_SetContent(UsrCtrl_MeasureStatus.Contents content, object info)
        {
            usrCtrl_MeasureStatus1.Set_Content(content, info);
        }

        public void TitrationGraph_Init(int rcpNo)
        {
            if (LT_Recipe.Get_RecipeObj(rcpNo, out var rcpObj) == true)
            {
                usrCtrl_TitrationGraph1.Init_Background(rcpObj);
            }
        }

        public void TitrationGrapgh_AddPoint(string stepName, double injVol, double analog_mV)
        {
            usrCtrl_TitrationGraph1.AddPoint(stepName, injVol, analog_mV);
        }

        public void MeasureResult_Init(int rcpNo)
        {
            List<AnalyzeRef> analyzeRefs = LT_Recipe.Get_AllAnalyzeRef(rcpNo);
            analyzeRefs.ForEach(analyzeRef =>
            {
                switch (analyzeRef.Type)
                {
                    case AnalyzeType.pH:
                    case AnalyzeType.ORP:
                        MB_Elem_Analog elem = MB_Elem_Analog.GetElem(analyzeRef.TtrRef.AnalogInfo.AnalogInputLogicalName);
                        if (elem != null)
                        {
                            MeasResultDic[(DrvMB_L_Titrator.LineOrder)elem.LineNo].Init_Info(analyzeRef.TtrRef.SampleName, analyzeRef.TtrRef.ResultUnit);
                        }
                        else
                        {
                            MeasResultDic[(DrvMB_L_Titrator.LineOrder)elem.LineNo].Clear();
                        }
                        break;

                    case AnalyzeType.ISE:
                        break;
                }
            });
        }

        public void MeasureResult_Clear()
        {
            MeasResultDic.Values.ToList().ForEach(ctrl => ctrl.Clear());
        }

        public void MeasureResult_SetResult(DrvMB_L_Titrator.LineOrder lineOrder, double concentration, double analog_mV, double output_mA)
        {
            MeasResultDic[lineOrder].Set_Result(concentration, analog_mV, output_mA);
        }

        private void chk_CenterSubjectView(object sender, EventArgs e)
        {
            var selectedChkBox = (CheckBox)sender;

            if (selectedChkBox.Checked == true)
            {
                if (selectedChkBox == chk_ShowTitrateGraph)
                {
                    // Show Fluidics
                    tbl_CenterSubjectView.ColumnStyles[1].SizeType = SizeType.Percent;

                    tbl_CenterSubjectView.ColumnStyles[0].Width = 0;
                    tbl_CenterSubjectView.ColumnStyles[1].Width = 100;
                }
                else if (selectedChkBox == chk_ShowFluidics)
                {
                    // Show TitrationGraph
                    tbl_CenterSubjectView.ColumnStyles[1].SizeType = SizeType.Percent;

                    tbl_CenterSubjectView.ColumnStyles[0].Width = 100;
                    tbl_CenterSubjectView.ColumnStyles[1].Width = 0;
                }
                else if (selectedChkBox == chk_ShowBoth)
                {
                    // Show Both
                    tbl_CenterSubjectView.ColumnStyles[1].SizeType = SizeType.Absolute;

                    tbl_CenterSubjectView.ColumnStyles[0].Width = 100;
                    tbl_CenterSubjectView.ColumnStyles[1].Width = 410;
                }

                var chkBoxes = tbl_SelectCenterSubject.Controls.OfType<CheckBox>().ToList();
                var unselected = chkBoxes.Except(new List<CheckBox>() { selectedChkBox }).ToList();
                unselected.ForEach(chk => chk.Checked = false);
            }
        }

        public void CloseAbortMsgFrm()
        {
            if (MsgFrm_Abort.IsShown == true)
            {
                MsgFrm_Abort.Close();
            }
        }

        public void RunEnd()
        {
            if (this.Visible == true)
            {
                UserAuthorityIsChanged();
            }
        }

        public bool IsAdmin()
        { 
            if (GlbVar.CurrentAuthority == UserAuthority.User)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Log-In First.");
                msgNtf.ShowDialog();

                return false;
            }
            return true;
        }

        public bool IsManualMode()
        {
            if (GlbVar.OnlineMode == OnlineMode.Remote && LT_Config.GenPrm_Manual_AvailableOnRemote.Value == false)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Manual operation is disabled on Remote Mode.");
                msgNtf.ShowDialog();

                return false;
            }
            if (LT_Config.GenPrm_Manual_Enabled.Value == false)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Manual operation is disabled.");
                msgNtf.ShowDialog();

                return false;
            }
            return true;
        }

        public bool IsInitialized()
        {
            if (FluidicsControl.MainState == FluidicsState.None)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Initialize first.");
                msgNtf.ShowDialog();

                return false;
            }
            return true;
        }

        public bool IsIdle()
        {
            if (FluidicsControl.MainState == FluidicsState.Error)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Clear alarm condition and Initialize.");
                msgNtf.ShowDialog();

                return false;
            }
            if (FluidicsControl.MainState == FluidicsState.Run)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Sequence is Running.");
                msgNtf.ShowDialog();

                return false;
            }
            return true;
        }

        private void EnableUserInput(bool enbStart, bool enbAbort)
        {
            btn_OnlineMode.Enabled = enbStart;
            btn_Initialize.Enabled = enbStart;
            btn_Measure.Enabled = enbStart;
            tbl_HotKeys.Enabled = enbStart;

            btn_Abort.Enabled = enbAbort;
        }

        private void btn_Initialize_Click(object sender, EventArgs e)
        {
            if (IsAdmin() == false)
            {
                return;
            }

            FluidicsControl.StartHotKeyFunction((int)LT_Recipe.HotKeyRecipeNo.Initialize, 1);

            EnableUserInput(false, true);
        }

        private void btn_Measure_Click(object sender, EventArgs e)
        {
            if (IsAdmin() == false)
            {
                return;
            }
            if (IsManualMode() == false)
            {
                return;
            }
            if (IsIdle() == false)
            {
                return;
            }
            if (IsInitialized() == false)
            {
                return;
            }

            Frm_ManualStart frm = new Frm_ManualStart();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                FluidicsControl.StartMeasure(frm.RecipeNo, frm.IterationCounts);

                EnableUserInput(false, true);
            }
        }

        private void btn_Abort_Click(object sender, EventArgs e)
        {
            if (FluidicsControl.MainState == FluidicsState.Run)
            {
                if (MsgFrm_Abort.ShowDialog() == DialogResult.Yes)
                {
                    FluidicsControl.AbortMeasure();
                }
            }
        }

        private void HotKeyClicked(object sender, EventArgs e)
        {
            if (IsAdmin() == false)
            {
                return;
            }
            if (IsManualMode() == false)
            {
                return;
            }
            if (IsIdle() == false)
            {
                return;
            }

            Button btn = (Button)sender;
            int iterCnt = 1;
            LT_Recipe.HotKeyRecipeNo hotKey = (LT_Recipe.HotKeyRecipeNo)btn.Tag;
            switch (hotKey)
            {
                case LT_Recipe.HotKeyRecipeNo.Flush:
                case LT_Recipe.HotKeyRecipeNo.RefillSyringe1:
                case LT_Recipe.HotKeyRecipeNo.RefillSyringe2:
                    Frm_NumPad numPad = new Frm_NumPad($"{hotKey}", (int)1);
                    if (numPad.ShowDialog() == DialogResult.OK)
                    {
                        iterCnt = (int)numPad.NewValue;
                    }
                    break;

                case LT_Recipe.HotKeyRecipeNo.VesselEmpty:
                    break;

                case LT_Recipe.HotKeyRecipeNo.Validation1:
                case LT_Recipe.HotKeyRecipeNo.Validation2:
                    break;
            }

            if (iterCnt > 0)
            {
                FluidicsControl.StartHotKeyFunction((int)hotKey, iterCnt);

                EnableUserInput(false, true);
            }
        }

        public void EnableOnlineModeChangeButton(bool enb)
        {
            btn_OnlineMode.Enabled = enb;
            if (enb == true)
            {
                btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;
            }
            else
            {
                btn_OnlineMode.BackColor = Color.DarkGray;
            }
        }

        private void btn_OnlineMode_Click(object sender, EventArgs e)
        {
            if (GlbVar.CurrentAuthority == UserAuthority.User)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Log In First.");
                msgNtf.ShowDialog();

                return;
            }

            OnlineMode tgtMode = GlbVar.OnlineMode == OnlineMode.Remote ? OnlineMode.Local : OnlineMode.Remote;
            MsgFrm_AskYesNo msgAsk = new MsgFrm_AskYesNo($"Do you want to change OnlineMode to {tgtMode.ToString().ToUpper()}?");
            if (msgAsk.ShowDialog() == DialogResult.Yes)
            {
                Set_OnlineMode(tgtMode);
            }
        }

        private void Set_OnlineMode(OnlineMode onlineMode)
        {
            GlbVar.OnlineMode = onlineMode;

            btn_OnlineMode.Text = GlbVar.OnlineMode.ToString().ToUpper();
            btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;

            OnlineModeChangedEvent?.Invoke(GlbVar.OnlineMode);
        }

        public void Update_AlarmStatus()
        {
            if (this.Visible == false)
            {
                return;
            }
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(Update_AlarmStatus));
                return;
            }

            string stsText = string.Empty;
            Color stsColor = Color.Gray;
            var alarmNames = Enum.GetValues(typeof(AlarmName));
            for (int i = 0; i < alarmNames.Length; i++)
            {
                AlarmName name = (AlarmName)alarmNames.GetValue(i);
                switch (name)
                {
                    case AlarmName.Com_Internal:
                    case AlarmName.Com_External:
                    case AlarmName.Leak:
                    case AlarmName.OverFlow:
                    case AlarmName.LifeTimeExpired:
                        var alarmObj = LT_Alarm.GetAlarmObject(name);
                        if (alarmObj != null)
                        {
                            if (alarmObj.Gen_Level.Value == AlarmLevel.NoUse)
                            {
                                stsText = "NoUse";
                                stsColor = Color.DarkGray;
                            }
                            else
                            {
                                if (LT_Alarm.IsAlarmOn(name) == true)
                                {
                                    switch (alarmObj.Gen_Level.Value)
                                    {
                                        case AlarmLevel.Notify:
                                            stsText = "OK";
                                            stsColor = Color.White;
                                            break;

                                        case AlarmLevel.Warning:
                                            stsText = "Warning";
                                            stsColor = Color.DarkOrange;
                                            break;

                                        case AlarmLevel.Critical:
                                            stsText = "Error";
                                            stsColor = Color.Crimson;
                                            break;
                                    }
                                }
                                else
                                {
                                    stsText = "OK";
                                    stsColor = Color.White;
                                }
                            }
                        }

                        Label lbl = Get_StatusLabel(name);
                        if (lbl != null)
                        {
                            lbl.Text = stsText;
                            lbl.BackColor = stsColor;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private Label Get_StatusLabel(AlarmName alarmName)
        {
            Label lbl = null;
            switch (alarmName)
            {
                case AlarmName.Com_Internal:
                    lbl = lbl_Sts_Mainboard;
                    break;

                case AlarmName.Com_External:
                    lbl = lbl_Sts_External;
                    break;

                case AlarmName.Leak:
                    lbl = lbl_Sts_Leak1;
                    break;

                case AlarmName.OverFlow:
                    lbl = lbl_Sts_Overflow;
                    break;

                case AlarmName.LifeTimeExpired:
                    lbl = lbl_Sts_LifeTime;
                    break;
            }
            return lbl;
        }

        private void btn_StartFunction_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = btn.Enabled == true ? Color.White : Color.DarkGray;
        }

        private void tbl_HotKeys_EnabledChanged(object sender, EventArgs e)
        {
            if (tbl_HotKeys.Enabled == true)
            {
                btn_VLD1.Enabled = LT_Recipe.RecipeExist((int)LT_Recipe.HotKeyRecipeNo.Validation1);
                btn_VLD2.Enabled = LT_Recipe.RecipeExist((int)LT_Recipe.HotKeyRecipeNo.Validation2);
                btn_Measure.Enabled = LT_Config.GenPrm_Manual_Enabled.Value;
                btn_Abort.Enabled = LT_Config.GenPrm_Manual_Enabled.Value;
            }
        }

        public void UserAuthorityIsChanged()
        {
            switch (GlbVar.CurrentAuthority)
            {
                case UserAuthority.User:
                case UserAuthority.Engineer:
                    if (GlbVar.OnlineMode == OnlineMode.Local)
                    {
                        Set_OnlineMode(OnlineMode.Remote);
                    }
                    EnableUserInput(false, false);
                    break;

                case UserAuthority.Admin:
                    bool bStartInputEnabled = GlbVar.CurrentMainState != MainState.Run;
                    bool bAbortInputEnabled = GlbVar.CurrentMainState == MainState.Run;
                    EnableUserInput(bStartInputEnabled, bAbortInputEnabled);
                    break;
            }
        }

        private void Page_Main_VisibleChanged(object sender, EventArgs e)
        {
            usrCtrl_Fluidics_Small1.EnableFluidicsUpdate(this.Visible);

            if (this.Visible == true)
            {
                UserAuthorityIsChanged();
                Update_AlarmStatus();
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
    }
}
