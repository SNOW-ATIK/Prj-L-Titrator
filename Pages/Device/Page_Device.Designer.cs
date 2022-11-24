
namespace L_Titrator.Pages
{
    partial class Page_Device
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Communication = new System.Windows.Forms.Button();
            this.btn_Overview = new System.Windows.Forms.Button();
            this.btn_IO = new System.Windows.Forms.Button();
            this.pnl_View = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnl_View, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 634);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btn_Communication, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_Overview, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_IO, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 585);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1022, 48);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btn_Communication
            // 
            this.btn_Communication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Communication.BackColor = System.Drawing.Color.White;
            this.btn_Communication.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Communication.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.btn_Communication.Location = new System.Drawing.Point(1, 1);
            this.btn_Communication.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Communication.Name = "btn_Communication";
            this.btn_Communication.Size = new System.Drawing.Size(253, 46);
            this.btn_Communication.TabIndex = 1;
            this.btn_Communication.TabStop = false;
            this.btn_Communication.Tag = "COMMUNICATION";
            this.btn_Communication.Text = "COMMUNICATION";
            this.btn_Communication.UseVisualStyleBackColor = false;
            this.btn_Communication.Click += new System.EventHandler(this.Request_ChangeSubPage);
            // 
            // btn_Overview
            // 
            this.btn_Overview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Overview.BackColor = System.Drawing.Color.White;
            this.btn_Overview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Overview.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.btn_Overview.Location = new System.Drawing.Point(766, 1);
            this.btn_Overview.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Overview.Name = "btn_Overview";
            this.btn_Overview.Size = new System.Drawing.Size(255, 46);
            this.btn_Overview.TabIndex = 1;
            this.btn_Overview.TabStop = false;
            this.btn_Overview.Tag = "OVERVIEW";
            this.btn_Overview.Text = "OVERVIEW";
            this.btn_Overview.UseVisualStyleBackColor = false;
            this.btn_Overview.Click += new System.EventHandler(this.Request_ChangeSubPage);
            // 
            // btn_IO
            // 
            this.btn_IO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_IO.BackColor = System.Drawing.Color.White;
            this.btn_IO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_IO.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.btn_IO.Location = new System.Drawing.Point(256, 1);
            this.btn_IO.Margin = new System.Windows.Forms.Padding(1);
            this.btn_IO.Name = "btn_IO";
            this.btn_IO.Size = new System.Drawing.Size(253, 46);
            this.btn_IO.TabIndex = 1;
            this.btn_IO.TabStop = false;
            this.btn_IO.Tag = "IO";
            this.btn_IO.Text = "I/O";
            this.btn_IO.UseVisualStyleBackColor = false;
            this.btn_IO.Click += new System.EventHandler(this.Request_ChangeSubPage);
            // 
            // pnl_View
            // 
            this.pnl_View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_View.Location = new System.Drawing.Point(0, 0);
            this.pnl_View.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(1024, 584);
            this.pnl_View.TabIndex = 3;
            // 
            // Page_Device
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Page_Device";
            this.Size = new System.Drawing.Size(1024, 634);
            this.VisibleChanged += new System.EventHandler(this.Page_Device_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_Communication;
        private System.Windows.Forms.Button btn_Overview;
        private System.Windows.Forms.Panel pnl_View;
        private System.Windows.Forms.Button btn_IO;
    }
}
