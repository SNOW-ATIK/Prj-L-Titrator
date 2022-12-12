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

namespace L_Titrator.Pages
{
    public partial class UsrCtrl_Recipe_StepDetail_Titration : UserControl, IAuthority
    {
        private TitrationRef Ref_TtrCfg;               // 수정 대상
        private TitrationRef Org_TtrCfg_ToCompare;     // 비교 원본 (cloned)
        private Step RefStep;
        private Step OrgStep_ToCompare;

        public UsrCtrl_Recipe_StepDetail_Titration()
        {
            InitializeComponent();
        }

        public void Parse(Step stepRef)
        {
            RefStep = stepRef;
            OrgStep_ToCompare = (Step)RefStep.Clone();

            Ref_TtrCfg = RefStep.AnalyzeRefObj.TtrRef;
            Org_TtrCfg_ToCompare = OrgStep_ToCompare.AnalyzeRefObj.TtrRef;

            if (Ref_TtrCfg != null)
            {
                CmpVal_RefFile.Prm_Value = stepRef.AnalyzeRefFileName;
                CmpVal_SampleName.Prm_Value = Ref_TtrCfg.SampleName;
                CmpVal_ReagentName.Prm_Value = Ref_TtrCfg.InjectionInfo.ReagentName;
                CmpVal_ScaleFactor.Prm_Value = Ref_TtrCfg.ScaleFactor;
                CmpVal_DisplayUnit.Prm_Value = Ref_TtrCfg.ResultUnit;

                CmpVal_MixingTime_Offset.Prm_Value = Ref_TtrCfg.MixingTime_AfterOffset;
                CmpVal_MixingTime_General.Prm_Value = Ref_TtrCfg.MixingTime_General;

                CmpCol_Sensor.Prm_Value = Ref_TtrCfg.AnalogInfo.AnalogInputLogicalName;
                CmpVal_Analog_Target.Prm_Value = Ref_TtrCfg.AnalogInfo.TargetValue;
                CmpVal_Analog_End.Prm_Value = Ref_TtrCfg.AnalogInfo.EndValue;

                CmpCol_Syringe.Prm_Value = Ref_TtrCfg.InjectionInfo.ReagentSyringeLogicalName;
                CmpVal_MaxIteration.Prm_Value = Ref_TtrCfg.MaxIterationCount;
                CmpVal_Offset.Prm_Value = Ref_TtrCfg.InjectionInfo.Offset;
                CmpVal_Change_LargeToMiddle.Prm_Value = Ref_TtrCfg.InjectionInfo.IncThreshold_ChangeToMiddle;
                CmpVal_Change_MiddleToSmall.Prm_Value = Ref_TtrCfg.InjectionInfo.IncThreshold_ChangeToSmall;
                CmpVal_Inj_Large.Prm_Value = Ref_TtrCfg.InjectionInfo.Inc_Large;
                CmpVal_Inj_Middle.Prm_Value = Ref_TtrCfg.InjectionInfo.Inc_Middle;
                CmpVal_Inj_Small.Prm_Value = Ref_TtrCfg.InjectionInfo.Inc_Small;
                CmpCol_EnableInterpolation.Prm_Value = Ref_TtrCfg.EnableInterpolation;

                CmpCol_VLD_Enabled.Prm_Value = Ref_TtrCfg.ValidationInfo.Enabled;
                CmpVal_VLD_Ref.Prm_Value = Ref_TtrCfg.ValidationInfo.ReferenceValue;
                CmpVal_VLD_Low.Prm_Value = Ref_TtrCfg.ValidationInfo.Limit_Low;
                CmpVal_VLD_High.Prm_Value = Ref_TtrCfg.ValidationInfo.Limit_High;
            }
        }

