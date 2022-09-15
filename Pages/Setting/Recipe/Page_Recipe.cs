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

namespace L_Titrator_Alpha.Pages
{
    public partial class Page_Recipe : UserControl, IPage
    {
        private SubPage_Recipe_Process ProcessPage = new SubPage_Recipe_Process();
        //private SubPage_Recipe_Measure OptionPage = new SubPage_Recipe_Measure();

        private const string CopiedRecipe = "CopiedRecipe";

        private int SelectedRecipeNo = -1;
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

        private void ShowSubPage_ProcessOrSetting(object sender, EventArgs e)
        {
        }

        private void Init_Page()
        {
            Btn_RecipeNos = Handle_UI.GetAllControlsRecursive(tbl_Recipes).OfType<Button>().ToList();
            Btn_RecipeNos.ForEach(btn => btn.Tag = false);

            LoadRecipeNames();
        }

        private void Init_SubPages()
        {
            ProcessPage.Visible = false;
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
            }
        }

        private void SelectedRecipeChanged(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int idxOfCur = Btn_RecipeNos.IndexOf(btn);
            bool clicked = Convert.ToBoolean(btn.Tag);
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
                ProcessPage.Visible = false;

                if (LT_Recipe.RecipeExist(idxOfCur) == true)
                {
                    btn_Create.Enabled = false;
                    btn_Delete.Enabled = true;
                    btn_Copy.Enabled = true;
                    btn_Paste.Enabled = false;

                    if (LT_Recipe.Get_RecipeObj(SelectedRecipeNo, out var rcpObj) == true)
                    {
                        ProcessPage.Visible = true;

                        // Reload PreDefSeq to Edit
                        ProcessPage.Init_PreDefinedSeq();
                        // Make Clone
                        ClonedRcpObj = (RecipeObj)rcpObj.Clone();
                        ProcessPage.SetWorkReference(ClonedRcpObj);
                    }
                }
                else
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
        }

        private void RecipeName_DoubleClick(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int idxDblClicked = Lbl_RecipeNames.IndexOf(lbl);
            
            if (LT_Recipe.Get_RecipeObj(idxDblClicked, out RecipeObj rcpObj) == false)
            {
                return;
            }

            Frm_StrKeyPad keyPad = new Frm_StrKeyPad($"Recipe {idxDblClicked} Name", rcpObj.Name);
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
                int rcpNo = Btn_RecipeNos.IndexOf(selectedBtn[0]);
                return rcpNo;
            }
            return -1;
        }

        private bool SameAsOriginal()
        {
            if (LT_Recipe.Get_RecipeObj(SelectedRecipeNo, out RecipeObj org) == true)
            {
                return ClonedRcpObj == org;
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
                    }
                }
            }
        }

        private void RestoreRecipe()
        { 
        }

        private void Page_Recipe_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                LoadRecipeNames();
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
                ProcessPage.Visible = false;
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
                    // Create
                    string rcpName = frm.NewValue;
                    if (LT_Recipe.CreateRecipe(rcpNo, rcpName) == true)
                    {
                        Lbl_RecipeNames[rcpNo].Text = rcpName;

                        // Show Seq.Page
                        // Clear View
                        ProcessPage.ClearAll();
                        ProcessPage.Visible = false;

                        Btn_RecipeNos[rcpNo].Tag = false.ToString();    // forced set to False to load pasted recipe
                        Btn_RecipeNos[rcpNo].PerformClick();
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
            
            Lbl_RecipeNames[rcpNo].Text = "";
            Lbl_RecipeNames[rcpNo].BackColor = Color.DarkGray;
            Btn_RecipeNos[rcpNo].Tag = false.ToString();
            Btn_RecipeNos[rcpNo].BackColor = Color.White;

            // Clear View
            ProcessPage.ClearAll();
            ProcessPage.Visible = false;
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
                int duplicatedCnt = 2;
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

            Lbl_RecipeNames[rcpDst.No].Text = rcpDst.Name;
            Btn_RecipeNos[rcpDst.No].Tag = false.ToString();    // forced set to False to load pasted recipe
            Btn_RecipeNos[rcpDst.No].PerformClick();

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
    }
}
