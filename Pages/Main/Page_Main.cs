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
    public partial class Page_Main : UserControl, IPage
    {
        public delegate void OnlineModeChanged(OnlineMode onlineMode);
        public event OnlineModeChanged OnlineModeChangedEvent;

        private Dictionary<DrvMB_L_Titrator.LineOrder, UsrCtrl_MeasureResult> MeasResultDic = new Dictionary<DrvMB_L_Titrator.LineOrder, UsrCtrl_MeasureResult>();

        public Page_Main()
        {
            InitializeComponent();

            Init_ResultControls();
        }

        public void Init()
        {
            btn_OnlineMode.Text = GlbVar.OnlineMode.ToString().ToUpper();
            btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;
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
            List<TitrationRef> titrationRefs = LT_Recipe.Get_AllTitrationRef(rcpNo);
            titrationRefs.ForEach(titrationRef =>
            {
                MB_Elem_Analog elem = MB_Elem_Analog.GetElem(titrationRef.AnalogInfo.AnalogInputLogicalName);
                if (elem != null)
                {
                    MeasResultDic[(DrvMB_L_Titrator.LineOrder)elem.LineNo].Init_Info(titrationRef.SampleName, titrationRef.ResultUnit);
                }
                else
                {
                    MeasResultDic[(DrvMB_L_Titrator.LineOrder)elem.LineNo].Clear();
                }
            });
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

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
            usrCtrl_Fluidics_Small1.EnableFluidicsUpdate(visible);
            
            tmr_StatusCheck.Enabled = visible;
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

        private void btn_Measure_Click(object sender, EventArgs e)
        {
            if (GlbVar.CurrentAuthority == UserAuthority.User)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Log-In First.");
                msgNtf.ShowDialog();

                return;
            }
            if (GlbVar.OnlineMode == OnlineMode.Remote && LT_Config.GenPrm_Manual_AvailableOnRemote.Value == false)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Manual operation is disabled on Remote Mode.");
                msgNtf.ShowDialog();

                return;
            }
            if (LT_Config.GenPrm_Manual_Enabled.Value == false)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Manual operation is disabled.");
                msgNtf.ShowDialog();

                return;
            }

            // Check Alarm
            Frm_NumPad numPad = new Frm_NumPad("Recipe No", (int)0);
            if (numPad.ShowDialog() == DialogResult.OK)
            {
                int rcpNo = (int)numPad.NewValue;
                if (LT_Recipe.RecipeExist(rcpNo) == true)
                {
                    numPad = new Frm_NumPad("Iteration Count", (int)0);
                    if (numPad.ShowDialog() == DialogResult.OK)
                    {
                        int iterCnt = (int)numPad.NewValue;
                        if (iterCnt > 0)
                        {
                            FluidicsControl.StartMeasure(rcpNo, iterCnt);
                        }
                    }
                }
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
                GlbVar.OnlineMode = tgtMode;

                btn_OnlineMode.Text = GlbVar.OnlineMode.ToString().ToUpper();
                btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;

                OnlineModeChangedEvent?.Invoke(GlbVar.OnlineMode);
            }
        }

        // 측정 중인 상태에서는 표시만 한다.
        private void tmr_StatusCheck_Tick(object sender, EventArgs e)
        {
            // Com.Mainboard
            var mbDrv = ATIK_MainBoard.Get_Driver(DefinedMainBoards.L_Titrator);
            DrvMB_L_Titrator drvLT = (DrvMB_L_Titrator)mbDrv;
            if (drvLT.IsInitialized == true)
            {
                if (drvLT.ComStatus == true)
                {
                    lbl_Sts_Mainboard.Text = "OK";
                    lbl_Sts_Mainboard.BackColor = Color.MediumSeaGreen;
                }
                else
                {
                    lbl_Sts_Mainboard.Text = "Error";
                    lbl_Sts_Mainboard.BackColor = Color.Crimson;
                }
            }
            else
            {
                lbl_Sts_Mainboard.Text = "None";
                lbl_Sts_Mainboard.BackColor = Color.DarkGray;
            }

            // TBD. Com.External
            lbl_Sts_External.Text = "None";
            lbl_Sts_External.BackColor = Color.DarkGray;

            // Leak1
            var elem_Leak1 = MB_Elem_Bit.GetElem("Leak_1");
            if (elem_Leak1.Get_State() == true)
            {
                lbl_Sts_Leak1.Text = "OK";
                lbl_Sts_Leak1.BackColor = Color.MediumSeaGreen;
            }
            else
            {
                lbl_Sts_Leak1.Text = "Error";
                lbl_Sts_Leak1.BackColor = Color.Crimson;
            }

            // Leak2
            var elem_Leak2 = MB_Elem_Bit.GetElem("Leak_2");
            if (elem_Leak2.Get_State() == true)
            {
                lbl_Sts_Leak2.Text = "OK";
                lbl_Sts_Leak2.BackColor = Color.MediumSeaGreen;
            }
            else
            {
                lbl_Sts_Leak2.Text = "Error";
                lbl_Sts_Leak2.BackColor = Color.Crimson;
            }

            // Overflow
            var elem_Overflow = MB_Elem_Bit.GetElem("Level_2");
            if (elem_Overflow.Get_State() == true)
            {
                lbl_Sts_Overflow.Text = "OK";
                lbl_Sts_Overflow.BackColor = Color.MediumSeaGreen;
            }
            else
            {
                lbl_Sts_Overflow.Text = "Error";
                lbl_Sts_Overflow.BackColor = Color.Crimson;
            }
        }
    }
}
