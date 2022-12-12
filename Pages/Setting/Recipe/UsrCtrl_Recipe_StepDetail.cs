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
using L_Titrator.Controls;

namespace L_Titrator.Pages
{
    public partial class UsrCtrl_Recipe_StepDetail : UserControl, IAuthority
    {
        private enum Edit
        { 
            Add,
            Insert,    
            Delete,
            Clear
        }

        private RecipeObj RefRecipe;
        private Sequence RefSequence;
        private int SelectedStepNo = -1;
        private int ShownFirstStepNo = -1;

        private List<Button> StepBtnList = new List<Button>();

        //private UsrCtrl_Recipe_StepDetail_Control usrCtrl_Control;
        //private UsrCtrl_Recipe_StepDetail_Titration usrCtrl_Titration;

        public UsrCtrl_Recipe_StepDetail()
        {
            InitializeComponent();

            SetBackground();
        }

        public void SetSequenceReference(RecipeObj rcpObj, int seqNo)
        {
            ClearAll();

            RefRecipe = rcpObj;
            if (RefRecipe == null)
            {
                return;
            }

            RefSequence = rcpObj.Get_Sequence(seqNo);
            if (RefSequence == null)
            {
                return;
            }

            // Set Steps
            for (int i = 0; i < RefSequence.Steps.Count; i++)
            {
                Step step = RefSequence.Steps[i];
                Button btn = CreateStepBtn(step.No);
                StepBtnList.Add(btn);
            }
            if (StepBtnList.Count > 0)
            {
                SetStepContainer(0);
            }
        }

        private void StepBtn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SelectedStepNo = (int)btn.Tag;
            btn.BackColor = Color.MediumSeaGreen;
            var except = StepBtnList.Except(new Button[] { btn }).ToList();
            except.ForEach(unselected => unselected.BackColor = Color.White);
            lbl_StepIdxInfo.Text = $"{SelectedStepNo + 1}/{StepBtnList.Count}";

            Step stepRef = RefSequence.Get_Step(SelectedStepNo);
            CmpVal_StepName.Prm_Value = stepRef.Name;
            CmpCol_StepEnabled.Prm_Value = stepRef.Enabled;
            CmpCol_IsTitration.Prm_Value = stepRef.IsAnalyzeStep;
            if (stepRef.IsAnalyzeStep == true)
            {
                // Show Titration Page
                usrCtrl_Control.Visible = false;
                switch (stepRef.AnalyzeRefObj.Type)
                {
                    case AnalyzeType.pH:
                    case AnalyzeType.ORP:
                        // TBD. usrCtrl_ISE.Visible = false;
                        usrCtrl_Titration.Visible = true;

                        tableLayoutPanel2.ColumnStyles[0].Width = 0;
                        tableLayoutPanel2.ColumnStyles[1].Width = 100;
                        tableLayoutPanel2.ColumnStyles[2].Width = 0;

                        usrCtrl_Titration.Parse(stepRef);
                        break;

                    case AnalyzeType.ISE:
                        // TBD. usrCtrl_ISE.Visible = true;
                        usrCtrl_Titration.Visible = false;

                        tableLayoutPanel2.ColumnStyles[0].Width = 0;
                        tableLayoutPanel2.ColumnStyles[1].Width = 0;
                        tableLayoutPanel2.ColumnStyles[2].Width = 100;

                        // TBD. usrCtrl_ISE.Parse(stepRef);
                        break;
                }

                // Disable Add or Remove buttons
                btn_Add.Enabled = false;
                btn_Insert.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Clear.Enabled = false;

                // Disable Reset button
                btn_ResetToPrvStep.Enabled = false;
            }
            else
            {
                //Step merged = LT_Recipe.Get_StateOfJustBefore(RecipeRef.No, SequenceRef.No, SelectedStepNo);
                Step merged = Get_StateOfJustBefore(RefSequence.No, SelectedStepNo);
                usrCtrl_Control.SetMergedInfo(merged);

                // Show Control Page
                usrCtrl_Control.Visible = true;
                usrCtrl_Titration.Visible = false;
                // TBD. usrCtrl_ISE.Visible = false;
                tableLayoutPanel2.ColumnStyles[0].Width = 100;
                tableLayoutPanel2.ColumnStyles[1].Width = 0;
                tableLayoutPanel2.ColumnStyles[2].Width = 0;

                usrCtrl_Control.Parse(stepRef);

                // Enable Add or Remove buttons
                btn_Add.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
                btn_Insert.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
                btn_Delete.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
                btn_Clear.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;

                // Enable or Disable Reset button
                if (RefSequence.No == 0 && SelectedStepNo == 0)
                {
                    btn_ResetToPrvStep.Enabled = false;
                }
                else
                {
                    btn_ResetToPrvStep.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
                }
            }
        }

