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
using L_Titrator_Alpha.Controls;

namespace L_Titrator_Alpha.Pages
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
    }
}
