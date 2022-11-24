using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ATIK;

namespace L_Titrator.Pages
{
    public partial class SubPage_Recipe_Process : UserControl, IAuthority, IPage
    {
        public class SeqOrderCmp
        {
            public delegate void SeqOrderClicked(int order);
            public event SeqOrderClicked SeqOrderClickedEvent;

            public int Order { get; private set; }
            public string Name { get; private set; }
            public bool Selected { get; private set; }
            public Button OrderBtn;
            public Label NameLbl;

            private RecipeObj RefRecipeObj;

            public SeqOrderCmp(RecipeObj rcpObj, int order, string name, int cmpHeight)
            {
                RefRecipeObj = rcpObj;
                Order = order;
                Name = name;

                OrderBtn = new Button();
                OrderBtn.Margin = new Padding(1, 1, 1, 1);
                OrderBtn.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;// | AnchorStyles.Bottom;
                OrderBtn.Height = cmpHeight;
                OrderBtn.FlatStyle = FlatStyle.Flat;
                OrderBtn.BackColor = Color.White;
                OrderBtn.Font = new Font("Consolas", 10f, FontStyle.Bold);
                OrderBtn.Text = Order.ToString();
                OrderBtn.Click += OrderBtn_Click;

                NameLbl = new Label();
                NameLbl.BorderStyle = BorderStyle.FixedSingle;
                NameLbl.Margin = new Padding(1, 1, 1, 1);
                NameLbl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;// | AnchorStyles.Bottom;
                NameLbl.Height = cmpHeight;
                NameLbl.FlatStyle = FlatStyle.Flat;
                NameLbl.BackColor = Color.White;
                NameLbl.Font = new Font("Consolas", 10f, FontStyle.Bold);
                NameLbl.TextAlign = ContentAlignment.MiddleLeft;
                NameLbl.Text = name;
                NameLbl.DoubleClick += NameLbl_DoubleClick;
            }

            private void NameLbl_DoubleClick(object sender, EventArgs e)
            {
                Frm_StrKeyPad keyPad = new Frm_StrKeyPad("Sequnce Name Edit", NameLbl.Text);
                if (keyPad.ShowDialog() == DialogResult.OK)
                {
                    string newName = keyPad.NewValue;
                    if (Order < RefRecipeObj.Sequences.Count)
                    {
                        var nameQuery = from seq in RefRecipeObj.Sequences
                                        select seq.Name;
                        if (nameQuery.Contains(newName) == true)
                        {
                            MsgFrm_NotifyOnly ntf = new MsgFrm_NotifyOnly($"{newName} is already existed.");
                            ntf.ShowDialog();
                            return;
                        }
                        RefRecipeObj.Sequences[Order].Name = newName;
                        NameLbl.Text = newName;
                    }
                }
            }

            private void OrderBtn_Click(object sender, EventArgs e)
            {
                Select(true);
                SeqOrderClickedEvent?.Invoke(Order);
            }

            public void Select(bool selected)
            {
                OrderBtn.BackColor = selected == true ? Color.MediumSeaGreen : Color.White;
                NameLbl.BackColor = selected == true ? Color.FromArgb(200, Color.MediumSeaGreen.G + 50, 200) : Color.White;
                Selected = selected;
            }

            public void Set_SeqOrder(int order)
            {
                Order = order;
                OrderBtn.Text = Order.ToString();
            }
        }

        private RecipeObj RefRecipeObj;
        private List<SeqOrderCmp> SeqOrderCmps = new List<SeqOrderCmp>();
        private int SelectedOrder
        {
            get
            {
                if (SeqOrderCmps.Count == 0)
                {
                    return -1;
                }
                var selected = SeqOrderCmps.Where(cmp => cmp.Selected == true).ToList();
                if (selected.Count == 0)
                {
                    return -1;
                }
                return selected[0].Order;

            }
        }

        public SubPage_Recipe_Process()
        {
            InitializeComponent();
        }

        public void Init_PreDefinedSeq()
        {
            LT_Recipe.Load_PreDefinedSeq();

            if (LT_Recipe.Get_PreDefSeqNames(out var seqNames) == true)
            {
                CmpCol_PreDefSeq.Init_WithOutGenPrm("Pre-Defined Seq.", seqNames.ToArray());
            }
        }