        public Step Get_LastStateOfSequence(int seqNo)
        {
            // Create Step
            Step LastState = new Step();
            LastState.Valves = new List<Valve>();
            LastState.Mixers = new List<Mixer>();

            // Init. Base State
            if (RefRecipe.Sequences.Count > 0)
            {
                var BaseSeq = RefRecipe.Get_Sequence(0);
                if (BaseSeq.Steps.Count > 0)
                {
                    LastState = (Step)BaseSeq.Get_Step(0).Clone();
                    for (int i = 1; i < BaseSeq.Steps.Count; i++)
                    {
                        Step step = BaseSeq.Get_Step(i);

                        MergeStates(step, LastState);
                    }
                }

                if (seqNo == 0)
                {
                    return LastState;
                }

                for (int i = 1; i <= seqNo; i++)
                {
                    var SeqObj = RefRecipe.Get_Sequence(i);
                    for (int j = 0; j < SeqObj.Steps.Count; j++)
                    {
                        Step step = SeqObj.Get_Step(j);
                        if (step.IsAnalyzeStep == true)
                        {
                            break;  // Skip Titration condition
                        }

                        MergeStates(step, LastState);
                    }
                }
            }

            return LastState;
        }

        public Step Get_StateOfJustBefore(int seqNo, int stepNoCur)
        {
            Step LastState = new Step();
            if (seqNo == 0)
            {
                if (RefRecipe.Sequences.Count > 0)
                {
                    var BaseSeq = RefRecipe.Get_Sequence(0);
                    if (BaseSeq.Steps.Count > 0)
                    {
                        LastState = (Step)BaseSeq.Get_Step(0).Clone();
                        for (int i = 1; i < stepNoCur; i++)
                        {
                            Step step = BaseSeq.Get_Step(i);

                            MergeStates(step, LastState);
                        }
                    }
                }
            }
            else
            {
                LastState = Get_LastStateOfSequence(seqNo - 1);

                var curSeq = RefRecipe.Get_Sequence(seqNo);

                for (int i = 0; i < stepNoCur; i++)
                {
                    Step step = curSeq.Get_Step(i);

                    MergeStates(step, LastState);
                }
            }

            return LastState;
        }

        private void MergeStates(Step curStep, Step lastStep)
        {
            // Valves
            if (curStep.Control_Valve == true)
            {
                curStep.Valves.ForEach(ctrlValve =>
                {
                    for (int i = 0; i < lastStep.Valves.Count; i++)
                    {
                        var tgtValve = lastStep.Valves[i];
                        if (tgtValve.Name == ctrlValve.Name)
                        {
                            if (tgtValve.Get_Condition() != ctrlValve.Get_Condition())
                            {
                                tgtValve.Set_Condition(ctrlValve.Get_Condition());
                            }
                        }
                    }
                });
            }

            // Mixers
            if (curStep.Control_Mixer == true)
            {
                curStep.Mixers.ForEach(ctrlMixer =>
                {
                    for (int i = 0; i < lastStep.Mixers.Count; i++)
                    {
                        var tgtMixer = lastStep.Mixers[i];
                        if (tgtMixer.Name == ctrlMixer.Name)
                        {
                            if (tgtMixer.Get_Duty() != ctrlMixer.Get_Duty())
                            {
                                tgtMixer.Set_Duty(ctrlMixer.Get_Duty());
                            }
                        }
                    }
                });
            }
        }

