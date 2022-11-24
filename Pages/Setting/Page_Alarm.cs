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
    public partial class Page_Alarm : UserControl, IPage, IParamSetting, IAuthority
    {
        public delegate void RequestAlarmTestModeDisableDelegate();
        public event RequestAlarmTestModeDisableDelegate RequestAlarmTestModeDisable;

        private Dictionary<int, Panel> SubPages = new Dictionary<int, Panel>();
        private List<AlarmObject> Alarms;

        public delegate void ShowPagingButtons(bool show);
        public event ShowPagingButtons ShowPagingButtonsEvent;

        public Page_Alarm()
        {
            InitializeComponent();
        }

        public void Init()
        {
            pnl_View.Controls.Clear();

            Alarms = LT_Alarm.GetAllAlarms();

            int height = 57;
            int height_Margin = 5;
            //if (pnl_View.Height != 0)
            //{
            //    height_Margin = 9;
            //}
            int eachHeight = height + height_Margin;
            int ctrlsPerPage = pnl_View.Height / eachHeight;
            int pages = Alarms.Count / ctrlsPerPage;
            ShowPagingButtonsEvent?.Invoke(pages > 1);

            int width_Margin = 5;
            int width = pnl_View.Width - (2 * width_Margin);

            if (Alarms.Count % ctrlsPerPage > 0)
            {
                ++pages;
            }

            for (int pageIdx = 0; pageIdx < pages; pageIdx++)
            {
                Panel pnl = new Panel();
                pnl.Width = pnl_View.Width;
                pnl.Height = pnl_View.Height;
                pnl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                pnl.Visible = false;

                int idx_Start = ctrlsPerPage * pageIdx;
                int putInCnt = 0;
                for (int ctrlIdx = idx_Start; ctrlIdx < idx_Start + ctrlsPerPage; ctrlIdx++)
                {
                    if (ctrlIdx == Alarms.Count)
                    {
                        break;
                    }
                    UsrCtrl_AlarmInfo ctrl = new UsrCtrl_AlarmInfo(Alarms[ctrlIdx]);
                    ctrl.Size = new Size(width, height);
                    ctrl.Location = new Point(width_Margin, height_Margin + (height + height_Margin) * putInCnt);
                    ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    pnl.Controls.Add(ctrl);
                    pnl.VisibleChanged += Pnl_VisibleChanged;

                    ++putInCnt;
                }

                SubPages.Add(pageIdx, pnl);
                pnl_View.Controls.Add(pnl);
            }

            if (SubPages.Count > 0)
            {
                SubPages[0].Visible = true;
            }
        }

        private void Pnl_VisibleChanged(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            if (pnl.Visible == true)
            {
                List<UsrCtrl_AlarmInfo> cmps = pnl.Controls.OfType<UsrCtrl_AlarmInfo>().ToList();
                cmps.ForEach(cmp =>
                {
                    cmp.ChangeLanguage();
                    EnableTestMode(false);
                });
            }
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void ShowSubPage(string subPageName)
        {
            throw new NotImplementedException();
        }

        public void PagingNext()
        {
        }

        public void PagingPrev()
        {
        }

        public void Restore()
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.Restore());
        }

        public void UpdateChangedStatus()
        {
            Handle_UI.GetAllControlsRecursive(this).OfType<UsrCtrl_AlarmInfo>().ToList().ForEach(ctrl =>
            {
                ctrl.UpdateChangedStatus();
            });
        }

        public void SaveAllParams(bool askSave)
        {
            Handle_UI.GetAllControlsRecursive(this).OfType<UsrCtrl_AlarmInfo>().ToList().ForEach(ctrl =>
            {
                ctrl.SaveAllParams(false);
            });
        }

        public bool IsParamChanged()
        {
            bool paramChanged = false;
            Handle_UI.GetAllControlsRecursive(this).OfType<UsrCtrl_AlarmInfo>().ToList().ForEach(ctrl =>
            {
                if (ctrl.IsParamChanged() == true)
                {
                    paramChanged = true;
                    return;
                }
            });
            return paramChanged;
        }

        public void EnableTestMode(bool enb)
        {
            Handle_UI.GetAllControlsRecursive(this).OfType<UsrCtrl_AlarmInfo>().ToList().ForEach(ctrl =>
            {
                ctrl.EnabledAlarmTestMode(enb);
            });
        }

        public void UserAuthorityIsChanged()
        {
            Handle_UI.GetAllControlsRecursive(this).OfType<UsrCtrl_AlarmInfo>().ToList().ForEach(ctrl =>
            {
                ctrl.EnableParameter(GlbVar.CurrentAuthority == UserAuthority.Admin);
            });
        }

        private void Page_Alarm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                UserAuthorityIsChanged();
            }
            else
            {
                RequestAlarmTestModeDisable?.Invoke();
            }
        }
    }
}
