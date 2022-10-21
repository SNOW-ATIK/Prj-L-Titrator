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
    public partial class UsrCtrl_Login : UserControl
    {
        private const string PW_ENGINEER = "999999";
        private const string PW_ADMIN = "123456";

        public delegate void UserAuthorityChanged(UserAuthority authority);
        public event UserAuthorityChanged UserAuthorityChangedEvent;

        public UsrCtrl_Login()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (GlbVar.CurrentAuthority == UserAuthority.User)
            {
                MsgFrm_PasswordIn msgFrm = new MsgFrm_PasswordIn("Password", true);
                if (msgFrm.ShowDialog() == DialogResult.OK)
                {
                    if (msgFrm.Password_KeyIn == PW_ADMIN)
                    {
                        GlbVar.CurrentAuthority = UserAuthority.Admin;

                        Log.WriteLog(LogSection.Main, "Log-In: Admin.", true);
                    }
                    else if (msgFrm.Password_KeyIn == PW_ENGINEER)
                    {
                        GlbVar.CurrentAuthority = UserAuthority.Engineer;

                        Log.WriteLog(LogSection.Main, "Log-In: Engineer.", true);
                    }
                    else
                    {
                        GlbVar.CurrentAuthority = UserAuthority.User;

                        Log.WriteLog(LogSection.Main, "Log-In: User.", true);
                    }

                    MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly($"Log-In: {GlbVar.CurrentAuthority}");
                    msg.Show(1000);
                }
            }
            else
            {
                MsgFrm_AskYesNo askLogOut = new MsgFrm_AskYesNo("Do you want to Log-Out?");
                if (askLogOut.ShowDialog() == DialogResult.Yes)
                {
                    GlbVar.CurrentAuthority = UserAuthority.User;

                    Log.WriteLog(LogSection.Main, "Log-Out: User.", true);
                }
            }

            SetAuthorityText();

            UserAuthorityChangedEvent?.Invoke(GlbVar.CurrentAuthority);
        }

        private void SetAuthorityText()
        {
            btn_Login.Text = GlbVar.CurrentAuthority.ToString().ToUpper();
            // TBD
            //switch (GlbVar.Authority)
            //{
            //    case UserAuthority.User:
            //        btn_Login.Text = SigmaLanguage.View_Top.Authority.Worker.ToUpper();
            //        break;

            //    case UserAuthority.Engineer:
            //        btn_Login.Text = SigmaLanguage.View_Top.Authority.Engineer.ToUpper();
            //        break;

            //    case UserAuthority.Admin:
            //        btn_Login.Text = SigmaLanguage.View_Top.Authority.Admin.ToUpper();
            //        break;
            //}
        }
    }
}