        // Step Order Edit
        private void Insert_Step(int stepNo)
        {
            Step newStep = new Step();
            newStep.Create(stepNo, "");

            StepBtnList.Add(CreateStepBtn(stepNo));
            RefSequence.Steps.Insert(stepNo, newStep);
            // Redirect StepNo
            for (int i = 0; i < RefSequence.Steps.Count; i++)
            {
                RefSequence.Steps[i].No = i;

                StepBtnList[i].Text = $"Step {i:00}";
                StepBtnList[i].Tag = i;
            }
            SetStepContainer(stepNo);

            StepBtnList[stepNo].PerformClick();
        }

        private void Remove_Step(int stepNo)
        {
            StepBtnList.RemoveAt(StepBtnList.Count - 1);    // 단지 Button일 뿐이어서 마지막 인덱스의 것을 뺀다
            RefSequence.Steps.RemoveAt(stepNo);
            // Redirect StepNo
            for (int i = 0; i < RefSequence.Steps.Count; i++)
            {
                RefSequence.Steps[i].No = i;
            }

            if (StepBtnList.Count > 0)
            {
                int active = stepNo + 1;
                while (active >= StepBtnList.Count)
                {
                    --active;
                }

                SetStepContainer(active);

                StepBtnList[active].PerformClick();
            }
            else
            {
                ClearAll();
            }
        }

        private void btn_StepPagePrv_Click(object sender, EventArgs e)
        {
            SetStepContainer(ShownFirstStepNo - 1);
        }

        private void btn_StepPageNxt_Click(object sender, EventArgs e)
        {
            SetStepContainer(ShownFirstStepNo + 1);
        }

        public void SetStepContainer(int startFrom)
        {
            if (StepBtnList.Count == 0 || startFrom > StepBtnList.Count - 1)
            {
                return;
            }

            if (tbl_StepContainer.Controls.Count == 0)
            {
                int endTo = startFrom + 3;
                int colIdx = 0;
                int validIdx = 0;
                for (validIdx = startFrom; validIdx < endTo; validIdx++)
                {
                    if (validIdx < StepBtnList.Count)
                    {
                        tbl_StepContainer.Controls.Add(StepBtnList[validIdx], colIdx, 0);
                        ++colIdx;
                    }
                }

                StepBtnList[startFrom].PerformClick();

                ShownFirstStepNo = startFrom;
                btn_StepPagePrv.Enabled = startFrom != 0;
                btn_StepPageNxt.Enabled = validIdx < StepBtnList.Count;
            }
            else
            {
                if (tbl_StepContainer.Controls.Count < 4)
                {
                }
                else
                { 
                }
                Button btn = (Button)tbl_StepContainer.Controls[0];

                tbl_StepContainer.Controls.Clear();

                int endTo = startFrom + 3;
                int colIdx = 0;
                int validIdx = 0;
                for (validIdx = startFrom; validIdx < endTo; validIdx++)
                {
                    if (validIdx < StepBtnList.Count)
                    {
                        tbl_StepContainer.Controls.Add(StepBtnList[validIdx], colIdx, 0);
                        ++colIdx;
                    }
                }

                ShownFirstStepNo = startFrom;
                btn_StepPagePrv.Enabled = startFrom != 0;
                btn_StepPageNxt.Enabled = validIdx < StepBtnList.Count;

                //if ((int)btn.Tag != startFrom)
                //{
                //    tbl_StepContainer.Controls.Clear();

                //    int endTo = startFrom + 3;
                //    int colIdx = 0;
                //    int validIdx = 0;
                //    for (validIdx = startFrom; validIdx < endTo; validIdx++)
                //    {
                //        if (validIdx < StepBtnList.Count)
                //        {
                //            tbl_StepContainer.Controls.Add(StepBtnList[validIdx], colIdx, 0);
                //            ++colIdx;
                //        }
                //    }

                //    ShownFirstStepNo = startFrom;
                //    btn_StepPagePrv.Enabled = startFrom != 0;
                //    btn_StepPageNxt.Enabled = validIdx < StepBtnList.Count;
                //}
            }
        }

