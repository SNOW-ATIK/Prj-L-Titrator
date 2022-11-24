
namespace L_Titrator.Pages
{
    partial class Page_Alarm
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
            this.pnl_View = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnl_View
            // 
            this.pnl_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_View.Location = new System.Drawing.Point(0, 0);
            this.pnl_View.Margin = new System.Windows.Forms.Padding(1);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(970, 580);
            this.pnl_View.TabIndex = 2;
            // 
            // Page_Alarm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnl_View);
            this.Name = "Page_Alarm";
            this.Size = new System.Drawing.Size(970, 580);
            this.Tag = "OPTION";
            this.VisibleChanged += new System.EventHandler(this.Page_Alarm_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_View;
    }
}
