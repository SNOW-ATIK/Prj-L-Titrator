using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L_Titrator.Controls
{
    public partial class UsrCtrl_MeasureResult : UserControl
    {
        public UsrCtrl_MeasureResult()
        {
            InitializeComponent();
        }

        public void Init_Info(string tgtName, string unit)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Init_Info(tgtName, unit)));
            }
            lbl_MeasureTarget.Text = tgtName;
            lbl_Unit.Text = unit;
            lbl_Result.Text = "";
            lbl_Analog_mV.Text = "";
            lbl_Output_mA.Text = "";
            lbl_MeasureTarget.BackColor = Color.DeepSkyBlue;
        }

        public void Set_Result(double concentration, double analog_mV, double output_mA)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Set_Result(concentration, analog_mV, output_mA)));
            }
            lbl_Result.Text = concentration.ToString("0.000#");
            lbl_Analog_mV.Text = analog_mV.ToString();
            lbl_Output_mA.Text = output_mA.ToString();
        }

        public void Clear()
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(Clear));
            }
            lbl_MeasureTarget.Text = "";
            lbl_Unit.Text = "";
            lbl_Result.Text = "";
            lbl_Analog_mV.Text = "";
            lbl_Output_mA.Text = "";
            lbl_MeasureTarget.BackColor = Color.DarkGray;
        }
    }
}
