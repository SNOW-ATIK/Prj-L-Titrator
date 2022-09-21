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
    public partial class UsrCtrl_Recipe_StepDetail_Control
    {
        private void Ctrl_BitStateChangedEventHandler(MB_Elem_Bit elem, UsrCtrl_Bit.BitState state)
        {
            if (RefStep == null)
            {
                return;
            }

            var tgtValveInStep = RefStep.Valves.Where(valve => valve.Name == elem.LogicalName).ToList();
            if (tgtValveInStep.Count == 1)
            {
                bool bState = state == UsrCtrl_Bit.BitState.On ? true : false;
                tgtValveInStep[0].Set_Condition(bState);
            }
            else
            {
                // #. Invalid
            }
        }

        private void Ctrl_SyringeConditionChangedEventHandler(MB_Elem_Syringe elem, MB_SyringeFlow flow, MB_SyringeDirection dir, int speed, double vol_mL)
        {
            if (RefStep == null)
            {
                return;
            }

            var tgtSyringeInStep = RefStep.Syringes.Where(syringe => syringe.Name == elem.LogicalName).ToList();
            if (tgtSyringeInStep.Count == 1)
            {
                tgtSyringeInStep[0].Condition = $"{flow},{dir},{speed},{vol_mL}";
            }
            else
            { 
            }
        }

        private void Ctrl_AnalogValueChangedEventHandler(MB_Elem_Analog elem, string sValue)
        {
            if (RefStep == null)
            {
                return;
            }

            var tgtMixerInStep = RefStep.Mixers.Where(mixer => mixer.Name == elem.LogicalName).ToList();
            if (tgtMixerInStep.Count == 1)
            {
                tgtMixerInStep[0].Condition = sValue;
            }
            else
            {
            }
        }

        private void StepEndCheckInThisStep_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            RefStep.EndCheckInThisStep = (bool)changedValue;
        }

        private void StepEnd_EnabledChangedEvent(object sender, object changedValue)
        {
            PrmCmp_Collection cmpCol = (PrmCmp_Collection)sender;
            bool bEnb = (bool)changedValue;
            if (cmpCol == CmpCol_StepEndEnabled_TimeDelay)
            {
                int time_sec = 0;
                if (bEnb == true)
                {
                    if (RefStep.StepEndCheck.TimeDelay.Time == 0)
                    {
                        time_sec = 1;
                    }
                    else
                    {
                        time_sec = RefStep.StepEndCheck.TimeDelay.Time;
                    }
                }

                CmpVal_StepEndValue_TimeDelay.Prm_Value = time_sec;

                RefStep.StepEndCheck.TimeDelay.Enabled = bEnb;
                RefStep.StepEndCheck.TimeDelay.Time = time_sec;
            }
            else if (cmpCol == CmpCol_StepEndEnabled_Sensor)
            {
                if (bEnb == true)
                {
                    if (CmpCol_StepEndValues_Sensor.Prm_Value == null)
                    {
                        if (string.IsNullOrEmpty(RefStep.StepEndCheck.SensorDetect.SensorNames) == false)
                        {
                            CmpCol_StepEndValues_Sensor.Prm_Value = RefStep.StepEndCheck.SensorDetect.SensorNames;
                        }
                        else
                        {
                            CmpCol_StepEndValues_Sensor.Prm_Value = CmpCol_StepEndValues_Sensor.Get_Collection()?[0];
                        }
                    }
                }
                else
                {
                    CmpCol_StepEndValues_Sensor.Prm_Value = null;
                }

                RefStep.StepEndCheck.SensorDetect.Enabled = bEnb;
                RefStep.StepEndCheck.SensorDetect.SensorNames = (string)CmpCol_StepEndValues_Sensor.Prm_Value;
            }
            else if (cmpCol == CmpCol_StepEndEnabled_Syringe)
            {
                RefStep.StepEndCheck.PositionSync.Enabled = bEnb;
            }
        }

        private void StepEnd_TimeDelay_ValueChangedEvent(object sender, object oldValue, object newValue)
        {
            RefStep.StepEndCheck.TimeDelay.Time = Convert.ToInt32(newValue);
        }

        private void StepEnd_Sensor_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            RefStep.StepEndCheck.SensorDetect.SensorNames = (string)changedValue;
        }

        private void UsrCtrl_Recipe_StepDetail_Control_Resize(object sender, EventArgs e)
        {
            pnl_Solenoids.Controls.OfType<UserControl>().ToList().ForEach(ctrl => ctrl.Width = pnl_Solenoids.Width - 2);
            pnl_Mixer.Controls.OfType<UserControl>().ToList().ForEach(ctrl => ctrl.Width = pnl_Mixer.Width - 2);
            pnl_StepEndInfo.Controls.OfType<UserControl>().ToList().ForEach(ctrl => ctrl.Width = pnl_StepEndInfo.Width - 2);
        }
    }
}
