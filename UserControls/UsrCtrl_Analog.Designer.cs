
namespace L_Titrator.Controls
{
    partial class UsrCtrl_Analog
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Unit = new System.Windows.Forms.Label();
            this.lbl_Value = new System.Windows.Forms.Label();
            this.lbl_State = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_Name, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 49);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbl_Name
            // 
            this.lbl_Name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Name.BackColor = System.Drawing.Color.LemonChiffon;
            this.lbl_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Name.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Name.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Name.Location = new System.Drawing.Point(1, 1);
            this.lbl_Name.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(196, 20);
            this.lbl_Name.TabIndex = 5;
            this.lbl_Name.Text = "Analog";
            this.lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Controls.Add(this.lbl_Unit, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbl_Value, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbl_State, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 22);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(198, 27);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // lbl_Unit
            // 
            this.lbl_Unit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Unit.BackColor = System.Drawing.Color.White;
            this.lbl_Unit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Unit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Unit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Unit.Location = new System.Drawing.Point(159, 1);
            this.lbl_Unit.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_Unit.Name = "lbl_Unit";
            this.lbl_Unit.Size = new System.Drawing.Size(38, 25);
            this.lbl_Unit.TabIndex = 5;
            this.lbl_Unit.Text = "mV";
            this.lbl_Unit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Value
            // 
            this.lbl_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Value.BackColor = System.Drawing.Color.White;
            this.lbl_Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Value.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Value.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Value.Location = new System.Drawing.Point(28, 1);
            this.lbl_Value.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_Value.Name = "lbl_Value";
            this.lbl_Value.Size = new System.Drawing.Size(129, 25);
            this.lbl_Value.TabIndex = 5;
            this.lbl_Value.Text = "mV";
            this.lbl_Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Value.Click += new System.EventHandler(this.lbl_Value_Click);
            // 
            // lbl_State
            // 
            this.lbl_State.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_State.BackColor = System.Drawing.Color.White;
            this.lbl_State.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_State.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_State.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_State.Location = new System.Drawing.Point(1, 1);
            this.lbl_State.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_State.Name = "lbl_State";
            this.lbl_State.Size = new System.Drawing.Size(25, 25);
            this.lbl_State.TabIndex = 5;
            this.lbl_State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_State.Click += new System.EventHandler(this.lbl_Value_Click);
            // 
            // UsrCtrl_Analog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximumSize = new System.Drawing.Size(200, 100);
            this.MinimumSize = new System.Drawing.Size(160, 40);
            this.Name = "UsrCtrl_Analog";
            this.Size = new System.Drawing.Size(198, 49);
            this.SizeChanged += new System.EventHandler(this.UsrCtrl_Analog_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lbl_Unit;
        private System.Windows.Forms.Label lbl_Value;
        private System.Windows.Forms.Label lbl_State;
    }
}
