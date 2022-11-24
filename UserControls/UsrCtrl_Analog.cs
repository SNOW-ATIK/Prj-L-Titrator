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

namespace L_Titrator.Controls
{
    public partial class UsrCtrl_Analog : UserControl, IAuthority
    {
        private MB_Elem_Analog MyElem;
        public string LogicalName
        {
            get
            {
                if (MyElem != null)
                {
                    return MyElem.LogicalName;
                }
                return "";
            }
        }
        private bool UserInputEnabled = false;

        private bool CmpForControl = true;
        private bool ShowState = false;
        private double ValueForSetting = 0;

        public delegate void AnalogValueChanged(MB_Elem_Analog elem, string sValue);
        public AnalogValueChanged AnalogValueChangedEvent;

        public UsrCtrl_Analog()
        {
            InitializeComponent();
        }

        public UsrCtrl_Analog(MB_Elem_Analog elem, bool cmpForControl = true, bool showState = false)
        {
            InitializeComponent();

            MyElem = elem;
            CmpForControl = cmpForControl;
            ShowState = showState;

            lbl_Name.Text = MyElem.LogicalName;
            UserInputEnabled = MyElem.IOType == MB_IOType.Output;
            lbl_Unit.Text = MyElem.Unit;

            if (MyElem.LogicalName.ToUpper() == "RESERVED")
            {
                lbl_Name.BackColor = Color.LightGray;
                lbl_Value.BackColor = Color.LightGray;
                lbl_Unit.BackColor = Color.LightGray;
            }
            else
            {
                UpdateState();
            }

            if (ShowState == true)
            {
                tableLayoutPanel2.ColumnStyles[0].Width = (tableLayoutPanel2.Width - lbl_Unit.Width - 2) / 2;
                Set_MergedState((int)-1);
            }
            else
            {
                tableLayoutPanel2.ColumnStyles[0].Width = 0;
            }
        }

        public void UpdateState()
        {
            double getValue = MyElem.Get_Value();
            lbl_Value.Text = getValue.ToString();
        }

        public void Set_Unknown()
        {
            if (CmpForControl == true)
            {
                throw new InvalidOperationException();
            }
            lbl_Value.Text = "-";
            lbl_Value.BackColor = Color.DarkGray;
        }

        public void Set_State(object value)
        {
            if (CmpForControl == true)
            {
                throw new InvalidOperationException();
            }

            ValueForSetting = Convert.ToDouble(value);
            if (value.GetType() == typeof(int))
            {
                lbl_Value.Text = ((int)value).ToString();
            }
            else
            {
                lbl_Value.Text = ((double)value).ToString("0.00");
            }

            if (ValueForSetting > 0)
            {
                lbl_Value.BackColor = Color.MediumSeaGreen;
            }
            else
            {
                lbl_Value.BackColor = Color.Crimson;
            }
        }

        public void Set_MergedState(object mergedVal)
        {
            if (ShowState == true)
            {
                if (mergedVal.GetType() == typeof(int))
                {
                    lbl_State.Text = ((int)mergedVal).ToString();
                }
                else
                {
                    lbl_State.Text = ((double)mergedVal).ToString("0.00");
                }

                double value = Convert.ToDouble(mergedVal);
                if (value == 0)
                {
                    lbl_State.BackColor = PreDef.Light_Red;
                }
                else if (value < 0)
                {
                    lbl_State.BackColor = Color.DarkGray;
                    lbl_State.Text = "-";
                }
                else
                {
                    lbl_State.BackColor = PreDef.Light_Green;
                }
            }
        }

        public void EnableUserInput(bool enb)
        {
            UserInputEnabled = enb;
        }

        private void lbl_Value_Click(object sender, EventArgs e)
        {
            if (UserInputEnabled == true)
            {
                if (double.TryParse(lbl_Value.Text, out double dOld) == false)
                {
                    dOld = 0;
                }

                Frm_NumPad numPad = new Frm_NumPad(MyElem.LogicalName, dOld);
                if (numPad.ShowDialog() == DialogResult.OK)
                {
                    double dNew = (double)numPad.NewValue;
                    if (dNew % 1 == 0)
                    {
                        lbl_Value.Text = dNew.ToString("0");
                    }
                    else
                    {
                        lbl_Value.Text = dNew.ToString("0.00");
                    }

                    if (CmpForControl == true)
                    {
                        MyElem.Set_Value((double)numPad.NewValue);
                    }
                    else
                    {
                        ValueForSetting = dNew;
                        if (ValueForSetting > 0)
                        {
                            lbl_Value.BackColor = Color.MediumSeaGreen;
                        }
                        else
                        {
                            lbl_Value.BackColor = Color.Crimson;
                        }

                        AnalogValueChangedEvent?.Invoke(MyElem, lbl_Value.Text);
                    }
                }
            }
        }

        private void UsrCtrl_Analog_SizeChanged(object sender, EventArgs e)
        {
            if (ShowState == true)
            {
                tableLayoutPanel2.ColumnStyles[0].Width = (tableLayoutPanel2.Width - lbl_Unit.Width - 2 ) / 2;
            }
            else
            {
                tableLayoutPanel2.ColumnStyles[0].Width = 0;
            }
        }

        public void EnableControl(bool enb)
        {
            lbl_Name.Enabled = enb;
        }

        public void UserAuthorityIsChanged()
        {
            EnableControl(GlbVar.CurrentAuthority == UserAuthority.Admin);
        }
    }
}
