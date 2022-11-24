
namespace L_Titrator
{
    partial class UsrCtrl_AlarmInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.chk_Set = new System.Windows.Forms.CheckBox();
            this.CmpCol_OutputRelay = new ATIK.PrmCmp_Collection();
            this.CmpCol_Level = new ATIK.PrmCmp_Collection();
            this.CmpVal_Description = new ATIK.PrmCmp_Value();
            this.CmpCol_Code = new ATIK.PrmCmp_Collection();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_Name, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CmpCol_OutputRelay, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.CmpCol_Level, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.chk_Set, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CmpVal_Description, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.CmpCol_Code, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(0, 55);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(798, 55);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbl_Name
            // 
            this.lbl_Name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Name.BackColor = System.Drawing.Color.LemonChiffon;
            this.lbl_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Name.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Name.Location = new System.Drawing.Point(56, 1);
            this.lbl_Name.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(131, 53);
            this.lbl_Name.TabIndex = 7;
            this.lbl_Name.Text = "Name";
            this.lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chk_Set
            // 
            this.chk_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_Set.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_Set.AutoSize = true;
            this.chk_Set.BackColor = System.Drawing.Color.White;
            this.chk_Set.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chk_Set.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSeaGreen;
            this.chk_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_Set.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.chk_Set.Location = new System.Drawing.Point(1, 1);
            this.chk_Set.Margin = new System.Windows.Forms.Padding(1);
            this.chk_Set.Name = "chk_Set";
            this.chk_Set.Size = new System.Drawing.Size(53, 53);
            this.chk_Set.TabIndex = 5;
            this.chk_Set.Text = "ON";
            this.chk_Set.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chk_Set.UseVisualStyleBackColor = false;
            this.chk_Set.CheckedChanged += new System.EventHandler(this.chk_Set_CheckedChanged);
            // 
            // CmpCol_OutputRelay
            // 
            this.CmpCol_OutputRelay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpCol_OutputRelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpCol_OutputRelay.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpCol_OutputRelay.Color_Value = System.Drawing.SystemColors.Window;
            this.CmpCol_OutputRelay.GenParam = null;
            this.CmpCol_OutputRelay.Location = new System.Drawing.Point(648, 1);
            this.CmpCol_OutputRelay.Margin = new System.Windows.Forms.Padding(1);
            this.CmpCol_OutputRelay.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpCol_OutputRelay.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpCol_OutputRelay.Name = "CmpCol_OutputRelay";
            this.CmpCol_OutputRelay.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpCol_OutputRelay.Prm_Name = "Output Relay";
            this.CmpCol_OutputRelay.Prm_Type = ATIK.PrmCmp.PrmType.Boolean;
            this.CmpCol_OutputRelay.Prm_Value = null;
            this.CmpCol_OutputRelay.Size = new System.Drawing.Size(149, 53);
            this.CmpCol_OutputRelay.SplitterDistance = 24;
            this.CmpCol_OutputRelay.TabIndex = 4;
            this.CmpCol_OutputRelay.Tag = "";
            this.CmpCol_OutputRelay.SelectedUserItemChangedEvent += new ATIK.PrmCmp_Collection.SelectedUserItemChangedEventHandler(this.CmpCol_OutputRelay_SelectedUserItemChangedEvent);
            // 
            // CmpCol_Level
            // 
            this.CmpCol_Level.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpCol_Level.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpCol_Level.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpCol_Level.Color_Value = System.Drawing.SystemColors.Window;
            this.CmpCol_Level.GenParam = null;
            this.CmpCol_Level.Location = new System.Drawing.Point(189, 1);
            this.CmpCol_Level.Margin = new System.Windows.Forms.Padding(1);
            this.CmpCol_Level.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpCol_Level.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpCol_Level.Name = "CmpCol_Level";
            this.CmpCol_Level.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpCol_Level.Prm_Name = "Level";
            this.CmpCol_Level.Prm_Type = ATIK.PrmCmp.PrmType.Boolean;
            this.CmpCol_Level.Prm_Value = null;
            this.CmpCol_Level.Size = new System.Drawing.Size(109, 53);
            this.CmpCol_Level.SplitterDistance = 25;
            this.CmpCol_Level.TabIndex = 4;
            this.CmpCol_Level.Tag = "";
            this.CmpCol_Level.SelectedUserItemChangedEvent += new ATIK.PrmCmp_Collection.SelectedUserItemChangedEventHandler(this.CmpCol_Level_SelectedUserItemChangedEvent);
            // 
            // CmpVal_Description
            // 
            this.CmpVal_Description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpVal_Description.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_Description.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_Description.Color_Value = System.Drawing.Color.White;
            this.CmpVal_Description.GenParam = null;
            this.CmpVal_Description.Location = new System.Drawing.Point(300, 1);
            this.CmpVal_Description.Margin = new System.Windows.Forms.Padding(1);
            this.CmpVal_Description.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpVal_Description.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpVal_Description.Name = "CmpVal_Description";
            this.CmpVal_Description.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpVal_Description.Prm_Name = "Description";
            this.CmpVal_Description.Prm_Type = ATIK.PrmCmp.PrmType.Integer;
            this.CmpVal_Description.Prm_Value = "";
            this.CmpVal_Description.Size = new System.Drawing.Size(220, 53);
            this.CmpVal_Description.SplitterDistance = 24;
            this.CmpVal_Description.TabIndex = 1;
            this.CmpVal_Description.UseKeyPadUI = false;
            this.CmpVal_Description.UseUserKeyPad = false;
            // 
            // CmpCol_Code
            // 
            this.CmpCol_Code.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpCol_Code.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpCol_Code.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpCol_Code.Color_Value = System.Drawing.SystemColors.Window;
            this.CmpCol_Code.GenParam = null;
            this.CmpCol_Code.Location = new System.Drawing.Point(522, 1);
            this.CmpCol_Code.Margin = new System.Windows.Forms.Padding(1);
            this.CmpCol_Code.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpCol_Code.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpCol_Code.Name = "CmpCol_Code";
            this.CmpCol_Code.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpCol_Code.Prm_Name = "Output Relay";
            this.CmpCol_Code.Prm_Type = ATIK.PrmCmp.PrmType.Boolean;
            this.CmpCol_Code.Prm_Value = null;
            this.CmpCol_Code.Size = new System.Drawing.Size(124, 53);
            this.CmpCol_Code.SplitterDistance = 24;
            this.CmpCol_Code.TabIndex = 4;
            this.CmpCol_Code.Tag = "";
            this.CmpCol_Code.SelectedUserItemChangedEvent += new ATIK.PrmCmp_Collection.SelectedUserItemChangedEventHandler(this.CmpCol_Code_SelectedUserItemChangedEvent);
            // 
            // UsrCtrl_AlarmInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UsrCtrl_AlarmInfo";
            this.Size = new System.Drawing.Size(798, 54);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ATIK.PrmCmp_Value CmpVal_Description;
        private ATIK.PrmCmp_Collection CmpCol_OutputRelay;
        private ATIK.PrmCmp_Collection CmpCol_Level;
        private System.Windows.Forms.CheckBox chk_Set;
        private System.Windows.Forms.Label lbl_Name;
        private ATIK.PrmCmp_Collection CmpCol_Code;
    }
}
