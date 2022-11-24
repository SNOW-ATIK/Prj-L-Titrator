
namespace L_Titrator.Controls
{
    partial class UsrCtrl_Fluidics_Large
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsrCtrl_Fluidics_Large));
            this.CmpVessel = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Vessel_Bmp();
            this.CmpSyringe1_Head = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Head_Bmp();
            this.CmpSyringe2_Head = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Head_Bmp();
            this.CmpSyringe2 = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Bmp();
            this.CmpSyringe1 = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Bmp();
            this.CmpReagent2 = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Reagent_Bmp();
            this.CmpReagent1 = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Reagent_Bmp();
            this.Cmp6Way = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_6Way_Bmp();
            this.Cmp2Way_Drain = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_2Way_Bmp();
            this.Cmp3Way_Sample = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_3Way_Bmp();
            this.Cmp3Way_DIW = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_3Way_Bmp();
            this.Cmp3Way_Validation = new ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_3Way_Bmp();
            this.SuspendLayout();
            // 
            // CmpVessel
            // 
            this.CmpVessel.BackColor = System.Drawing.Color.White;
            this.CmpVessel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpVessel.BackgroundImage")));
            this.CmpVessel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpVessel.Location = new System.Drawing.Point(450, 308);
            this.CmpVessel.Margin = new System.Windows.Forms.Padding(0);
            this.CmpVessel.Name = "CmpVessel";
            this.CmpVessel.Remains = 100;
            this.CmpVessel.Size = new System.Drawing.Size(122, 128);
            this.CmpVessel.TabIndex = 7;
            this.CmpVessel.VesselColor = ATIK.Common.ComponentEtc.Fluidics.Vessel_Color.Green;
            // 
            // CmpSyringe1_Head
            // 
            this.CmpSyringe1_Head.BackColor = System.Drawing.Color.White;
            this.CmpSyringe1_Head.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpSyringe1_Head.BackgroundImage")));
            this.CmpSyringe1_Head.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpSyringe1_Head.HeadType = ATIK.Common.ComponentEtc.Fluidics.Syringe_Head.LeftTopRight;
            this.CmpSyringe1_Head.Location = new System.Drawing.Point(270, 263);
            this.CmpSyringe1_Head.Margin = new System.Windows.Forms.Padding(0);
            this.CmpSyringe1_Head.Name = "CmpSyringe1_Head";
            this.CmpSyringe1_Head.Size = new System.Drawing.Size(60, 47);
            this.CmpSyringe1_Head.TabIndex = 6;
            this.CmpSyringe1_Head.Valve_Open = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.Left;
            this.CmpSyringe1_Head.Click += new System.EventHandler(this.CmpSyringe1_Click);
            // 
            // CmpSyringe2_Head
            // 
            this.CmpSyringe2_Head.BackColor = System.Drawing.Color.White;
            this.CmpSyringe2_Head.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpSyringe2_Head.BackgroundImage")));
            this.CmpSyringe2_Head.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpSyringe2_Head.HeadType = ATIK.Common.ComponentEtc.Fluidics.Syringe_Head.LeftRight;
            this.CmpSyringe2_Head.Location = new System.Drawing.Point(692, 264);
            this.CmpSyringe2_Head.Margin = new System.Windows.Forms.Padding(0);
            this.CmpSyringe2_Head.Name = "CmpSyringe2_Head";
            this.CmpSyringe2_Head.Size = new System.Drawing.Size(60, 47);
            this.CmpSyringe2_Head.TabIndex = 6;
            this.CmpSyringe2_Head.Valve_Open = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.Left;
            this.CmpSyringe2_Head.Click += new System.EventHandler(this.CmpSyringe2_Click);
            // 
            // CmpSyringe2
            // 
            this.CmpSyringe2.BackColor = System.Drawing.Color.White;
            this.CmpSyringe2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpSyringe2.BackgroundImage")));
            this.CmpSyringe2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpSyringe2.Location = new System.Drawing.Point(692, 310);
            this.CmpSyringe2.Margin = new System.Windows.Forms.Padding(0);
            this.CmpSyringe2.Name = "CmpSyringe2";
            this.CmpSyringe2.Remains = 100;
            this.CmpSyringe2.Size = new System.Drawing.Size(60, 205);
            this.CmpSyringe2.SyringeColor = ATIK.Common.ComponentEtc.Fluidics.Syringe_Color.Yellow;
            this.CmpSyringe2.TabIndex = 5;
            this.CmpSyringe2.Click += new System.EventHandler(this.CmpSyringe2_Click);
            // 
            // CmpSyringe1
            // 
            this.CmpSyringe1.BackColor = System.Drawing.Color.White;
            this.CmpSyringe1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpSyringe1.BackgroundImage")));
            this.CmpSyringe1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpSyringe1.Location = new System.Drawing.Point(270, 310);
            this.CmpSyringe1.Margin = new System.Windows.Forms.Padding(0);
            this.CmpSyringe1.Name = "CmpSyringe1";
            this.CmpSyringe1.Remains = 100;
            this.CmpSyringe1.Size = new System.Drawing.Size(60, 205);
            this.CmpSyringe1.SyringeColor = ATIK.Common.ComponentEtc.Fluidics.Syringe_Color.Orange;
            this.CmpSyringe1.TabIndex = 5;
            this.CmpSyringe1.Click += new System.EventHandler(this.CmpSyringe1_Click);
            // 
            // CmpReagent2
            // 
            this.CmpReagent2.BackColor = System.Drawing.Color.White;
            this.CmpReagent2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpReagent2.BackgroundImage")));
            this.CmpReagent2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpReagent2.Location = new System.Drawing.Point(811, 386);
            this.CmpReagent2.Margin = new System.Windows.Forms.Padding(0);
            this.CmpReagent2.Name = "CmpReagent2";
            this.CmpReagent2.ReagentColor = ATIK.Common.ComponentEtc.Fluidics.Reagent_Color.Yellow;
            this.CmpReagent2.ReagentOutLoc = ATIK.Common.ComponentEtc.Fluidics.Reagent_OutLocation.Right;
            this.CmpReagent2.Remains = 100;
            this.CmpReagent2.Size = new System.Drawing.Size(139, 107);
            this.CmpReagent2.TabIndex = 4;
            // 
            // CmpReagent1
            // 
            this.CmpReagent1.BackColor = System.Drawing.Color.White;
            this.CmpReagent1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CmpReagent1.BackgroundImage")));
            this.CmpReagent1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CmpReagent1.Location = new System.Drawing.Point(74, 386);
            this.CmpReagent1.Margin = new System.Windows.Forms.Padding(0);
            this.CmpReagent1.Name = "CmpReagent1";
            this.CmpReagent1.ReagentColor = ATIK.Common.ComponentEtc.Fluidics.Reagent_Color.Orange;
            this.CmpReagent1.ReagentOutLoc = ATIK.Common.ComponentEtc.Fluidics.Reagent_OutLocation.Left;
            this.CmpReagent1.Remains = 100;
            this.CmpReagent1.Size = new System.Drawing.Size(139, 107);
            this.CmpReagent1.TabIndex = 4;
            // 
            // Cmp6Way
            // 
            this.Cmp6Way.BackColor = System.Drawing.Color.White;
            this.Cmp6Way.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cmp6Way.BackgroundImage")));
            this.Cmp6Way.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Cmp6Way.Link_State = ATIK.Common.ComponentEtc.Fluidics.Valve_6Way_State.Link_None;
            this.Cmp6Way.Location = new System.Drawing.Point(677, 92);
            this.Cmp6Way.Margin = new System.Windows.Forms.Padding(0);
            this.Cmp6Way.Name = "Cmp6Way";
            this.Cmp6Way.Size = new System.Drawing.Size(88, 88);
            this.Cmp6Way.TabIndex = 3;
            this.Cmp6Way.Valve_Common_Port = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.None;
            this.Cmp6Way.Valve_Config = ATIK.Common.ComponentEtc.Fluidics.Valve_6Way_Cfg.None;
            this.Cmp6Way.Click += new System.EventHandler(this.Cmp6Way_Click);
            // 
            // Cmp2Way_Drain
            // 
            this.Cmp2Way_Drain.BackColor = System.Drawing.Color.White;
            this.Cmp2Way_Drain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cmp2Way_Drain.BackgroundImage")));
            this.Cmp2Way_Drain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Cmp2Way_Drain.Location = new System.Drawing.Point(483, 462);
            this.Cmp2Way_Drain.Margin = new System.Windows.Forms.Padding(0);
            this.Cmp2Way_Drain.Name = "Cmp2Way_Drain";
            this.Cmp2Way_Drain.Size = new System.Drawing.Size(57, 57);
            this.Cmp2Way_Drain.TabIndex = 2;
            this.Cmp2Way_Drain.Valve_Common_Port = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.None;
            this.Cmp2Way_Drain.Valve_Config = ATIK.Common.ComponentEtc.Fluidics.Valve_2Way_Cfg.TopBottom;
            this.Cmp2Way_Drain.Click += new System.EventHandler(this.Cmp2Way_Drain_Click);
            // 
            // Cmp3Way_Sample
            // 
            this.Cmp3Way_Sample.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cmp3Way_Sample.BackColor = System.Drawing.Color.White;
            this.Cmp3Way_Sample.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cmp3Way_Sample.BackgroundImage")));
            this.Cmp3Way_Sample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Cmp3Way_Sample.Location = new System.Drawing.Point(271, 93);
            this.Cmp3Way_Sample.Margin = new System.Windows.Forms.Padding(0);
            this.Cmp3Way_Sample.Name = "Cmp3Way_Sample";
            this.Cmp3Way_Sample.Size = new System.Drawing.Size(58, 58);
            this.Cmp3Way_Sample.TabIndex = 0;
            this.Cmp3Way_Sample.Valve_Common_Port = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.Bottom;
            this.Cmp3Way_Sample.Valve_Config = ATIK.Common.ComponentEtc.Fluidics.Valve_3Way_Cfg.BottomLeftTop;
            this.Cmp3Way_Sample.Click += new System.EventHandler(this.Cmp3Way_Sample_Click);
            // 
            // Cmp3Way_DIW
            // 
            this.Cmp3Way_DIW.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cmp3Way_DIW.BackColor = System.Drawing.Color.White;
            this.Cmp3Way_DIW.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cmp3Way_DIW.BackgroundImage")));
            this.Cmp3Way_DIW.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Cmp3Way_DIW.Location = new System.Drawing.Point(270, 10);
            this.Cmp3Way_DIW.Margin = new System.Windows.Forms.Padding(0);
            this.Cmp3Way_DIW.Name = "Cmp3Way_DIW";
            this.Cmp3Way_DIW.Size = new System.Drawing.Size(58, 58);
            this.Cmp3Way_DIW.TabIndex = 0;
            this.Cmp3Way_DIW.Valve_Common_Port = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.Left;
            this.Cmp3Way_DIW.Valve_Config = ATIK.Common.ComponentEtc.Fluidics.Valve_3Way_Cfg.RightBottomLeft;
            this.Cmp3Way_DIW.Click += new System.EventHandler(this.Cmp3Way_DIW_Click);
            // 
            // Cmp3Way_Validation
            // 
            this.Cmp3Way_Validation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cmp3Way_Validation.BackColor = System.Drawing.Color.White;
            this.Cmp3Way_Validation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cmp3Way_Validation.BackgroundImage")));
            this.Cmp3Way_Validation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Cmp3Way_Validation.Location = new System.Drawing.Point(272, 178);
            this.Cmp3Way_Validation.Margin = new System.Windows.Forms.Padding(0);
            this.Cmp3Way_Validation.Name = "Cmp3Way_Validation";
            this.Cmp3Way_Validation.Size = new System.Drawing.Size(58, 58);
            this.Cmp3Way_Validation.TabIndex = 0;
            this.Cmp3Way_Validation.Valve_Common_Port = ATIK.Common.ComponentEtc.Fluidics.Valve_PortDirection.Right;
            this.Cmp3Way_Validation.Valve_Config = ATIK.Common.ComponentEtc.Fluidics.Valve_3Way_Cfg.TopRightBottom;
            this.Cmp3Way_Validation.Click += new System.EventHandler(this.Cmp3Way_Validation_Click);
            // 
            // UsrCtrl_Fluidics_Large
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.CmpVessel);
            this.Controls.Add(this.CmpSyringe1_Head);
            this.Controls.Add(this.CmpSyringe2_Head);
            this.Controls.Add(this.CmpSyringe2);
            this.Controls.Add(this.CmpSyringe1);
            this.Controls.Add(this.CmpReagent2);
            this.Controls.Add(this.CmpReagent1);
            this.Controls.Add(this.Cmp6Way);
            this.Controls.Add(this.Cmp2Way_Drain);
            this.Controls.Add(this.Cmp3Way_Sample);
            this.Controls.Add(this.Cmp3Way_DIW);
            this.Controls.Add(this.Cmp3Way_Validation);
            this.DoubleBuffered = true;
            this.Name = "UsrCtrl_Fluidics_Large";
            this.Size = new System.Drawing.Size(1022, 582);
            this.ResumeLayout(false);

        }

        #endregion

        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_3Way_Bmp Cmp3Way_Validation;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_3Way_Bmp Cmp3Way_DIW;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_3Way_Bmp Cmp3Way_Sample;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_2Way_Bmp Cmp2Way_Drain;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Valve_6Way_Bmp Cmp6Way;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Reagent_Bmp CmpReagent1;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Reagent_Bmp CmpReagent2;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Bmp CmpSyringe1;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Bmp CmpSyringe2;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Head_Bmp CmpSyringe2_Head;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Syringe_Head_Bmp CmpSyringe1_Head;
        private ATIK.Common.ComponentEtc.Fluidics.Cmp_Vessel_Bmp CmpVessel;
    }
}