        public void SetWorkReference(RecipeObj refRcpObj)
        {
            // ★ RecipeObj는 복사(Clone)된 걸 받고, Seq와 Step은 복사된 RecipeObj 내의 것을 Ref로 넘겨서 작업한다.
            // (Seq, Step 작업 시 LT_Recipe 객체에 접근하지 않음. 단, PreDefinedSeq를 받을땐 LT_Recipe 객체에서 불러와서 복사(Clone)해서 받아온다.
            RefRecipeObj = refRcpObj;

            SeqOrderCmps.Clear();
            tbl_SeqOrder.Controls.Clear();

            if (RefRecipeObj.Sequences != null && RefRecipeObj.Sequences.Count > 0)
            {
                for (int i = 0; i < RefRecipeObj.Sequences.Count; i++)
                {
                    Sequence seq = RefRecipeObj.Sequences[i];

                    //Insert_SeqCmp(i, seq);    // Clear하고 다시 그리는 작업의 비효율을 막기 위해 할 수 없이 아래의 중복 코드 작성

                    SeqOrderCmp SeqCmp = new SeqOrderCmp(RefRecipeObj, seq.No, seq.Name, 35);
                    SeqCmp.SeqOrderClickedEvent += SeqCmp_SeqOrderClickedEvent;
                    SeqOrderCmps.Add(SeqCmp);
                    tbl_SeqOrder.RowStyles.Add(new RowStyle(SizeType.Absolute, 37));
                    tbl_SeqOrder.Controls.Add(SeqOrderCmps[i].OrderBtn, 0, i);
                    tbl_SeqOrder.Controls.Add(SeqOrderCmps[i].NameLbl, 1, i);
                }
                SeqOrderCmps[0].OrderBtn.PerformClick();
            }
            else
            {
                pnl_SeqInfo.Controls.Clear();
            }
        }

        private void SeqCmp_SeqOrderClickedEvent(int seqNo)
        {
            pnl_SeqInfo.Controls.Clear();

            UsrCtrl_Recipe_StepDetail ctrl = new UsrCtrl_Recipe_StepDetail();
            ctrl.Location = new Point(0, 0);
            ctrl.Size = pnl_SeqInfo.Size;
            ctrl.SetSequenceReference(RefRecipeObj, seqNo);

            pnl_SeqInfo.Controls.Add(ctrl);

            SeqOrderCmps.Except(new SeqOrderCmp[] { SeqOrderCmps[seqNo] }).ToList().ForEach(cmp => cmp.Select(false));
        }

        // Ctrl Insert or Remove
        private void Insert_SeqCmp(int idx, Sequence seq)
        {
            SeqOrderCmp SeqCmp = new SeqOrderCmp(RefRecipeObj, seq.No, seq.Name, 35);
            SeqCmp.SeqOrderClickedEvent += SeqCmp_SeqOrderClickedEvent;
            SeqOrderCmps.Insert(idx, SeqCmp);

            RefRecipeObj.Sequences.Insert(idx, seq);

            // Rediredct Seq. No
            // Clear 하고 다시 채워야 함. TableLayoutPanel에서 RowStyle.Insert 제대로 안 됨.
            tbl_SeqOrder.Controls.Clear();
            tbl_SeqOrder.RowStyles.Clear();

            for (int i = 0; i < SeqOrderCmps.Count; i++)
            {
                RefRecipeObj.Sequences[i].No = i;

                SeqOrderCmps[i].Set_SeqOrder(i);

                tbl_SeqOrder.RowStyles.Insert(i, new RowStyle(SizeType.Absolute, 37));
                tbl_SeqOrder.Controls.Add(SeqOrderCmps[i].OrderBtn, 0, i);
                tbl_SeqOrder.Controls.Add(SeqOrderCmps[i].NameLbl, 1, i);
            }
        }

