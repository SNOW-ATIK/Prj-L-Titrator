
namespace L_Titrator_Alpha.Controls
{
    partial class UsrCtrl_MainControl
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
            this.btn_Measure = new System.Windows.Forms.Button();
            this.lbl_MeasureTarget = new System.Windows.Forms.Label();
            this.btn_Initialize = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btn_Measure, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbl_MeasureTarget, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Initialize, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 125);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Measure
            // 
            this.btn_Measure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Measure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Measure.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.btn_Measure.Location = new System.Drawing.Point(1, 77);
            this.btn_Measure.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Measure.Name = "btn_Measure";
            this.btn_Measure.Size = new System.Drawing.Size(196, 47);
            this.btn_Measure.TabIndex = 3;
            this.btn_Measure.Text = "MEASURE";
            this.btn_Measure.UseVisualStyleBackColor = true;
            // 
            // lbl_MeasureTarget
            // 
            this.lbl_MeasureTarget.BackColor = System.Drawing.Color.LemonChiffon;
            this.lbl_MeasureTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_MeasureTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_MeasureTarget.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_MeasureTarget.Location = new System.Drawing.Point(1, 1);
            this.lbl_MeasureTarget.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_MeasureTarget.Name = "lbl_MeasureTarget";
            this.lbl_MeasureTarget.Size = new System.Drawing.Size(196, 25);
            this.lbl_MeasureTarget.TabIndex = 1;
            this.lbl_MeasureTarget.Text = "Control";
            this.lbl_MeasureTarget.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Initialize
            // 
            this.btn_Initialize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Initialize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Initialize.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.btn_Initialize.Location = new System.Drawing.Point(1, 28);
            this.btn_Initialize.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Initialize.Name = "btn_Initialize";
            this.btn_Initialize.Size = new System.Drawing.Size(196, 47);
            this.btn_Initialize.TabIndex = 2;
            this.btn_Initialize.Text = "INITIALIZE";
            this.btn_Initialize.UseVisualStyleBackColor = true;
            this.btn_Initialize.Click += new System.EventHandler(this.btn_Initialize_Click);
            // 
            // UsrCtrl_MainControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Consolas", 9F);
            this.Name = "UsrCtrl_MainControl";
            this.Size = new System.Drawing.Size(198, 125);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_MeasureTarget;
        private System.Windows.Forms.Button btn_Initialize;
        private System.Windows.Forms.Button btn_Measure;
    }
}
