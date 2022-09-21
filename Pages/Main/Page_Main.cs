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
        private Dictionary<DrvMB_L_Titrator.LineOrder, UsrCtrl_MeasureResult> MeasResultDic = new Dictionary<DrvMB_L_Titrator.LineOrder, UsrCtrl_MeasureResult>();
        public Page_Main()
        {
            InitializeComponent();

            Init_ResultControls();
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
            usrCtrl_SeqStepInfo.Set_Sequence(seqName);
            usrCtrl_SeqStepInfo.Set_Step(stepName);

            Log.WriteLog("Debug", $"Print SeqStep Info: {seqName}, {stepName}");
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
            usrCtrl_Fluidics_Small1.EnableFluidicsUpdate(visible);
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

        private void chk_OnlineMode_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
