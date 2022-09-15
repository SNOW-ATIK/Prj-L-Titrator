
namespace L_Titrator_Alpha.Controls
{
    partial class UsrCtrl_Light
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
            this.lbl_Light = new System.Windows.Forms.Label();
            this.btn_Light = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_Light, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Light, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(100, 62);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lbl_Light
            // 
            this.lbl_Light.BackColor = System.Drawing.Color.LemonChiffon;
            this.lbl_Light.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Light.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Light.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Light.Location = new System.Drawing.Point(1, 1);
            this.lbl_Light.Margin = new System.Windows.Forms.Padding(1);
            this.lbl_Light.Name = "lbl_Light";
            this.lbl_Light.Size = new System.Drawing.Size(98, 18);
            this.lbl_Light.TabIndex = 1;
            this.lbl_Light.Text = "Light";
            this.lbl_Light.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Light
            // 
            this.btn_Light.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Light.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Light.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.btn_Light.Location = new System.Drawing.Point(1, 21);
            this.btn_Light.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Light.Name = "btn_Light";
            this.btn_Light.Size = new System.Drawing.Size(98, 40);
            this.btn_Light.TabIndex = 2;
            this.btn_Light.Text = "ON";
            this.btn_Light.UseVisualStyleBackColor = true;
            // 
            // UsrCtrl_Light
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UsrCtrl_Light";
            this.Size = new System.Drawing.Size(100, 62);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_Light;
        private System.Windows.Forms.Button btn_Light;
    }
}
