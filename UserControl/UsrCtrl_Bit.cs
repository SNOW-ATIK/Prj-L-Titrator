using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator_Alpha.Controls
{
    public partial class UsrCtrl_Bit : UserControl
    {
        public enum BitState
        { 
            Unknown,
            Off,
            On,
        }

        private MB_Elem_Bit MyElem;
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
        private bool CmpForControl = true;
        private BitState StateForSetting = BitState.Off;
        private BitState StateMerged = BitState.Off;

        public delegate void BitStateChanged(MB_Elem_Bit elem, BitState state);
        public event BitStateChanged BitStateChangedEvent;

        public UsrCtrl_Bit()
        {
            InitializeComponent();
        }

        public UsrCtrl_Bit(MB_Elem_Bit elem, bool cmpForControl = true)
        {
            InitializeComponent();

            MyElem = elem;
            CmpForControl = cmpForControl;

            lbl_Name.Text = MyElem.LogicalName.Replace("_", " ");
            btn_State.Enabled = MyElem.IOType == MB_IOType.Output;

            if (MyElem.LogicalName.ToUpper() == "RESERVED")
            {
                lbl_Name.BackColor = Color.LightGray;
                btn_State.BackColor = Color.LightGray;
            }
            else
            {
                if (CmpForControl == true)
                {
                    UpdateState();
                }
            }
        }

        public void Set_SplitterDistance(int width)
        {
            tableLayoutPanel1.ColumnStyles[1].Width = width;
        }

        public void UpdateState()
        {
            if (CmpForControl == true)
            {
                bool getState = MyElem.Get_State();
                btn_State.BackColor = getState == true ? Color.MediumSeaGreen : Color.Crimson;
                btn_State.Text = getState == true ? "ON" : "OFF";
            }
        }

        public void Set_State(BitState state)
        {
            if (CmpForControl == true)
            {
                throw new InvalidOperationException();
            }

            StateForSetting = state;
            switch (StateForSetting)
            {
                case BitState.Unknown:
                    btn_State.BackColor = Color.DarkGray;
                    btn_State.Text = "-";
                    break;

                case BitState.Off:
                    btn_State.BackColor = Color.Crimson;
                    btn_State.Text = "OFF";
                    break;

                case BitState.On:
                    btn_State.BackColor = Color.MediumSeaGreen;
                    btn_State.Text = "ON";
                    break;
            }
        }

        public void Set_State(bool state)
        {
            if (CmpForControl == true)
            {
                throw new InvalidOperationException();
            }

            Set_State(state == true ? BitState.On : BitState.Off);
        }

        public void Set_MergedState(BitState state)
        {
            if (CmpForControl == true)
            {
                throw new InvalidOperationException();
            }

            StateMerged = state;
            switch (StateMerged)
            {
                case BitState.Unknown:
                    lbl_Name.BackColor = Color.White;
                    break;

                case BitState.Off:
                    lbl_Name.BackColor = PreDef.Light_Red;
                    break;

                case BitState.On:
                    lbl_Name.BackColor = PreDef.Light_Green;
                    break;
            }
        }

        private void btn_State_Click(object sender, EventArgs e)
        {
            if (CmpForControl == true)
            {
                bool getState = MyElem.Get_State();
                bool setState = !(getState);
                MyElem.Set_State(setState);
            }
            else
            {
                BitState targetState = BitState.Unknown;
                if (StateForSetting == BitState.Off)
                {
                    targetState = BitState.On;
                }
                else if (StateForSetting == BitState.On)
                {
                    targetState = BitState.Unknown;
                }
                else if (StateForSetting == BitState.Unknown)
                {
                    targetState = BitState.Off;
                }

                //BitState targetState;
                //if (StateForSetting == BitState.Off)
                //{
                //    targetState = BitState.On;
                //}
                //else
                //{
                //    targetState = BitState.Off;
                //}

                Set_State(targetState);

                BitStateChangedEvent?.Invoke(MyElem, targetState);
            }
        }
    }
}
