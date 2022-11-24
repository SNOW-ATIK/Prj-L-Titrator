
namespace L_Titrator.Controls
{
    partial class Frm_PopUpMenu
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_View = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnl_View
            // 
            this.pnl_View.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_View.Location = new System.Drawing.Point(0, 0);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(202, 100);
            this.pnl_View.TabIndex = 0;
            // 
            // Frm_PopUpMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(202, 100);
            this.Controls.Add(this.pnl_View);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_PopUpMenu";
            this.Text = "Frm_PopUpMeneu";
            this.SizeChanged += new System.EventHandler(this.Frm_PopUpMenu_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_View;
    }
}