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
using ATIK.Device;
using ATIK.Device.ATIK_MainBoard;

namespace L_Titrator
{
    public partial class Frm_SyringeControl : Form
    {

        private Point StartLocation;
        private MB_Elem_Syringe ElemSyringe;
        public Syringe_Command ApplySyringeState = new Syringe_Command();

        public Frm_SyringeControl()
        {
            InitializeComponent();
        }

        public Frm_SyringeControl(MB_Elem_Syringe elemSyringe, Point startLocation)
        {
            InitializeComponent();

            ElemSyringe = elemSyringe;
            StartLocation = startLocation;
            lbl_SyringeName.Text = ElemSyringe.LogicalName;
            CmpSyringe.SetElem(ElemSyringe, false);
            CmpSyringe.ShowCurrentVolume = true;
            CmpSyringe.UpdateState();
            CmpSyringe.SyringeConditionChangedEvent += CmpSyringe_SyringeConditionChangedEvent;
        }

        private void CmpSyringe_SyringeConditionChangedEvent(MB_Elem_Syringe elem, MB_SyringeFlow flow, MB_SyringeDirection dir, int speed, double vol_mL)
        {
            ApplySyringeState.Flow = flow;
            ApplySyringeState.Direction = dir;
            ApplySyringeState.Speed = speed;
            ApplySyringeState.Volume_mL = vol_mL;
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            // TBD. Check Validity


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Frm_SyringeControl_Load(object sender, EventArgs e)
        {
            this.Location = StartLocation;
        }
    }
}
