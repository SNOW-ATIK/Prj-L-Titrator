using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK.Common.ComponentEtc.Fluidics;

namespace L_Titrator
{
    public partial class Frm_ValveControl : Form
    {
        private Point StartLocation;
        private object ValveCmp;
        public Dictionary<Valve_PortDirection, Valve_State> ApplyPortState = new Dictionary<Valve_PortDirection, Valve_State>();

        public Frm_ValveControl()
        {
            InitializeComponent();
        }

        //public Frm_ValveControl(string valveName, Valve_Category valveCat, object valveCfg, Valve_Port cmnPort = Valve_Port.None)
        //{
        //    InitializeComponent();

        //    Init(valveName, valveCat, valveCfg, cmnPort);
        //}

        public Frm_ValveControl(object cmpValve, Point startLoc)
        {
            InitializeComponent();

            ValveCmp = cmpValve;
            StartLocation = startLoc;

            if (ValveCmp.GetType() == typeof(Cmp_Valve_2Way_Bmp))
            {
                Cmp_Valve_2Way_Bmp valve2Way = (Cmp_Valve_2Way_Bmp)cmpValve;
                Init(valve2Way.Valve_Name, valve2Way.Valve_Category, valve2Way.Valve_Config, valve2Way.Valve_Common_Port);

                SetCurrentState(valve2Way.Port_State);
            }
            else if (ValveCmp.GetType() == typeof(Cmp_Valve_3Way_Bmp))
            {
                Cmp_Valve_3Way_Bmp valve3Way = (Cmp_Valve_3Way_Bmp)cmpValve;
                Init(valve3Way.Valve_Name, valve3Way.Valve_Category, valve3Way.Valve_Config, valve3Way.Valve_Common_Port);

                SetCurrentState(valve3Way.Port_State);
            }
            else if (ValveCmp.GetType() == typeof(Cmp_Valve_6Way_Bmp))
            {
                Cmp_Valve_6Way_Bmp valve6Way = (Cmp_Valve_6Way_Bmp)cmpValve;
                Init(valve6Way.Valve_Name, valve6Way.Valve_Category, valve6Way.Valve_Config, valve6Way.Valve_Common_Port);

                SetCurrentState(valve6Way.Port_State);
            }
        }

        private void Init(string valveName, Valve_Category valveCat, object valveCfg, Valve_PortDirection cmnPort = Valve_PortDirection.None)
        {

            lbl_ValveName.Text = valveName;

            switch (valveCat)
            {
                case Valve_Category.TwoWay:
                    ApplyPortState.Add(Valve_PortDirection.Bottom, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Left, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Right, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Top, Valve_State.Close);

                    DisableNotUsePort_2Way((Valve_2Way_Cfg)valveCfg);
                    break;

                case Valve_Category.ThreeWay:
                    ApplyPortState.Add(Valve_PortDirection.Bottom, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Left, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Right, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Top, Valve_State.Close);

                    DisableNotUsePort_3Way((Valve_3Way_Cfg)valveCfg);
                    DisableCommonPort(cmnPort);
                    break;

                case Valve_Category.SixWay:
                    ApplyPortState.Add(Valve_PortDirection.Link_12, Valve_State.Close);
                    ApplyPortState.Add(Valve_PortDirection.Link_23, Valve_State.Close);

                    tbl_Left.Enabled = false;
                    tbl_Left.Visible = false;
                    tbl_Right.Enabled = false;
                    tbl_Right.Visible = false;

                    lbl_Top.Text = "Transfer";
                    lbl_Bottom.Text = "Capture";
                    break;
            }
        }

        private void DisableNotUsePort_2Way(Valve_2Way_Cfg cfg2way)
        {
            switch (cfg2way)
            {
                case Valve_2Way_Cfg.TopBottom:
                    ApplyPortState[Valve_PortDirection.Left] = Valve_State.NotExist;
                    ApplyPortState[Valve_PortDirection.Right] = Valve_State.NotExist;
                    tbl_Left.Enabled = false;
                    tbl_Right.Enabled = false;
                    break;

                case Valve_2Way_Cfg.LeftRight:
                    ApplyPortState[Valve_PortDirection.Top] = Valve_State.NotExist;
                    ApplyPortState[Valve_PortDirection.Bottom] = Valve_State.NotExist;
                    tbl_Top.Enabled = false;
                    tbl_Bottom.Enabled = false;
                    break;
            }
        }

        private void DisableNotUsePort_3Way(Valve_3Way_Cfg cfg3Way)
        {
            switch (cfg3Way)
            {
                case Valve_3Way_Cfg.BottomLeftTop:
                    ApplyPortState[Valve_PortDirection.Right] = Valve_State.NotExist;
                    tbl_Right.Enabled = false;
                    break;

                case Valve_3Way_Cfg.LeftTopRight:
                    ApplyPortState[Valve_PortDirection.Bottom] = Valve_State.NotExist;
                    tbl_Bottom.Enabled = false;
                    break;

                case Valve_3Way_Cfg.RightBottomLeft:
                    ApplyPortState[Valve_PortDirection.Top] = Valve_State.NotExist;
                    tbl_Top.Enabled = false;
                    break;

                case Valve_3Way_Cfg.TopRightBottom:
                    ApplyPortState[Valve_PortDirection.Left] = Valve_State.NotExist;
                    tbl_Left.Enabled = false;
                    break;
            }
        }

