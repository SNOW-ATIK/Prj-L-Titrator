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

namespace L_Titrator
{
    public partial class Frm_ManualStart : Form
    {
        private int _RecipeNo = -1;
        public int RecipeNo { get => _RecipeNo; }

        private int _IterationCounts = 1;
        public int IterationCounts { get => _IterationCounts; }

        private Dictionary<int, (Button RcpNoBtn, Label RcpNameLabel)> DicRecipes = new Dictionary<int, (Button, Label)>();

        public Frm_ManualStart()
        {
            InitializeComponent();

            CmpVal_Iteration.Init_WithOutGenPrm("Interation Counts", (int)1);

            DicRecipes.Add(0, (button1, label1));
            DicRecipes.Add(1, (button2, label2));
            DicRecipes.Add(2, (button3, label3));
            DicRecipes.Add(3, (button4, label4));
            DicRecipes.Add(4, (button5, label5));
            DicRecipes.Add(5, (button6, label6));
            DicRecipes.Add(6, (button7, label7));
            DicRecipes.Add(7, (button8, label8));
            DicRecipes.Add(8, (button9, label9));
            DicRecipes.Add(9, (button10, label10));

            for (int i = 0; i < LT_Recipe.RecipeMaxCount; i++)
            {
                DicRecipes[i].RcpNoBtn.Tag = i;

                if (LT_Recipe.Get_RecipeObj(i, out var rcpObj) == true)
                {
                    DicRecipes[i].RcpNoBtn.Enabled = true;
                    DicRecipes[i].RcpNameLabel.Enabled = true;

                    DicRecipes[i].RcpNameLabel.Text = rcpObj.Name;

                }
                else
                {
                    DicRecipes[i].RcpNoBtn.Enabled = false;
                    DicRecipes[i].RcpNameLabel.Enabled = false;
                }
            }
        }

        private void Frm_ManualStart_Load(object sender, EventArgs e)
        {
            CmpVal_Iteration.Enabled = LT_Config.GenPrm_Validation_Enabled.Value == false;
        }

        private void Control_EnabledChanged(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            if (sender.GetType() == typeof(Button))
            {
                ctrl.BackColor = ctrl.Enabled == true ? Color.White : Color.Gray;
            }
            else if (sender.GetType() == typeof(Label))
            {
                ctrl.BackColor = ctrl.Enabled == true ? Color.White : Color.LightGray;
            }
        }

        private void RecieNo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            _RecipeNo = (int)btn.Tag;

            for (int i = 0; i < LT_Recipe.RecipeMaxCount; i++)
            {
                if (i == _RecipeNo)
                {
                    DicRecipes[i].RcpNoBtn.BackColor = Color.MediumSeaGreen;
                    DicRecipes[i].RcpNameLabel.BackColor = Color.FromArgb(200, Color.MediumSeaGreen.G + 50, 200);
                }
                else
                {
                    if (DicRecipes[i].RcpNoBtn.Enabled == true)
                    {
                        DicRecipes[i].RcpNoBtn.BackColor = Color.White;
                        DicRecipes[i].RcpNameLabel.BackColor = Color.White;
                    }
                }                
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (_RecipeNo < 0)
            {
                MsgFrm_NotifyOnly msgFrm = new MsgFrm_NotifyOnly("Select the Recipe to start.");
                msgFrm.ShowDialog();
                return;
            }

            _IterationCounts = int.Parse((string)CmpVal_Iteration.Prm_Value);
            if (_IterationCounts < 1)
            {
                MsgFrm_NotifyOnly msgFrm = new MsgFrm_NotifyOnly("Invalid Iteration Counts.");
                msgFrm.ShowDialog();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CmpVal_Iteration_EnabledChanged(object sender, EventArgs e)
        {
            CmpVal_Iteration.Color_Name = CmpVal_Iteration.Enabled == true ? Color.LemonChiffon : Color.Gray;
            CmpVal_Iteration.Color_Value = CmpVal_Iteration.Enabled == true ? Color.White : Color.LightGray;
        }
    }
}
