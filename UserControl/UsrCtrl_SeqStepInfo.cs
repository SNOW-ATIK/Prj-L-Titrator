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
    public partial class UsrCtrl_SeqStepInfo : UserControl
    {
        public UsrCtrl_SeqStepInfo()
        {
            InitializeComponent();
        }

        public void Set_Sequence(string seqName)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Set_Sequence(seqName)));
                return;
            }
            lbl_Sequence.Text = seqName;
        }

        public void Set_Step(string stepName)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Set_Step(stepName)));
                return;
            }
            lbl_Step.Text = stepName;
        }
    }
}
