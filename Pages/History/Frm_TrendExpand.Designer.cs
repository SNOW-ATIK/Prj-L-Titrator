
namespace L_Titrator.Pages.History
{
    partial class Frm_TrendExpand
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
            this.btn_Close = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TrendChart = new DevExpress.XtraCharts.ChartControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CmpVal_Min = new ATIK.PrmCmp_Value();
            this.CmpVal_Max = new ATIK.PrmCmp_Value();
            this.CmpVal_Avg = new ATIK.PrmCmp_Value();
            this.CmpVal_RSD = new ATIK.PrmCmp_Value();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrendChart)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Close
            // 
            this.btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Close.BackColor = System.Drawing.Color.White;
            this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Close.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Bold);
            this.btn_Close.Location = new System.Drawing.Point(984, 0);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(40, 40);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "X";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TrendChart, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 14);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(996, 602);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // TrendChart
            // 
            this.TrendChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrendChart.Location = new System.Drawing.Point(3, 3);
            this.TrendChart.Name = "TrendChart";
            this.TrendChart.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.TrendChart.Size = new System.Drawing.Size(990, 554);
            this.TrendChart.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.CmpVal_Min, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CmpVal_Max, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.CmpVal_Avg, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.CmpVal_RSD, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 565);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(996, 37);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // CmpVal_Min
            // 
            this.CmpVal_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpVal_Min.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_Min.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_Min.Color_Value = System.Drawing.Color.White;
            this.CmpVal_Min.GenParam = null;
            this.CmpVal_Min.Location = new System.Drawing.Point(2, 2);
            this.CmpVal_Min.Margin = new System.Windows.Forms.Padding(2);
            this.CmpVal_Min.MaximumSize = new System.Drawing.Size(1000, 90);
            this.CmpVal_Min.MinimumSize = new System.Drawing.Size(30, 24);
            this.CmpVal_Min.Name = "CmpVal_Min";
            this.CmpVal_Min.NameTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CmpVal_Min.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CmpVal_Min.Prm_Name = "MIN";
            this.CmpVal_Min.Prm_Type = ATIK.PrmCmp.PrmType.Double;
            this.CmpVal_Min.Prm_Value = "";
            this.CmpVal_Min.Size = new System.Drawing.Size(245, 33);
            this.CmpVal_Min.SplitterDistance = 97;
            this.CmpVal_Min.TabIndex = 0;
            this.CmpVal_Min.UseKeyPadUI = false;
            this.CmpVal_Min.UseUserKeyPad = false;
            this.CmpVal_Min.ValueTextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CmpVal_Max
            // 
            this.CmpVal_Max.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpVal_Max.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_Max.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_Max.Color_Value = System.Drawing.Color.White;
            this.CmpVal_Max.GenParam = null;
            this.CmpVal_Max.Location = new System.Drawing.Point(251, 2);
            this.CmpVal_Max.Margin = new System.Windows.Forms.Padding(2);
            this.CmpVal_Max.MaximumSize = new System.Drawing.Size(1000, 90);
            this.CmpVal_Max.MinimumSize = new System.Drawing.Size(30, 24);
            this.CmpVal_Max.Name = "CmpVal_Max";
            this.CmpVal_Max.NameTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CmpVal_Max.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CmpVal_Max.Prm_Name = "MAX";
            this.CmpVal_Max.Prm_Type = ATIK.PrmCmp.PrmType.Double;
            this.CmpVal_Max.Prm_Value = "";
            this.CmpVal_Max.Size = new System.Drawing.Size(245, 33);
            this.CmpVal_Max.SplitterDistance = 97;
            this.CmpVal_Max.TabIndex = 0;
            this.CmpVal_Max.UseKeyPadUI = false;
            this.CmpVal_Max.UseUserKeyPad = false;
            this.CmpVal_Max.ValueTextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CmpVal_Avg
            // 
            this.CmpVal_Avg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpVal_Avg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_Avg.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_Avg.Color_Value = System.Drawing.Color.White;
            this.CmpVal_Avg.GenParam = null;
            this.CmpVal_Avg.Location = new System.Drawing.Point(500, 2);
            this.CmpVal_Avg.Margin = new System.Windows.Forms.Padding(2);
            this.CmpVal_Avg.MaximumSize = new System.Drawing.Size(1000, 90);
            this.CmpVal_Avg.MinimumSize = new System.Drawing.Size(30, 24);
            this.CmpVal_Avg.Name = "CmpVal_Avg";
            this.CmpVal_Avg.NameTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CmpVal_Avg.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CmpVal_Avg.Prm_Name = "AVG";
            this.CmpVal_Avg.Prm_Type = ATIK.PrmCmp.PrmType.Double;
            this.CmpVal_Avg.Prm_Value = "";
            this.CmpVal_Avg.Size = new System.Drawing.Size(245, 33);
            this.CmpVal_Avg.SplitterDistance = 97;
            this.CmpVal_Avg.TabIndex = 0;
            this.CmpVal_Avg.UseKeyPadUI = false;
            this.CmpVal_Avg.UseUserKeyPad = false;
            this.CmpVal_Avg.ValueTextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CmpVal_RSD
            // 
            this.CmpVal_RSD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpVal_RSD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_RSD.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_RSD.Color_Value = System.Drawing.Color.White;
            this.CmpVal_RSD.GenParam = null;
            this.CmpVal_RSD.Location = new System.Drawing.Point(749, 2);
            this.CmpVal_RSD.Margin = new System.Windows.Forms.Padding(2);
            this.CmpVal_RSD.MaximumSize = new System.Drawing.Size(1000, 90);
            this.CmpVal_RSD.MinimumSize = new System.Drawing.Size(30, 24);
            this.CmpVal_RSD.Name = "CmpVal_RSD";
            this.CmpVal_RSD.NameTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CmpVal_RSD.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CmpVal_RSD.Prm_Name = "RSD";
            this.CmpVal_RSD.Prm_Type = ATIK.PrmCmp.PrmType.String;
            this.CmpVal_RSD.Prm_Value = "";
            this.CmpVal_RSD.Size = new System.Drawing.Size(245, 33);
            this.CmpVal_RSD.SplitterDistance = 97;
            this.CmpVal_RSD.TabIndex = 0;
            this.CmpVal_RSD.UseKeyPadUI = false;
            this.CmpVal_RSD.UseUserKeyPad = false;
            this.CmpVal_RSD.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Frm_TrendExpand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(1024, 630);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_TrendExpand";
            this.Text = "Frm_TrendExpand";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Frm_TrendExpand_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TrendChart)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraCharts.ChartControl TrendChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ATIK.PrmCmp_Value CmpVal_Min;
        private ATIK.PrmCmp_Value CmpVal_Max;
        private ATIK.PrmCmp_Value CmpVal_Avg;
        private ATIK.PrmCmp_Value CmpVal_RSD;
    }
}