        private void DisableCommonPort(Valve_PortDirection cmnPort)
        {
            ApplyPortState[cmnPort] = Valve_State.Common;
            switch (cmnPort)
            {
                case Valve_PortDirection.Top:
                    tbl_Top.Enabled = false;
                    break;

                case Valve_PortDirection.Bottom:
                    tbl_Bottom.Enabled = false;
                    break;

                case Valve_PortDirection.Left:
                    tbl_Left.Enabled = false;
                    break;

                case Valve_PortDirection.Right:
                    tbl_Right.Enabled = false;
                    break;
            }
        }

        private void SetCurrentState(Dictionary<Valve_PortDirection, Valve_State> portState)
        {
            if (tbl_Top.Enabled == true)
            {
                if (ValveCmp.GetType() == typeof(Cmp_Valve_6Way_Bmp))
                {
                    if (portState[Valve_PortDirection.Link_12] == Valve_State.Open)
                    {
                        chk_Top_Open.Checked = true;
                    }
                    else
                    {
                        chk_Top_Close.Checked = true;
                    }
                }
                else
                {
                    if (portState[Valve_PortDirection.Top] == Valve_State.Open)
                    {
                        chk_Top_Open.Checked = true;
                    }
                    else
                    {
                        chk_Top_Close.Checked = true;
                    }
                }
            }
            if (tbl_Bottom.Enabled == true)
            {
                if (ValveCmp.GetType() == typeof(Cmp_Valve_6Way_Bmp))
                {
                    if (portState[Valve_PortDirection.Link_23] == Valve_State.Open)
                    {
                        chk_Bottom_Open.Checked = true;
                    }
                    else
                    {
                        chk_Bottom_Close.Checked = true;
                    }
                }
                else
                {
                    if (portState[Valve_PortDirection.Bottom] == Valve_State.Open)
                    {
                        chk_Bottom_Open.Checked = true;
                    }
                    else
                    {
                        chk_Bottom_Close.Checked = true;
                    }
                }
            }
            if (tbl_Left.Enabled == true)
            {
                if (portState[Valve_PortDirection.Left] == Valve_State.Open)
                {
                    chk_Left_Open.Checked = true;
                }
                else
                {
                    chk_Left_Close.Checked = true;
                }
            }
            if (tbl_Right.Enabled == true)
            {
                if (portState[Valve_PortDirection.Right] == Valve_State.Open)
                {
                    chk_Right_Open.Checked = true;
                }
                else
                {
                    chk_Right_Close.Checked = true;
                }
            }
        }

        private void Labels_EnabledChanged(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = lbl.Enabled == true ? Color.LemonChiffon : Color.DarkGray;
        }

        private void ControlCheckBoxes_EnabledChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            chk.BackColor = chk.Enabled == true ? Color.White : Color.DarkGray;
        }

        private void chk_Open_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            TableLayoutPanel tbl = (TableLayoutPanel)chk.Parent;
            var pair = tbl.Controls.OfType<CheckBox>().Except(new CheckBox[] { chk }).ToList()[0];
            pair.Checked = !chk.Checked;
        }

        private void chk_Close_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            TableLayoutPanel tbl = (TableLayoutPanel)chk.Parent;
            var pair = tbl.Controls.OfType<CheckBox>().Except(new CheckBox[] { chk }).ToList()[0];
            pair.Checked = !chk.Checked;
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            if (ValveCmp.GetType() == typeof(Cmp_Valve_6Way_Bmp))
            {
                ApplyPortState[Valve_PortDirection.Link_12] = chk_Top_Open.Checked == true ? Valve_State.Open : Valve_State.Close;
                ApplyPortState[Valve_PortDirection.Link_23] = chk_Bottom_Open.Checked == true ? Valve_State.Open : Valve_State.Close;
            }
            else
            {
                if (tbl_Top.Enabled == true)
                {
                    ApplyPortState[Valve_PortDirection.Top] = chk_Top_Open.Checked == true ? Valve_State.Open : Valve_State.Close;
                }
                if (tbl_Bottom.Enabled == true)
                {
                    ApplyPortState[Valve_PortDirection.Bottom] = chk_Bottom_Open.Checked == true ? Valve_State.Open : Valve_State.Close;
                }
                if (tbl_Left.Enabled == true)
                {
                    ApplyPortState[Valve_PortDirection.Left] = chk_Left_Open.Checked == true ? Valve_State.Open : Valve_State.Close;
                }
                if (tbl_Right.Enabled == true)
                {
                    ApplyPortState[Valve_PortDirection.Right] = chk_Right_Open.Checked == true ? Valve_State.Open : Valve_State.Close;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Frm_ValveControl_Load(object sender, EventArgs e)
        {
            this.Location = StartLocation;
        }
    }
}
