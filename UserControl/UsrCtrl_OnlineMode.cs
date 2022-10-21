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

namespace L_Titrator.Controls
{
    public partial class UsrCtrl_OnlineMode : UserControl
    {
        public delegate void OnlineModeChanged(OnlineMode onlineMode);
        public event OnlineModeChanged OnlineModeChangedEvent;

        public UsrCtrl_OnlineMode()
        {
            InitializeComponent();
        }

        public void EnableButton(bool enb)
        {
            btn_OnlineMode.Enabled = enb;
            if (enb == true)
            {
                btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;
            }
            else
            {
                btn_OnlineMode.BackColor = Color.DarkGray;
            }
        }

        private void btn_OnlineMode_Click(object sender, EventArgs e)
        {
            if (GlbVar.CurrentAuthority == UserAuthority.User)
            {
                MsgFrm_NotifyOnly msgNtf = new MsgFrm_NotifyOnly("Log In First.");
                msgNtf.ShowDialog();

                return;
            }

            OnlineMode tgtMode = GlbVar.OnlineMode == OnlineMode.Remote ? OnlineMode.Local : OnlineMode.Remote;
            MsgFrm_AskYesNo msgAsk = new MsgFrm_AskYesNo($"Do you want to change OnlineMode to {tgtMode.ToString().ToUpper()}?");
            if (msgAsk.ShowDialog() == DialogResult.Yes)
            {
                GlbVar.OnlineMode = tgtMode;

                btn_OnlineMode.Text = GlbVar.OnlineMode.ToString().ToUpper();
                btn_OnlineMode.BackColor = GlbVar.OnlineMode == OnlineMode.Remote ? Color.MediumSeaGreen : Color.Crimson;

                OnlineModeChangedEvent?.Invoke(GlbVar.OnlineMode);
            }
        }
    }
}
