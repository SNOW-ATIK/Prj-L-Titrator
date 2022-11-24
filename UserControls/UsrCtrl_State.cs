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
    public partial class UsrCtrl_State : UserControl
    {
        public UsrCtrl_State()
        {
            InitializeComponent();
        }

        public void Set_State(MainState state)
        {
            switch (state)
            {
                case MainState.Unknown:
                    lbl_State.BackColor = Color.DarkGray;
                    break;

                case MainState.Idle:
                    lbl_State.BackColor = Color.White;
                    break;

                case MainState.Run:
                    lbl_State.BackColor = Color.MediumSeaGreen;
                    break;

                case MainState.Warning:
                    lbl_State.BackColor = Color.DarkOrange;
                    break;

                case MainState.Error:
                    lbl_State.BackColor = Color.Crimson;
                    break;
            }

            GlbVar.CurrentMainState = state;

            lbl_State.Text = state.ToString();
        }
    }
}