        public void SetBackground()
        {
            CmpVal_RefFile.Init_WithOutGenPrm("Reference File", "");
            CmpVal_RefFile.ValueClickedEvent += CmpVal_EditWordEvent;

            CmpVal_SampleName.Init_WithOutGenPrm("Sample", "");
            CmpVal_SampleName.ValueClickedEvent += CmpVal_EditWordEvent;

            CmpVal_ReagentName.Init_WithOutGenPrm("Reagent", "");
            CmpVal_ReagentName.ValueClickedEvent += CmpVal_EditWordEvent;

            CmpVal_ScaleFactor.Init_WithOutGenPrm("Scale Factor", (double)0);
            CmpVal_ScaleFactor.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_DisplayUnit.Init_WithOutGenPrm("Display Unit", "");
            CmpVal_DisplayUnit.ValueClickedEvent += CmpVal_EditWordEvent;


            CmpVal_MixingTime_Offset.Init_WithOutGenPrm("After Offset");
            CmpVal_MixingTime_Offset.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_MixingTime_General.Init_WithOutGenPrm("General");
            CmpVal_MixingTime_General.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpCol_Sensor.Init_WithOutGenPrm("Sensor", new string[] { "Sensor_1", "Sensor_2", "Sensor_3", "Sensor_4" }, "");
            CmpCol_Sensor.SelectedUserItemChangedEvent += CmpCol_SelectedUserItemChangedEvent;

            CmpVal_Analog_Target.Init_WithOutGenPrm("Target [mV]", (double)0);
            CmpVal_Analog_Target.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_Analog_End.Init_WithOutGenPrm("End [mV]", (double)0);
            CmpVal_Analog_End.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpCol_EnableInterpolation.Init_WithOutGenPrm("Enable Interpolation", new bool[] { false, true });
            CmpCol_EnableInterpolation.SelectedUserItemChangedEvent += CmpCol_SelectedUserItemChangedEvent;

            CmpCol_Syringe.Init_WithOutGenPrm("Syringe", new string[] { "Syringe_1", "Syringe_2" }, "");
            CmpCol_Syringe.SelectedUserItemChangedEvent += CmpCol_SelectedUserItemChangedEvent;

            CmpVal_MaxIteration.Init_WithOutGenPrm("Max Iteration", 0);
            CmpVal_MaxIteration.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_Offset.Init_WithOutGenPrm("Offset [mL]", (double)0);
            CmpVal_Offset.ValueClickedEvent += CmpVal_EditNumberEvent;


            CmpVal_Change_LargeToMiddle.Init_WithOutGenPrm("Large to Middle", (double)0);
            CmpVal_Change_LargeToMiddle.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_Change_MiddleToSmall.Init_WithOutGenPrm("Middle to Small", (double)0);
            CmpVal_Change_MiddleToSmall.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_Inj_Large.Init_WithOutGenPrm("Large", (double)0);
            CmpVal_Inj_Large.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_Inj_Middle.Init_WithOutGenPrm("Middle", (double)0);
            CmpVal_Inj_Middle.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_Inj_Small.Init_WithOutGenPrm("Small", (double)0);
            CmpVal_Inj_Small.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpCol_VLD_Enabled.Init_WithOutGenPrm("Enable Validation", new bool[] { false, true });
            CmpCol_VLD_Enabled.SelectedUserItemChangedEvent += CmpCol_SelectedUserItemChangedEvent;

            CmpVal_VLD_Ref.Init_WithOutGenPrm("Reference", (double)0);
            CmpVal_VLD_Ref.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_VLD_Low.Init_WithOutGenPrm("Low", (double)0);
            CmpVal_VLD_Low.ValueClickedEvent += CmpVal_EditNumberEvent;

            CmpVal_VLD_High.Init_WithOutGenPrm("High", (double)0);
            CmpVal_VLD_High.ValueClickedEvent += CmpVal_EditNumberEvent;
        }