        private void Remove_SeqCmp(int idx)
        {
            SeqOrderCmps[idx].SeqOrderClickedEvent -= SeqCmp_SeqOrderClickedEvent;
            SeqOrderCmps.RemoveAt(idx);

            RefRecipeObj.Sequences.RemoveAt(idx);

            // Clear 하고 다시 채워야 함. TableLayoutPanel에서 RowStyle.RemoveAt(idx) 해봤자, idx 뒤에 Control들이 당겨와지지 않음.
            tbl_SeqOrder.Controls.Clear();
            tbl_SeqOrder.RowStyles.Clear();

            // TBD. Rediredct Seq. No
            for (int i = 0; i < SeqOrderCmps.Count; i++)
            {
                RefRecipeObj.Sequences[i].No = i;

                SeqOrderCmps[i].Set_SeqOrder(i);

                tbl_SeqOrder.RowStyles.Insert(i, new RowStyle(SizeType.Absolute, 37));
                tbl_SeqOrder.Controls.Add(SeqOrderCmps[i].OrderBtn, 0, i);
                tbl_SeqOrder.Controls.Add(SeqOrderCmps[i].NameLbl, 1, i);
            }
        }

        public void ClearAll()
        {
            SeqOrderCmps.Clear();
            tbl_SeqOrder.Controls.Clear();
            tbl_SeqOrder.RowStyles.Clear();
            RefRecipeObj?.Sequences?.Clear();
            pnl_SeqInfo.Controls.OfType<UsrCtrl_Recipe_StepDetail>().ToList().ForEach(page => page.ClearAll());
        }

        public void UpdateNamePlates()
        {
            pnl_SeqInfo.Controls.OfType<UsrCtrl_Recipe_StepDetail>().ToList()?.ForEach(ctrl => ctrl.UpateNamePlates());
        }

        // Seq. Order Edit
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (CmpCol_PreDefSeq.Prm_Value == null)
            {
                return;
            }

            string seqName = (string)CmpCol_PreDefSeq.Prm_Value;
            if (RefRecipeObj.Sequences.Count == 0)
            {
                if (RefRecipeObj.No < LT_Recipe.RecipeMaxCount)
                {
                    if (seqName != "Start")
                    {
                        MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly("Add Start sequence first.");
                        msg.ShowDialog();

                        CmpCol_PreDefSeq.Prm_Value = "Start";

                        return;
                    }
                }
            }

            if (LT_Recipe.Get_CopiedPreDefSeq(seqName, out Sequence seq) == true)
            {
                Insert_SeqCmp(SeqOrderCmps.Count, seq);
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (CmpCol_PreDefSeq.Prm_Value == null)
            {
                return;
            }

            string seqName = (string)CmpCol_PreDefSeq.Prm_Value;
            if (LT_Recipe.Get_CopiedPreDefSeq(seqName, out Sequence seq) == true)
            {
                int insertIdx = SelectedOrder < 0? SeqOrderCmps.Count: SelectedOrder;
                if (insertIdx == 0 && CmpCol_PreDefSeq.Prm_Name != "Start")
                {
                    MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly("Unavailable to add the selected sequence in first.");
                    msg.ShowDialog();

                    return;
                }
                Insert_SeqCmp(insertIdx, seq);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (SelectedOrder < 0)
            {
                return;
            }
            if (SelectedOrder == 0)
            {
                MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly("Unavailable to delete the first sequence.");
                msg.ShowDialog();

                return;
            }

            Remove_SeqCmp(SelectedOrder);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        public void SetMargin(Padding margin)
        {
            throw new NotImplementedException();
        }

        public void SetDock(DockStyle dockStyle)
        {
            throw new NotImplementedException();
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
            if (visible == true)
            {
                UserAuthorityIsChanged();
            }
        }

        public void ShowSubPage(string subPageName)
        {
            throw new NotImplementedException();
        }

        public void PagingNext()
        {
            throw new NotImplementedException();
        }

        public void PagingPrev()
        {
            throw new NotImplementedException();
        }

        public void UserAuthorityIsChanged()
        {
            if (FluidicsControl.MainState == FluidicsState.Run)
            {
                CmpCol_PreDefSeq.EnableModifying(true, false);
                tbl_Edit.Enabled = false;
            }
            else
            {
                CmpCol_PreDefSeq.EnableModifying(true, GlbVar.CurrentAuthority == UserAuthority.Admin);
                tbl_Edit.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
            }
        }
    }
}
