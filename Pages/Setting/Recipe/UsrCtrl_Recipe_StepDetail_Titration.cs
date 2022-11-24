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
        private TitrationRef RefTitration;
        private TitrationRef OrgTitration_ToCompare;
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

            RefTitration = RefStep.TitrationRef;
            OrgTitration_ToCompare = (TitrationRef)RefTitration.Clone();

            if (RefTitration != null)
            {
                CmpVal_RefFile.Prm_Value = stepRef.TitrationRefFileName;
                CmpVal_SampleName.Prm_Value = RefTitration.SampleName;
                CmpVal_ReagentName.Prm_Value = RefTitration.InjectionInfo.ReagentName;
                CmpVal_ScaleFactor.Prm_Value = RefTitration.ScaleFactor;
                CmpVal_DisplayUnit.Prm_Value = RefTitration.ResultUnit;

                CmpVal_MixingTime_Offset.Prm_Value = RefTitration.MixingTime_AfterOffset;
                CmpVal_MixingTime_General.Prm_Value = RefTitration.MixingTime_General;

                CmpCol_Sensor.Prm_Value = RefTitration.AnalogInfo.AnalogInputLogicalName;
                CmpVal_Analog_Target.Prm_Value = RefTitration.AnalogInfo.TargetValue;
                CmpVal_Analog_End.Prm_Value = RefTitration.AnalogInfo.EndValue;

                CmpCol_Syringe.Prm_Value = RefTitration.InjectionInfo.ReagentSyringeLogicalName;
                CmpVal_MaxIteration.Prm_Value = RefTitration.MaxIterationCount;
                CmpVal_Offset.Prm_Value = RefTitration.InjectionInfo.Offset;
                CmpVal_Change_LargeToMiddle.Prm_Value = RefTitration.InjectionInfo.IncThreshold_ChangeToMiddle;
                CmpVal_Change_MiddleToSmall.Prm_Value = RefTitration.InjectionInfo.IncThreshold_ChangeToSmall;
                CmpVal_Inj_Large.Prm_Value = RefTitration.InjectionInfo.Inc_Large;
                CmpVal_Inj_Middle.Prm_Value = RefTitration.InjectionInfo.Inc_Middle;
                CmpVal_Inj_Small.Prm_Value = RefTitration.InjectionInfo.Inc_Small;
                CmpCol_EnableInterpolation.Prm_Value = RefTitration.EnableInterpolation;

                CmpCol_VLD_Enabled.Prm_Value = RefTitration.ValidationInfo.Enabled;
                CmpVal_VLD_Ref.Prm_Value = RefTitration.ValidationInfo.ReferenceValue;
                CmpVal_VLD_Low.Prm_Value = RefTitration.ValidationInfo.Limit_Low;
                CmpVal_VLD_High.Prm_Value = RefTitration.ValidationInfo.Limit_High;
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
            string sNew = sOld;
            bool bEnbSpecialKeys = cmpVal.Prm_Name == "Display Unit";
            Frm_StrKeyPad sKeyPad = new Frm_StrKeyPad(cmpVal.Prm_Name, sOld, bEnbSpecialKeys);
            if (sKeyPad.ShowDialog() == DialogResult.OK)
            {
                sNew = sKeyPad.NewValue;
                switch (cmpVal.Prm_Name)
                {
                    case "Reference File":
                        RefStep.TitrationRefFileName = sKeyPad.NewValue;
                        sOrg = OrgStep_ToCompare.TitrationRefFileName;
                        break;

                    case "Sample":
                        RefTitration.SampleName = sKeyPad.NewValue;
                        sOrg = OrgTitration_ToCompare.SampleName;
                        break;

                    case "Reagent":
                        RefTitration.InjectionInfo.ReagentName = sKeyPad.NewValue;
                        sOrg = OrgTitration_ToCompare.InjectionInfo.ReagentName;
                        break;

                    case "Display Unit":
                        RefTitration.ResultUnit = sKeyPad.NewValue;
                        sOrg = OrgTitration_ToCompare.ResultUnit;
                        break;
                }
                cmpVal.Prm_Value = sKeyPad.NewValue;
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
                        RefTitration.ScaleFactor = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.ScaleFactor;
                        break;

                    case "After Offset":
                        RefTitration.MixingTime_AfterOffset = (int)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.MixingTime_AfterOffset;
                        break;

                    case "General":
                        RefTitration.MixingTime_General = (int)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.MixingTime_General;
                        break;

                    case "Target [mV]":
                        RefTitration.AnalogInfo.TargetValue = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.AnalogInfo.TargetValue;
                        break;

                    case "End [mV]":
                        RefTitration.AnalogInfo.EndValue = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.AnalogInfo.EndValue;
                        break;

                    case "Max Iteration":
                        RefTitration.MaxIterationCount = (int)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.MaxIterationCount;
                        break;

                    case "Offset [mL]":
                        RefTitration.InjectionInfo.Offset = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.InjectionInfo.Offset;
                        break;

                    case "Large to Middle":
                        RefTitration.InjectionInfo.IncThreshold_ChangeToMiddle = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.InjectionInfo.IncThreshold_ChangeToMiddle;
                        break;

                    case "Middle to Small":
                        RefTitration.InjectionInfo.IncThreshold_ChangeToSmall = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.InjectionInfo.IncThreshold_ChangeToSmall;
                        break;

                    case "Large":
                        RefTitration.InjectionInfo.Inc_Large = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.InjectionInfo.Inc_Large;
                        break;

                    case "Middle":
                        RefTitration.InjectionInfo.Inc_Middle = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.InjectionInfo.Inc_Middle;
                        break;

                    case "Small":
                        RefTitration.InjectionInfo.Inc_Small = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.InjectionInfo.Inc_Small;
                        break;

                    case "Reference":
                        RefTitration.ValidationInfo.ReferenceValue = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.ValidationInfo.ReferenceValue;
                        break;

                    case "Low":
                        RefTitration.ValidationInfo.Limit_Low = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.ValidationInfo.Limit_Low;
                        break;

                    case "High":
                        RefTitration.ValidationInfo.Limit_High = (double)nKeyPad.NewValue;
                        objOrg = OrgTitration_ToCompare.ValidationInfo.Limit_High;
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
                    RefTitration.AnalogInfo.AnalogInputLogicalName = (string)changedValue;
                    objOrg = OrgTitration_ToCompare.AnalogInfo.AnalogInputLogicalName;
                    break;

                case "Syringe":
                    RefTitration.InjectionInfo.ReagentSyringeLogicalName = (string)changedValue;
                    objOrg = OrgTitration_ToCompare.InjectionInfo.ReagentSyringeLogicalName;
                    break;

                case "Enable Interpolation":
                    RefTitration.EnableInterpolation = (bool)changedValue;
                    objOrg = OrgTitration_ToCompare.EnableInterpolation;
                    break;

                case "Enable Validation":
                    RefTitration.ValidationInfo.Enabled = (bool)changedValue;
                    objOrg = OrgTitration_ToCompare.ValidationInfo.Enabled;
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
