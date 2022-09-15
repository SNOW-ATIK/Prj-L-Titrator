
namespace L_Titrator_Alpha.Pages
{
    partial class Page_History
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
            this.pnl_View = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_View)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_View
            // 
            this.pnl_View.AllowTouchScroll = true;
            this.pnl_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_View.InvertTouchScroll = true;
            this.pnl_View.Location = new System.Drawing.Point(0, 0);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(1024, 634);
            this.pnl_View.TabIndex = 0;
            // 
            // Page_History
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnl_View);
            this.Name = "Page_History";
            this.Size = new System.Drawing.Size(1024, 634);
            ((System.ComponentModel.ISupportInitialize)(this.pnl_View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnl_View;
    }
}
