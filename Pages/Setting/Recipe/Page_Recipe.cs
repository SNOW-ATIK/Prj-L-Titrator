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
    public partial class Page_Recipe : UserControl, IPage, IAuthority
    {
        private enum ShownList
        { 
            General,
            HotKey
        }

        private SubPage_Recipe_Process ProcessPage = new SubPage_Recipe_Process();
        //private SubPage_Recipe_Measure OptionPage = new SubPage_Recipe_Measure();

        private const string CopiedRecipe = "CopiedRecipe";

        private int SelectedRecipeNo = -1;
        private ShownList SelectedRecipeList = ShownList.General;
        private List<Button> Btn_RecipeNos;
        private List<Label> Lbl_RecipeNames;

        private RecipeObj ClonedRcpObj;
        private RecipeObj ToBePastedRcpObj;

        public Page_Recipe()
        {
            InitializeComponent();

            Init_Page();
            Init_SubPages();
        }

        private void Init_Page()
        {
            Btn_RecipeNos = Handle_UI.GetAllControlsRecursive(tbl_Recipes).OfType<Button>().ToList();
            Btn_RecipeNos.ForEach(btn => btn.Tag = false);

            LoadRecipeNames();
        }

        private void Init_SubPages()
        {
            ProcessPage.SetVisible(false);
            ProcessPage.Dock = DockStyle.Fill;
            ProcessPage.Margin = new Padding(0, 0, 0, 0);
            ProcessPage.Location = new Point(0, 0);
            pnl_SubPageView.Controls.Add(ProcessPage);

            //OptionPage.Visible = false;
            //OptionPage.Dock = DockStyle.Fill;
            //OptionPage.Margin = new Padding(0, 0, 0, 0);
            //OptionPage.Location = new Point(0, 0);
            //pnl_SubPageView.Controls.Add(OptionPage);
        }

        private void LoadRecipeNames()
        {
            Lbl_RecipeNames = Handle_UI.GetAllControlsRecursive(tbl_Recipes).OfType<Label>().ToList();
            for (int i = 0; i < LT_Recipe.RecipeMaxCount; i++)
            {
                if (LT_Recipe.Get_RecipeObj(i, out var rcpObj) == true)
                {
                    Lbl_RecipeNames[i].Text = rcpObj.Name;
                    Lbl_RecipeNames[i].BackColor = Color.White;
                }
                else
                {
                    Lbl_RecipeNames[i].Text = "";
                    Lbl_RecipeNames[i].BackColor = Color.DarkGray;
                }
                Btn_RecipeNos[i].Text = $"{i}";
                Btn_RecipeNos[i].Enabled = true;
            }
        }

        private void LoadHotKeyNames()
        {
            Lbl_RecipeNames = Handle_UI.GetAllControlsRecursive(tbl_Recipes).OfType<Label>().ToList();
            for (int i = 0; i < LT_Recipe.RecipeMaxCount; i++)
            {
                int rcpNo = i + LT_Recipe.RecipeMaxCount;
                if (rcpNo < LT_Recipe.RecipeMaxCount + LT_Recipe.HotKeyMaxCount + LT_Recipe.ValidationMaxCount + LT_Recipe.DummyMaxCount)
                {
                    if (LT_Recipe.Get_RecipeObj(rcpNo, out var rcpObj) == true)
                    {
                        Lbl_RecipeNames[i].Text = rcpObj.Name;
                        Lbl_RecipeNames[i].BackColor = Color.White;
                    }
                    else
                    {
                        Lbl_RecipeNames[i].Text = "";
                        Lbl_RecipeNames[i].BackColor = Color.DarkGray;
                    }
                    Btn_RecipeNos[i].Text = $"{rcpNo}";
                }
                else
                {
                    Lbl_RecipeNames[i].Text = "";
                    Lbl_RecipeNames[i].BackColor = Color.DarkGray;
                    Btn_RecipeNos[i].Text = $"";
                    Btn_RecipeNos[i].Enabled = false;
                }
            }
        }

        private void SelectedRecipeChanged(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int idxOfCur = Btn_RecipeNos.IndexOf(btn);
            bool clicked = Convert.ToBoolean(btn.Tag);

            if (SelectedRecipeList == ShownList.General)
            {
                if (clicked == false)
                {
                    List<Button> prvClicked = Btn_RecipeNos.Where(prv => SelectedRecipeNo == int.Parse(prv.Name.Substring(prv.Name.Length - 1, 1))).ToList();
                    if (prvClicked.Count > 0)
                    {
                        prvClicked[0].Tag = false.ToString();
                        prvClicked[0].BackColor = Color.White;

                        int idxOfPrv = Btn_RecipeNos.IndexOf(prvClicked[0]);
                        if (LT_Recipe.RecipeExist(idxOfPrv) == true)
                        {
                            Lbl_RecipeNames[idxOfPrv].BackColor = Color.White;
                        }
                        else
                        {
                            Lbl_RecipeNames[idxOfPrv].BackColor = Color.DarkGray;
                        }
                    }
                    SelectedRecipeNo = int.Parse(btn.Name.Substring(btn.Name.Length - 1, 1));
                    btn.BackColor = Color.MediumSeaGreen;
                    btn.Tag = true.ToString();
                    Lbl_RecipeNames[idxOfCur].BackColor = Color.FromArgb(200, Color.MediumSeaGreen.G + 50, 200);

                    // Clear View
                    ProcessPage.ClearAll();
                    ProcessPage.SetVisible(false);

                    if (LT_Recipe.RecipeExist(idxOfCur) == true)
                    {
                        if (GlbVar.CurrentAuthority == UserAuthority.Admin)
                        {
                            btn_Create.Enabled = false;
                            btn_Delete.Enabled = true;
                            btn_Copy.Enabled = true;
                            btn_Paste.Enabled = false;
                        }
                        else
                        {
                            btn_Create.Enabled = false;
                            btn_Delete.Enabled = false;
                            btn_Copy.Enabled = false;
                            btn_Paste.Enabled = false;
                        }

                        if (LT_Recipe.Get_RecipeObj(SelectedRecipeNo, out var rcpObj) == true)
                        {
                            ProcessPage.SetVisible(true);

                            // Reload PreDefSeq to Edit
                            ProcessPage.Init_PreDefinedSeq();
                            // Make Clone
                            ClonedRcpObj = (RecipeObj)rcpObj.Clone();
                            ProcessPage.SetWorkReference(ClonedRcpObj);
                        }
                    }
                    else
                    {
                        if (GlbVar.CurrentAuthority == UserAuthority.Admin)
                        {
                            btn_Create.Enabled = true;
                            btn_Delete.Enabled = false;
                            btn_Copy.Enabled = false;
                            if (ToBePastedRcpObj != null)
                            {
                                btn_Paste.Enabled = true;
                            }
                            else
                            {
                                btn_Paste.Enabled = false;
                            }
                        }
                        else
                        {
                            btn_Create.Enabled = false;
                            btn_Delete.Enabled = false;
                            btn_Copy.Enabled = false;
                            btn_Paste.Enabled = false;
                        }
                    }
                }
            }
            else // if (SelectedRecipeList == ShownList.HotKey)
            {
                if (clicked == false)
                {
                    List<Button> prvClicked = Btn_RecipeNos.Where(prv => SelectedRecipeNo == int.Parse(prv.Name.Substring(prv.Name.Length - 1, 1)) + LT_Recipe.RecipeMaxCount).ToList();
                    if (prvClicked.Count > 0)
                    {
                        prvClicked[0].Tag = false.ToString();
                        prvClicked[0].BackColor = Color.White;

                        int idxOfPrv = Btn_RecipeNos.IndexOf(prvClicked[0]);
                        if (LT_Recipe.RecipeExist(idxOfPrv + LT_Recipe.RecipeMaxCount) == true)
                        {
                            Lbl_RecipeNames[idxOfPrv].BackColor = Color.White;
                        }
                        else
                        {
                            Lbl_RecipeNames[idxOfPrv].BackColor = Color.DarkGray;
                        }
                    }
                    SelectedRecipeNo = int.Parse(btn.Name.Substring(btn.Name.Length - 1, 1)) + LT_Recipe.RecipeMaxCount;
                    btn.BackColor = Color.MediumSeaGreen;
                    btn.Tag = true.ToString();
                    Lbl_RecipeNames[idxOfCur].BackColor = Color.FromArgb(200, Color.MediumSeaGreen.G + 50, 200);

                    // Clear View
                    ProcessPage.ClearAll();
                    ProcessPage.SetVisible(false);

                    if (LT_Recipe.RecipeExist(SelectedRecipeNo) == true)
                    {
                        if (GlbVar.CurrentAuthority == UserAuthority.Admin)
                        {
                            if (SelectedRecipeNo < LT_Recipe.RecipeMaxCount + LT_Recipe.HotKeyMaxCount)
                            {
                                btn_Create.Enabled = false;
                                btn_Delete.Enabled = false;
                                btn_Copy.Enabled = true;
                                btn_Paste.Enabled = false;
                            }
                            else
                            {
                                btn_Create.Enabled = false;
                                btn_Delete.Enabled = true;
                                btn_Copy.Enabled = true;
                                btn_Paste.Enabled = false;
                            }
                        }
                        else
                        {
                            btn_Create.Enabled = false;
                            btn_Delete.Enabled = false;
                            btn_Copy.Enabled = false;
                            btn_Paste.Enabled = false;
                        }

                        if (LT_Recipe.Get_RecipeObj(SelectedRecipeNo, out var rcpObj) == true)
                        {
                            ProcessPage.SetVisible(true);

                            // Reload PreDefSeq to Edit
                            ProcessPage.Init_PreDefinedSeq();
                            // Make Clone
                            ClonedRcpObj = (RecipeObj)rcpObj.Clone();
                            ProcessPage.SetWorkReference(ClonedRcpObj);
                        }
                    }
                    else
                    {
                        if (GlbVar.CurrentAuthority == UserAuthority.Admin)
                        {
                            if (SelectedRecipeNo < LT_Recipe.RecipeMaxCount + LT_Recipe.HotKeyMaxCount + LT_Recipe.ValidationMaxCount + LT_Recipe.DummyMaxCount)
                            {
                                btn_Create.Enabled = true;
                                btn_Delete.Enabled = false;
                                btn_Copy.Enabled = false;
                                if (ToBePastedRcpObj != null)
                                {
                                    btn_Paste.Enabled = true;
                                }
                                else
                                {
                                    btn_Paste.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            btn_Create.Enabled = false;
                            btn_Delete.Enabled = false;
                            btn_Copy.Enabled = false;
                            btn_Paste.Enabled = false;
                        }
                    }
                }
            }
        }

        private void RecipeName_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedRecipeNo < 0)
            {
                return;
            }
            if (GlbVar.CurrentAuthority != UserAuthority.Admin)
            {
                return;
            }
            if ((SelectedRecipeNo >= LT_Recipe.RecipeMaxCount) && (SelectedRecipeNo < LT_Recipe.RecipeMaxCount + LT_Recipe.HotKeyMaxCount))
            {
                return;
            }

            Label lbl = (Label)sender;
            int idxDblClicked = Lbl_RecipeNames.IndexOf(lbl);

            int coef = SelectedRecipeList == ShownList.General ? 0 : LT_Recipe.RecipeMaxCount;
            if (LT_Recipe.Get_RecipeObj(idxDblClicked + coef, out RecipeObj rcpObj) == false)
            {
                return;
            }

            Frm_StrKeyPad keyPad = new Frm_StrKeyPad($"Recipe {idxDblClicked + coef} Name", rcpObj.Name);
            if (keyPad.ShowDialog() == DialogResult.OK)
            {
                // TBD. Ask

                // Remove and Delete Old
                LT_Recipe.DeleteRecipe(rcpObj.No);

                // Save New
                string newName = keyPad.NewValue;
                rcpObj.Name = newName;
                if (ClonedRcpObj != null)
                {
                    ClonedRcpObj.Name = newName;
                }
                lbl.Text = newName;
                LT_Recipe.SerializeRecipe(rcpObj);
            }
        }

        private int Get_SelectedRecipeNo()
        {
            var selectedBtn = Handle_UI.GetAllControlsRecursive(tbl_Recipes).OfType<Button>().Where(btn => Convert.ToBoolean(btn.Tag) == true).ToList();
            if (selectedBtn.Count == 1)
            {
                int coef = SelectedRecipeList == ShownList.General ? 0 : LT_Recipe.RecipeMaxCount;
                int rcpNo = Btn_RecipeNos.IndexOf(selectedBtn[0]) + coef;
                return rcpNo;
            }
            return -1;
        }

        private bool SameAsOriginal()
        {
            if (LT_Recipe.Get_RecipeObj(SelectedRecipeNo, out RecipeObj org) == true)
            {
                bool rtn = ClonedRcpObj == org;
                return rtn;
            }
            throw new ArgumentNullException($"Original Recipe is not found. (RecipeNo={SelectedRecipeNo})");
        }

        private void SaveRecipe()
        {
            // 선택한 Recipe와 Clone한 Recipe를 비교하고, 
            // 변경 사항이 있으면 저장 여부를 물은 뒤,
            // 사용자 선택에 따라 처리한다.
            if (LT_Recipe.Get_RecipeObj(SelectedRecipeNo, out RecipeObj rcp) == true)
            {
                if (ClonedRcpObj == rcp)
                {
                    MsgFrm_NotifyOnly ntf = new MsgFrm_NotifyOnly("Recipe is same as original.");
                    ntf.ShowDialog();
                }
                else
                {
                    MsgFrm_AskYesNo ask = new MsgFrm_AskYesNo("Do you want to save?");
                    if (ask.ShowDialog() == DialogResult.Yes)
                    {
                        LT_Recipe.DeleteRecipe(SelectedRecipeNo);
                        LT_Recipe.SerializeRecipe(ClonedRcpObj);
                        ProcessPage.UpdateNamePlates();
                    }
                }
            }
        }

        private void Page_Recipe_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                btn_Create.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Copy.Enabled = false;
                btn_Paste.Enabled = false;

                LoadRecipeNames();
                UserAuthorityIsChanged();
            }
            else
            {
                // Clone한 Obj가 있는지 확인하고,
                if (ClonedRcpObj != null)
                {
                    if (SameAsOriginal() == false)
                    {
                        // 선택한 Recipe와 Clone한 Recipe를 비교하고, 
                        // 변경 사항이 있으면 저장 여부를 물은 뒤,
                        // 사용자 선택에 따라 처리한다.
                        SaveRecipe();
                    }

                    ClonedRcpObj = null;
                }

                // Clear View
                SelectedRecipeNo = -1;
                Lbl_RecipeNames.Clear();
                Btn_RecipeNos.ForEach(btn =>
                {
                    btn.BackColor = Color.White;
                    btn.Tag = false.ToString();
                });
                ProcessPage.ClearAll();
                ProcessPage.SetVisible(false);
            }
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            // TBD. Ask
            int rcpNo = Get_SelectedRecipeNo();
            if (LT_Recipe.RecipeExist(rcpNo) == true)
            {
                MsgFrm_NotifyOnly ntf = new MsgFrm_NotifyOnly("Select Empty Recipe No.");
                ntf.ShowDialog();
                return;
            }

            Frm_StrKeyPad frm = new Frm_StrKeyPad("RecipeName", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.NewValue != "")
                {
                    int coef = rcpNo < LT_Recipe.RecipeMaxCount ? 0 : LT_Recipe.RecipeMaxCount;
                    // Create
                    string rcpName = frm.NewValue;
                    if (LT_Recipe.CreateRecipe(rcpNo, rcpName) == true)
                    {
                        Lbl_RecipeNames[rcpNo - coef].Text = rcpName;

                        // Show Seq.Page
                        // Clear View
                        ProcessPage.ClearAll();
                        ProcessPage.SetVisible(false);

                        Btn_RecipeNos[rcpNo - coef].Tag = false.ToString();    // forced set to False to load pasted recipe
                        Btn_RecipeNos[rcpNo - coef].PerformClick();
                    }
                }
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            int rcpNo = Get_SelectedRecipeNo();
            if (rcpNo < 0)
            {
                return;
            }

            // TBD. Ask

            LT_Recipe.DeleteRecipe(rcpNo);

            int coef = rcpNo < LT_Recipe.RecipeMaxCount ? 0 : LT_Recipe.RecipeMaxCount;
            Lbl_RecipeNames[rcpNo - coef].Text = "";
            Lbl_RecipeNames[rcpNo - coef].BackColor = Color.DarkGray;
            Btn_RecipeNos[rcpNo - coef].Tag = false.ToString();
            Btn_RecipeNos[rcpNo - coef].BackColor = Color.White;

            // Clear View
            ProcessPage.ClearAll();
            ProcessPage.SetVisible(false);
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            int rcpNo = Get_SelectedRecipeNo();
            if (LT_Recipe.Get_RecipeObj(rcpNo, out RecipeObj rcpObj) == true)
            {
                ToBePastedRcpObj = (RecipeObj)rcpObj.Clone();
            }
        }

        private void btn_Paste_Click(object sender, EventArgs e)
        {
            // TBD. Ask
            RecipeObj rcpDst = (RecipeObj)ToBePastedRcpObj.Clone();

            // Set No.
            int dstRcpNo = Get_SelectedRecipeNo();
            rcpDst.No = dstRcpNo;

            // Set Name
            if (LT_Recipe.RecipeExist(rcpDst.Name) == true)
            {
                int duplicatedCnt = 1;
                string rcpDstNewName = "";
                bool duplicated = true;
                while (duplicated == true)
                {
                    rcpDstNewName = $"{rcpDst.Name} {duplicatedCnt}";
                    duplicated = LT_Recipe.RecipeExist(rcpDstNewName);
                    ++duplicatedCnt;
                }
                rcpDst.Name = rcpDstNewName;
            }

            LT_Recipe.SerializeRecipe(rcpDst);

            int cmpIdxOffset = SelectedRecipeList == ShownList.General ? 0 : LT_Recipe.RecipeMaxCount;
            Lbl_RecipeNames[rcpDst.No - cmpIdxOffset].Text = rcpDst.Name;
            Btn_RecipeNos[rcpDst.No - cmpIdxOffset].Tag = false.ToString();    // forced set to False to load pasted recipe
            Btn_RecipeNos[rcpDst.No - cmpIdxOffset].PerformClick();

            ToBePastedRcpObj = null;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (SameAsOriginal() == false)
            {
                SaveRecipe();
            }
        }

        private void btn_Restore_Click(object sender, EventArgs e)
        {
            // 선택한 Recipe와 Clone한 Recipe를 비교하고, 
            // 변경 사항이 있으면 복구 여부를 물은 뒤,
            // 사용자 선택에 따라 처리한다.
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
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
            switch (GlbVar.CurrentAuthority)
            {
                case UserAuthority.User:
                case UserAuthority.Engineer:
                    tbl_RecipeListSelect.ColumnStyles[0].Width = 100;
                    tbl_RecipeListSelect.ColumnStyles[1].Width = 0;

                    btn_RecipeGeneral.Text = "Recipe List";
                    btn_RecipeGeneral.PerformClick();

                    btn_Save.Enabled = false;
                    btn_Restore.Enabled = false;
                    break;

                case UserAuthority.Admin:
                    tbl_RecipeListSelect.ColumnStyles[0].Width = 50;
                    tbl_RecipeListSelect.ColumnStyles[1].Width = 50;

                    btn_RecipeGeneral.Text = "Recipe";
                    btn_RecipeGeneral.PerformClick();

                    if (FluidicsControl.MainState != FluidicsState.Run)
                    {
                        btn_Save.Enabled = true;
                        btn_Restore.Enabled = true;
                    }
                    break;
            }
        }

        private void btn_RecipeGeneral_Click(object sender, EventArgs e)
        {
            btn_RecipeGeneral.BackColor = Color.Gold;
            btn_HotKeyList.BackColor = Color.White;

            SelectedRecipeList = ShownList.General;
            Btn_RecipeNos.ForEach(btn =>
            {
                btn.Tag = false;
                btn.BackColor = Color.White;
            });
            // Clear View
            ProcessPage.ClearAll();
            ProcessPage.SetVisible(false);
            LoadRecipeNames();
        }

        private void btn_HotKeyList_Click(object sender, EventArgs e)
        {
            btn_RecipeGeneral.BackColor = Color.White;
            btn_HotKeyList.BackColor = Color.Gold;

            SelectedRecipeList = ShownList.HotKey;
            Btn_RecipeNos.ForEach(btn =>
            {
                btn.Tag = false;
                btn.BackColor = Color.White;
            });
            // Clear View
            ProcessPage.ClearAll();
            ProcessPage.SetVisible(false);
            LoadHotKeyNames();
        }
    }
}