        public void SetBackground()
        {
            CmpVal_StepName.ValueClickedEvent += StepName_ValueClickedEvent;
            CmpCol_StepEnabled.Init_WithOutGenPrm("Enabled", new bool[] { false, true }, false);
            CmpCol_IsTitration.Init_WithOutGenPrm("Titration", new bool[] { false, true }, false);

            // Draw Control
            usrCtrl_Control.SetBackground();

            // Draw Titration
            usrCtrl_Titration.SetBackground();

            if (FluidicsControl.MainState == FluidicsState.Run)
            {
                btn_Add.Enabled = false;
                btn_Insert.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Clear.Enabled = false;

                CmpVal_StepName.EnableModifying(true, false);
                CmpCol_StepEnabled.EnableModifying(true, false);
                //CmpCol_IsTitration.EnableParameter(GlbVar.CurrentAuthority == UserAuthority.Admin);
                btn_ResetToPrvStep.Enabled = false;

                if (FluidicsControl.MainState == FluidicsState.Run)
                {
                    usrCtrl_Control.EnableControl(false);
                    usrCtrl_Titration.EnableControl(false);
                }
                else
                {
                    usrCtrl_Control.UserAuthorityIsChanged();
                    usrCtrl_Titration.UserAuthorityIsChanged();
                }
            }
            else
            {
                UserAuthorityIsChanged();
            }
        }

        private void StepName_ValueClickedEvent(object sender, object oldValue)
        {
            if (SelectedStepNo < 0)
            {
                return;
            }
            Frm_StrKeyPad keyPad = new Frm_StrKeyPad("Step Name", (string)oldValue);
            if (keyPad.ShowDialog() == DialogResult.OK)
            {
                RefSequence.Steps[SelectedStepNo].Name = keyPad.NewValue;
                CmpVal_StepName.Prm_Value = keyPad.NewValue;
            }
        }

        private Button CreateStepBtn(int stepNo)
        {
            Button btn = new Button();
            btn.FlatStyle = FlatStyle.Flat;
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            btn.Font = new Font("Consolas", 9f, FontStyle.Regular);
            btn.Margin = new Padding(1, 1, 1, 1);
            btn.Location = new Point(1, 1);
            btn.Text = $"Step {stepNo:00}";
            btn.Tag = stepNo;
            btn.Click += StepBtn_Click;
            return btn;
        }

        public void ClearAll()
        {
            StepBtnList.Clear();
            tbl_StepContainer.Controls.Clear();
            lbl_StepIdxInfo.Text = "-/-";
            CmpVal_StepName.Prm_Value = "";
            //usrCtrl_Control.Visible = false;
            //usrCtrl_Titration.Visible = false;
            SelectedStepNo = -1;
        }

        public void UpateNamePlates()
        {
            if (usrCtrl_Titration.Visible == true)
            {
                ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.Color_Name = Color.LemonChiffon);
                usrCtrl_Titration.Update_RefObject_ToCompare();
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Insert_Step(StepBtnList.Count);
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (RefSequence.No == 0 && SelectedStepNo == 0)
            {
                MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly("Unavailable to insert at first step of first sequence.");
                msg.ShowDialog();

                return;
            }

            Insert_Step(SelectedStepNo);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (RefSequence.No == 0 && SelectedStepNo == 0)
            {
                MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly("Unavailable to delete first step of first sequence.");
                msg.ShowDialog();

                return;
            }

            Remove_Step(SelectedStepNo);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            if (RefSequence.No == 0 && SelectedStepNo == 0)
            {
                MsgFrm_NotifyOnly msg = new MsgFrm_NotifyOnly("Unavailable to delete first step of first sequence.");
                msg.ShowDialog();

                return;
            }

            // TBD. Ask
            ClearAll();
            RefSequence.Steps.Clear();
        }

        private void btn_ResetToPrvStep_Click(object sender, EventArgs e)
        {
            usrCtrl_Control.SetSameAsPrvStep();
        }

        public void UserAuthorityIsChanged()
        {
            btn_Add.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
            btn_Insert.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
            btn_Delete.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;
            btn_Clear.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;

            CmpVal_StepName.EnableModifying(true, GlbVar.CurrentAuthority == UserAuthority.Admin);
            CmpCol_StepEnabled.EnableModifying(true, GlbVar.CurrentAuthority == UserAuthority.Admin);
            //CmpCol_IsTitration.EnableParameter(GlbVar.CurrentAuthority == UserAuthority.Admin);
            btn_ResetToPrvStep.Enabled = GlbVar.CurrentAuthority == UserAuthority.Admin;

            usrCtrl_Control.UserAuthorityIsChanged();
            usrCtrl_Titration.UserAuthorityIsChanged();
        }
    }
}