        private void CmpVal_EditWordEvent(object sender, object oldValue)
        {
            PrmCmp_Value cmpVal = (PrmCmp_Value)sender;
            string sOld = (string)oldValue;
            string sOrg = string.Empty;
            bool bEnbSpecialKeys = cmpVal.Prm_Name == "Display Unit";
            Frm_StrKeyPad sKeyPad = new Frm_StrKeyPad(cmpVal.Prm_Name, sOld, bEnbSpecialKeys);
            if (sKeyPad.ShowDialog() == DialogResult.OK)
            {
                string sNew = sKeyPad.NewValue;
                
                switch (cmpVal.Prm_Name)
                {
                    case "Reference File":
                        string endWith = sNew.Substring(sNew.Length - 4, 4);
                        if (endWith.ToUpper() != ".XML")
                        {
                            sNew += ".xml";
                        }
                        RefStep.AnalyzeRefFileName = sNew;
                        sOrg = OrgStep_ToCompare.AnalyzeRefFileName;
                        break;

                    case "Sample":
                        Ref_TtrCfg.SampleName = sNew;
                        sOrg = Org_TtrCfg_ToCompare.SampleName;

                        RefStep.AnalyzeRefObj.SampleName = sNew;    // AnalyzeRef와 TitrationRef의 SampleName을 통일하기 위해 강제 할당한다.
                        break;

                    case "Reagent":
                        Ref_TtrCfg.InjectionInfo.ReagentName = sNew;
                        sOrg = Org_TtrCfg_ToCompare.InjectionInfo.ReagentName;
                        break;

                    case "Display Unit":
                        Ref_TtrCfg.ResultUnit = sNew;
                        sOrg = Org_TtrCfg_ToCompare.ResultUnit;
                        break;
                }
                cmpVal.Prm_Value = sNew;
                cmpVal.Color_Name = sNew == sOrg ? Color.LemonChiffon : Color.DarkOrange;
            }
        }

        private void CmpVal_EditNumberEvent(object sender, object oldValue)
        {
            PrmCmp_Value cmpVal = (PrmCmp_Value)sender;
            object objOrg = null;
            object objNew = null;
            Frm_NumPad nKeyPad = new Frm_NumPad(cmpVal.Prm_Name, GetConvertedObject(cmpVal));
            if (nKeyPad.ShowDialog() == DialogResult.OK)
            {
                objNew = nKeyPad.NewValue;
                switch (cmpVal.Prm_Name)
                {
                    case "Scale Factor":
                        Ref_TtrCfg.ScaleFactor = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.ScaleFactor;
                        break;

                    case "After Offset":
                        Ref_TtrCfg.MixingTime_AfterOffset = (int)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.MixingTime_AfterOffset;
                        break;

                    case "General":
                        Ref_TtrCfg.MixingTime_General = (int)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.MixingTime_General;
                        break;

                    case "Target [mV]":
                        Ref_TtrCfg.AnalogInfo.TargetValue = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.AnalogInfo.TargetValue;
                        break;

                    case "End [mV]":
                        Ref_TtrCfg.AnalogInfo.EndValue = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.AnalogInfo.EndValue;
                        break;

                    case "Max Iteration":
                        Ref_TtrCfg.MaxIterationCount = (int)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.MaxIterationCount;
                        break;

                    case "Offset [mL]":
                        Ref_TtrCfg.InjectionInfo.Offset = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.InjectionInfo.Offset;
                        break;

                    case "Large to Middle":
                        Ref_TtrCfg.InjectionInfo.IncThreshold_ChangeToMiddle = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.InjectionInfo.IncThreshold_ChangeToMiddle;
                        break;

                    case "Middle to Small":
                        Ref_TtrCfg.InjectionInfo.IncThreshold_ChangeToSmall = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.InjectionInfo.IncThreshold_ChangeToSmall;
                        break;

                    case "Large":
                        Ref_TtrCfg.InjectionInfo.Inc_Large = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.InjectionInfo.Inc_Large;
                        break;

                    case "Middle":
                        Ref_TtrCfg.InjectionInfo.Inc_Middle = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.InjectionInfo.Inc_Middle;
                        break;

                    case "Small":
                        Ref_TtrCfg.InjectionInfo.Inc_Small = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.InjectionInfo.Inc_Small;
                        break;

                    case "Reference":
                        Ref_TtrCfg.ValidationInfo.ReferenceValue = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.ValidationInfo.ReferenceValue;
                        break;

                    case "Low":
                        Ref_TtrCfg.ValidationInfo.Limit_Low = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.ValidationInfo.Limit_Low;
                        break;

                    case "High":
                        Ref_TtrCfg.ValidationInfo.Limit_High = (double)nKeyPad.NewValue;
                        objOrg = Org_TtrCfg_ToCompare.ValidationInfo.Limit_High;
                        break;
                }
                cmpVal.Prm_Value = nKeyPad.NewValue;
                cmpVal.Color_Name = objNew.Equals(objOrg) ? Color.LemonChiffon : Color.DarkOrange;
            }
        }

