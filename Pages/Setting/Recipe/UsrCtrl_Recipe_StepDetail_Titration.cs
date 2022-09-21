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
    public partial class UsrCtrl_Recipe_StepDetail_Titration : UserControl
    {
        private TitrationRef RefTitration;
        private Step RefStep;

        public UsrCtrl_Recipe_StepDetail_Titration()
        {
            InitializeComponent();
        }

        public void Parse(Step stepRef)
        {
            RefStep = stepRef;
            RefTitration = RefStep.TitrationRef;
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
            }
        }

        public void SetBackground()
        {
            CmpVal_RefFile.Init_WithOutGenPrm("Reference File", "");
            CmpVal_RefFile.ValueClickedEvent += CmpVal_EditWordEvent;
            //CmpVal_RefFile.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_SampleName.Init_WithOutGenPrm("Sample", "");
            CmpVal_SampleName.ValueClickedEvent += CmpVal_EditWordEvent;
            //CmpVal_SampleName.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_ReagentName.Init_WithOutGenPrm("Reagent", "");
            CmpVal_ReagentName.ValueClickedEvent += CmpVal_EditWordEvent;
            //CmpVal_ReagentName.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_ScaleFactor.Init_WithOutGenPrm("Scale Factor", (double)0);
            CmpVal_ScaleFactor.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_ScaleFactor.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_DisplayUnit.Init_WithOutGenPrm("Display Unit", "");
            CmpVal_DisplayUnit.ValueClickedEvent += CmpVal_EditWordEvent;
            //CmpVal_DisplayUnit.ValueChangedEvent += CmpVal_ValueChangedEvent;


            CmpVal_MixingTime_Offset.Init_WithOutGenPrm("After Offset");
            CmpVal_MixingTime_Offset.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_MixingTime_Offset.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_MixingTime_General.Init_WithOutGenPrm("General");
            CmpVal_MixingTime_General.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_MixingTime_General.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpCol_Sensor.Init_WithOutGenPrm("Sensor", new string[] { "Sensor_1", "Sensor_2", "Sensor_3", "Sensor_4" }, "");
            CmpCol_Sensor.SelectedUserItemChangedEvent += CmpCol_SelectedUserItemChangedEvent;

            CmpVal_Analog_Target.Init_WithOutGenPrm("Target [mV]", (double)0);
            CmpVal_Analog_Target.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Analog_Target.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_Analog_End.Init_WithOutGenPrm("End [mV]", (double)0);
            CmpVal_Analog_End.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Analog_End.ValueChangedEvent += CmpVal_ValueChangedEvent;


            CmpCol_Syringe.Init_WithOutGenPrm("Syringe", new string[] { "Syringe_1", "Syringe_2" }, "");
            CmpCol_Syringe.SelectedUserItemChangedEvent += CmpCol_SelectedUserItemChangedEvent;

            CmpVal_MaxIteration.Init_WithOutGenPrm("Max Iteration", 0);
            CmpVal_MaxIteration.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_MaxIteration.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_Offset.Init_WithOutGenPrm("Offset [mL]", (double)0);
            CmpVal_Offset.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Offset.ValueChangedEvent += CmpVal_ValueChangedEvent;


            CmpVal_Change_LargeToMiddle.Init_WithOutGenPrm("Large to Middle", (double)0);
            CmpVal_Change_LargeToMiddle.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Change_LargeToMiddle.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_Change_MiddleToSmall.Init_WithOutGenPrm("Middle to Small", (double)0);
            CmpVal_Change_MiddleToSmall.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Change_MiddleToSmall.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_Inj_Large.Init_WithOutGenPrm("Large", (double)0);
            CmpVal_Inj_Large.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Inj_Large.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_Inj_Middle.Init_WithOutGenPrm("Middle", (double)0);
            CmpVal_Inj_Middle.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Inj_Middle.ValueChangedEvent += CmpVal_ValueChangedEvent;

            CmpVal_Inj_Small.Init_WithOutGenPrm("Small", (double)0);
            CmpVal_Inj_Small.ValueClickedEvent += CmpVal_EditNumberEvent;
            //CmpVal_Inj_Small.ValueChangedEvent += CmpVal_ValueChangedEvent;
        }

        private void CmpVal_EditWordEvent(object sender, object oldValue)
        {
            PrmCmp_Value cmpVal = (PrmCmp_Value)sender;
            string sOld = (string)oldValue;
            bool bEnbSpecialKeys = cmpVal.Prm_Name == "Display Unit";
            Frm_StrKeyPad sKeyPad = new Frm_StrKeyPad(cmpVal.Prm_Name, sOld, bEnbSpecialKeys);
            if (sKeyPad.ShowDialog() == DialogResult.OK)
            {
                switch (cmpVal.Prm_Name)
                {
                    case "Reference File":
                        RefStep.TitrationRefFileName = sKeyPad.NewValue;
                        break;

                    case "Sample":
                        RefTitration.SampleName = sKeyPad.NewValue;
                        break;

                    case "Reagent":
                        RefTitration.InjectionInfo.ReagentName = sKeyPad.NewValue;
                        break;

                    case "Display Unit":
                        RefTitration.ResultUnit = sKeyPad.NewValue;
                        break;
                }
                cmpVal.Prm_Value = sKeyPad.NewValue;
            }
        }

        private void CmpVal_EditNumberEvent(object sender, object oldValue)
        {
            PrmCmp_Value cmpVal = (PrmCmp_Value)sender;
            Frm_NumPad nKeyPad = new Frm_NumPad(cmpVal.Prm_Name, GetConvertedObject(cmpVal));
            if (nKeyPad.ShowDialog() == DialogResult.OK)
            {
                switch (cmpVal.Prm_Name)
                {
                    case "Scale Factor":
                        RefTitration.ScaleFactor = (double)nKeyPad.NewValue;
                        break;

                    case "After Offset":
                        RefTitration.MixingTime_AfterOffset = (int)nKeyPad.NewValue;
                        break;

                    case "General":
                        RefTitration.MixingTime_General = (int)nKeyPad.NewValue;
                        break;

                    case "Target [mV]":
                        RefTitration.AnalogInfo.TargetValue = (double)nKeyPad.NewValue;
                        break;

                    case "End [mV]":
                        RefTitration.AnalogInfo.EndValue = (double)nKeyPad.NewValue;
                        break;

                    case "Max Iteration":
                        RefTitration.MaxIterationCount = (int)nKeyPad.NewValue;
                        break;

                    case "Offset [mL]":
                        RefTitration.InjectionInfo.Offset = (double)nKeyPad.NewValue;
                        break;

                    case "Large to Middle":
                        RefTitration.InjectionInfo.IncThreshold_ChangeToMiddle = (double)nKeyPad.NewValue;
                        break;

                    case "Middle to Small":
                        RefTitration.InjectionInfo.IncThreshold_ChangeToSmall = (double)nKeyPad.NewValue;
                        break;

                    case "Large":
                        RefTitration.InjectionInfo.Inc_Large = (double)nKeyPad.NewValue;
                        break;

                    case "Middle":
                        RefTitration.InjectionInfo.Inc_Middle = (double)nKeyPad.NewValue;
                        break;

                    case "Small":
                        RefTitration.InjectionInfo.Inc_Small = (double)nKeyPad.NewValue;
                        break;
                }
                cmpVal.Prm_Value = nKeyPad.NewValue;
            }
        }

        private void CmpCol_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            PrmCmp_Collection cmpCol = (PrmCmp_Collection)sender;
            switch (cmpCol.Prm_Name)
            {
                case "Sensor":
                    RefTitration.AnalogInfo.AnalogInputLogicalName = (string)changedValue;
                    break;

                case "Syringe":
                    RefTitration.InjectionInfo.ReagentSyringeLogicalName = (string)changedValue;
                    break;
            }
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
            }
            else
            { 
            }
        }
    }
}
