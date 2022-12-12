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
    public partial class UsrCtrl_Recipe_StepDetail_ISE : UserControl, IAuthority
    {
        private AnalyzeRef Ref_AnalyzeCfg;
        private AnalyzeRef Org_AnalyzeCfg_ToCompare;
        private Step RefStep;
        private Step OrgStep_ToCompare;

        public UsrCtrl_Recipe_StepDetail_ISE()
        {
            InitializeComponent();
        }

        public void Parse(Step stepRef)
        {
            // TBD
            //RefStep = stepRef;
            //OrgStep_ToCompare = (Step)RefStep.Clone();

            //Ref_AnalyzeCfg = RefStep.TitrationRef;
            //Org_AnalyzeCfg_ToCompare = (TitrationRef)Ref_AnalyzeCfg.Clone();

            //if (Ref_AnalyzeCfg != null)
            //{
            //    CmpVal_RefFile.Prm_Value = stepRef.TitrationRefFileName;
            //    CmpVal_SampleName.Prm_Value = Ref_AnalyzeCfg.SampleName;
            //    CmpVal_ReagentName.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.ReagentName;
            //    CmpVal_ScaleFactor.Prm_Value = Ref_AnalyzeCfg.ScaleFactor;
            //    CmpVal_DisplayUnit.Prm_Value = Ref_AnalyzeCfg.ResultUnit;

            //    CmpVal_MixingTime_Offset.Prm_Value = Ref_AnalyzeCfg.MixingTime_AfterOffset;
            //    CmpVal_MixingTime_General.Prm_Value = Ref_AnalyzeCfg.MixingTime_General;

            //    CmpCol_Sensor.Prm_Value = Ref_AnalyzeCfg.AnalogInfo.AnalogInputLogicalName;
            //    CmpVal_Analog_Target.Prm_Value = Ref_AnalyzeCfg.AnalogInfo.TargetValue;
            //    CmpVal_Analog_End.Prm_Value = Ref_AnalyzeCfg.AnalogInfo.EndValue;

            //    CmpCol_Syringe.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.ReagentSyringeLogicalName;
            //    CmpVal_MaxIteration.Prm_Value = Ref_AnalyzeCfg.MaxIterationCount;
            //    CmpVal_Offset.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.Offset;
            //    CmpVal_Change_LargeToMiddle.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.IncThreshold_ChangeToMiddle;
            //    CmpVal_Change_MiddleToSmall.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.IncThreshold_ChangeToSmall;
            //    CmpVal_Inj_Large.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.Inc_Large;
            //    CmpVal_Inj_Middle.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.Inc_Middle;
            //    CmpVal_Inj_Small.Prm_Value = Ref_AnalyzeCfg.InjectionInfo.Inc_Small;
            //    CmpCol_EnableInterpolation.Prm_Value = Ref_AnalyzeCfg.EnableInterpolation;

            //    CmpCol_VLD_Enabled.Prm_Value = Ref_AnalyzeCfg.ValidationInfo.Enabled;
            //    CmpVal_VLD_Ref.Prm_Value = Ref_AnalyzeCfg.ValidationInfo.ReferenceValue;
            //    CmpVal_VLD_Low.Prm_Value = Ref_AnalyzeCfg.ValidationInfo.Limit_Low;
            //    CmpVal_VLD_High.Prm_Value = Ref_AnalyzeCfg.ValidationInfo.Limit_High;
            //}
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
            // TBD
            //PrmCmp_Value cmpVal = (PrmCmp_Value)sender;
            //string sOld = (string)oldValue;
            //string sOrg = string.Empty;
            //string sNew = sOld;
            //bool bEnbSpecialKeys = cmpVal.Prm_Name == "Display Unit";
            //Frm_StrKeyPad sKeyPad = new Frm_StrKeyPad(cmpVal.Prm_Name, sOld, bEnbSpecialKeys);
            //if (sKeyPad.ShowDialog() == DialogResult.OK)
            //{
            //    sNew = sKeyPad.NewValue;
            //    switch (cmpVal.Prm_Name)
            //    {
            //        case "Reference File":
            //            RefStep.TitrationRefFileName = sKeyPad.NewValue;
            //            sOrg = OrgStep_ToCompare.TitrationRefFileName;
            //            break;

            //        case "Sample":
            //            Ref_AnalyzeCfg.SampleName = sKeyPad.NewValue;
            //            sOrg = Org_AnalyzeCfg_ToCompare.SampleName;
            //            break;

            //        case "Reagent":
            //            Ref_AnalyzeCfg.InjectionInfo.ReagentName = sKeyPad.NewValue;
            //            sOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.ReagentName;
            //            break;

            //        case "Display Unit":
            //            Ref_AnalyzeCfg.ResultUnit = sKeyPad.NewValue;
            //            sOrg = Org_AnalyzeCfg_ToCompare.ResultUnit;
            //            break;
            //    }
            //    cmpVal.Prm_Value = sKeyPad.NewValue;
            //    cmpVal.Color_Name = sNew == sOrg ? Color.LemonChiffon : Color.DarkOrange;
            //}
        }

        private void CmpVal_EditNumberEvent(object sender, object oldValue)
        {
            // TBD
            //PrmCmp_Value cmpVal = (PrmCmp_Value)sender;
            //object objOrg = null;
            //object objNew = null;
            //Frm_NumPad nKeyPad = new Frm_NumPad(cmpVal.Prm_Name, GetConvertedObject(cmpVal));
            //if (nKeyPad.ShowDialog() == DialogResult.OK)
            //{
            //    objNew = nKeyPad.NewValue;
            //    switch (cmpVal.Prm_Name)
            //    {
            //        case "Scale Factor":
            //            Ref_AnalyzeCfg.ScaleFactor = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.ScaleFactor;
            //            break;

            //        case "After Offset":
            //            Ref_AnalyzeCfg.MixingTime_AfterOffset = (int)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.MixingTime_AfterOffset;
            //            break;

            //        case "General":
            //            Ref_AnalyzeCfg.MixingTime_General = (int)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.MixingTime_General;
            //            break;

            //        case "Target [mV]":
            //            Ref_AnalyzeCfg.AnalogInfo.TargetValue = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.AnalogInfo.TargetValue;
            //            break;

            //        case "End [mV]":
            //            Ref_AnalyzeCfg.AnalogInfo.EndValue = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.AnalogInfo.EndValue;
            //            break;

            //        case "Max Iteration":
            //            Ref_AnalyzeCfg.MaxIterationCount = (int)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.MaxIterationCount;
            //            break;

            //        case "Offset [mL]":
            //            Ref_AnalyzeCfg.InjectionInfo.Offset = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.Offset;
            //            break;

            //        case "Large to Middle":
            //            Ref_AnalyzeCfg.InjectionInfo.IncThreshold_ChangeToMiddle = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.IncThreshold_ChangeToMiddle;
            //            break;

            //        case "Middle to Small":
            //            Ref_AnalyzeCfg.InjectionInfo.IncThreshold_ChangeToSmall = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.IncThreshold_ChangeToSmall;
            //            break;

            //        case "Large":
            //            Ref_AnalyzeCfg.InjectionInfo.Inc_Large = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.Inc_Large;
            //            break;

            //        case "Middle":
            //            Ref_AnalyzeCfg.InjectionInfo.Inc_Middle = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.Inc_Middle;
            //            break;

            //        case "Small":
            //            Ref_AnalyzeCfg.InjectionInfo.Inc_Small = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.Inc_Small;
            //            break;

            //        case "Reference":
            //            Ref_AnalyzeCfg.ValidationInfo.ReferenceValue = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.ValidationInfo.ReferenceValue;
            //            break;

            //        case "Low":
            //            Ref_AnalyzeCfg.ValidationInfo.Limit_Low = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.ValidationInfo.Limit_Low;
            //            break;

            //        case "High":
            //            Ref_AnalyzeCfg.ValidationInfo.Limit_High = (double)nKeyPad.NewValue;
            //            objOrg = Org_AnalyzeCfg_ToCompare.ValidationInfo.Limit_High;
            //            break;
            //    }
            //    cmpVal.Prm_Value = nKeyPad.NewValue;
            //    cmpVal.Color_Name = objNew.Equals(objOrg) ? Color.LemonChiffon : Color.DarkOrange;
            //}
        }

        private void CmpCol_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            // TBD
            //PrmCmp_Collection cmpCol = (PrmCmp_Collection)sender;
            //object objOrg = null;
            //object objNew = changedValue;
            //switch (cmpCol.Prm_Name)
            //{
            //    case "Sensor":
            //        Ref_AnalyzeCfg.AnalogInfo.AnalogInputLogicalName = (string)changedValue;
            //        objOrg = Org_AnalyzeCfg_ToCompare.AnalogInfo.AnalogInputLogicalName;
            //        break;

            //    case "Syringe":
            //        Ref_AnalyzeCfg.InjectionInfo.ReagentSyringeLogicalName = (string)changedValue;
            //        objOrg = Org_AnalyzeCfg_ToCompare.InjectionInfo.ReagentSyringeLogicalName;
            //        break;

            //    case "Enable Interpolation":
            //        Ref_AnalyzeCfg.EnableInterpolation = (bool)changedValue;
            //        objOrg = Org_AnalyzeCfg_ToCompare.EnableInterpolation;
            //        break;

            //    case "Enable Validation":
            //        Ref_AnalyzeCfg.ValidationInfo.Enabled = (bool)changedValue;
            //        objOrg = Org_AnalyzeCfg_ToCompare.ValidationInfo.Enabled;
            //        break;
            //}
            //cmpCol.Color_Name = objNew.Equals(objOrg) ? Color.LemonChiffon : Color.DarkOrange;
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