        private void CmpCol_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            PrmCmp_Collection cmpCol = (PrmCmp_Collection)sender;
            object objOrg = null;
            object objNew = changedValue;
            switch (cmpCol.Prm_Name)
            {
                case "Sensor":
                    Ref_TtrCfg.AnalogInfo.AnalogInputLogicalName = (string)changedValue;
                    objOrg = Org_TtrCfg_ToCompare.AnalogInfo.AnalogInputLogicalName;
                    break;

                case "Syringe":
                    Ref_TtrCfg.InjectionInfo.ReagentSyringeLogicalName = (string)changedValue;
                    objOrg = Org_TtrCfg_ToCompare.InjectionInfo.ReagentSyringeLogicalName;
                    break;

                case "Enable Interpolation":
                    Ref_TtrCfg.EnableInterpolation = (bool)changedValue;
                    objOrg = Org_TtrCfg_ToCompare.EnableInterpolation;
                    break;

                case "Enable Validation":
                    Ref_TtrCfg.ValidationInfo.Enabled = (bool)changedValue;
                    objOrg = Org_TtrCfg_ToCompare.ValidationInfo.Enabled;
                    break;
            }
            cmpCol.Color_Name = objNew.Equals(objOrg) ? Color.LemonChiffon : Color.DarkOrange;
        }

        private object GetConvertedObject(PrmCmp_Value cmpVal)
        {
            object valObj = null;
            switch (cmpVal.Prm_Name)
            {
                case "Scale Factor":
                case "Target [mV]":
                case "End [mV]":
                case "Offset [mL]":
                case "Large to Middle":
                case "Middle to Small":
                case "Large":
                case "Middle":
                case "Small":
                    valObj = Convert.ToDouble(cmpVal.Prm_Value);
                    break;

                case "After Offset":
                case "General":
                case "Max Iteration":
                    valObj = Convert.ToInt32(cmpVal.Prm_Value);
                    break;
            }
            return valObj;
        }

        public void Update_RefObject_ToCompare()
        {
            // Page가 보여지고 있는 상태에서 값을 바꾸고 저장한 뒤 다시 값을 바꾸는 경우,
            // Page를 보여줄때 복사해둔 저장하기 전의 객체와 비교하면서 잘못된 비교 결과를 얻는다.
            // 따라서 저장한 이후에 저장한 객체를 비교 원본에 다시 복사하여 갱신한다.
            OrgStep_ToCompare = (Step)RefStep.Clone();
        }

        private void UsrCtrl_Recipe_StepDetail_Titration_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                if (FluidicsControl.MainState == FluidicsState.Run)
                {
                    EnableControl(false);
                }
                else
                {
                    UserAuthorityIsChanged();
                }
            }
        }

        public void EnableControl(bool enb)
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(ctrl => ctrl.EnableModifying(true, enb));
        }

        public void UserAuthorityIsChanged()
        {
            EnableControl(GlbVar.CurrentAuthority == UserAuthority.Admin);
        }
    }
}
