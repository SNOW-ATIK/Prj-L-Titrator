
namespace L_Titrator.Pages
{
    partial class Page_DataHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page_DataHistory));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "StartTime",
            "0000-00-00 00:00:00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "EndTime",
            "0000-00-00 00:00:00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Titration Duration",
            "00:00:00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "SampleName",
            "-"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Injected",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Concentration",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Reagent",
            "-"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "MaxIteration",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Offset Volume",
            "0.00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "MixingTime(1st)",
            "00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "MixingTime(Gen)",
            "00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Factor(Vol2Con)",
            "000.000"}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "Target mV",
            "000"}, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "End mV",
            "000"}, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Large to Medium",
            "000"}, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Medium to Small",
            "000"}, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "Inj.Volume Large",
            "0.00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "Inj.Volume Medium",
            "0.00"}, -1);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] {
            "Inj.Volume Small",
            "0.00"}, -1);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.Calendar = new DevExpress.XtraEditors.Controls.CalendarControl();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_DataList = new System.Windows.Forms.Button();
            this.btn_Trend = new System.Windows.Forms.Button();
            this.tbl_DataList_Or_Trend = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_OneDayList = new System.Windows.Forms.DataGridView();
            this.col_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_RecipeInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbl_Trend = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.chk_DayTrend = new System.Windows.Forms.CheckBox();
            this.chk_MonthTrend = new System.Windows.Forms.CheckBox();
            this.chk_PeriodTrend = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_SetPeriod = new System.Windows.Forms.Button();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.CmpVal_Period_To = new ATIK.PrmCmp_Value();
            this.CmpVal_Period_From = new ATIK.PrmCmp_Value();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.CmpCol_Recipe = new ATIK.PrmCmp_Collection();
            this.CmpCol_Target = new ATIK.PrmCmp_Collection();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_ZoomIn = new System.Windows.Forms.Button();
            this.btn_ZoomOut = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Expand = new System.Windows.Forms.Button();
            this.TrendChart = new DevExpress.XtraCharts.ChartControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.lsv_SelectedInfoSummary = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_TitrationList = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_InjectionInfo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Analog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.usrCtrl_TitrationGraph1 = new L_Titrator.Controls.UsrCtrl_TitrationGraph();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Calendar.CalendarTimeProperties)).BeginInit();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tbl_DataList_Or_Trend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OneDayList)).BeginInit();
            this.tbl_Trend.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrendChart)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InjectionInfo)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 317F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 634);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 304F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel8, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1024, 317);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.Calendar, 0, 1);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(304, 317);
            this.tableLayoutPanel7.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.Color.LemonChiffon;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1, 1);
            this.label6.Margin = new System.Windows.Forms.Padding(1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(302, 25);
            this.label6.TabIndex = 1;
            this.label6.Text = "Calendar";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Calendar
            // 
            this.Calendar.AllowClickInactiveDays = false;
            this.Calendar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Calendar.CalendarAppearance.DayCell.Options.UseTextOptions = true;
            this.Calendar.CalendarAppearance.DayCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Calendar.CalendarAppearance.DayCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.Calendar.CalendarAppearance.DayCellDisabled.Options.UseTextOptions = true;
            this.Calendar.CalendarAppearance.DayCellDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Calendar.CalendarAppearance.DayCellDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.Calendar.CalendarAppearance.DayCellHighlighted.Options.UseTextOptions = true;
            this.Calendar.CalendarAppearance.DayCellHighlighted.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Calendar.CalendarAppearance.DayCellHighlighted.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.Calendar.CalendarAppearance.DayCellHoliday.Options.UseTextOptions = true;
            this.Calendar.CalendarAppearance.DayCellHoliday.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Calendar.CalendarAppearance.DayCellHoliday.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.Calendar.CalendarAppearance.DayCellSelected.Options.UseTextOptions = true;
            this.Calendar.CalendarAppearance.DayCellSelected.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Calendar.CalendarAppearance.DayCellSelected.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.Calendar.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.False;
            this.Calendar.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Calendar.CellSize = new System.Drawing.Size(42, 32);
            this.Calendar.DateTime = new System.DateTime(2022, 5, 27, 0, 0, 0, 0);
            this.Calendar.DrawCellLines = true;
            this.Calendar.EditValue = new System.DateTime(2022, 5, 27, 0, 0, 0, 0);
            this.Calendar.HighlightTodayCell = DevExpress.Utils.DefaultBoolean.False;
            this.Calendar.HighlightTodayCellWhenSelected = false;
            this.Calendar.InactiveDaysVisibility = DevExpress.XtraEditors.Controls.CalendarInactiveDaysVisibility.Hidden;
            this.Calendar.Location = new System.Drawing.Point(1, 28);
            this.Calendar.Margin = new System.Windows.Forms.Padding(1);
            this.Calendar.Name = "Calendar";
            this.Calendar.ShowFooter = false;
            this.Calendar.ShowTodayButton = false;
            this.Calendar.ShowToolTips = false;
            this.Calendar.Size = new System.Drawing.Size(302, 288);
            this.Calendar.TabIndex = 2;
            this.Calendar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.calendarControl1_MouseUp);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tbl_DataList_Or_Trend, 0, 1);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(304, 0);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(720, 317);
            this.tableLayoutPanel8.TabIndex = 6;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.btn_DataList, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btn_Trend, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(720, 27);
            this.tableLayoutPanel9.TabIndex = 6;
            // 
            // btn_DataList
            // 
            this.btn_DataList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DataList.BackColor = System.Drawing.Color.White;
            this.btn_DataList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DataList.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold);
            this.btn_DataList.Location = new System.Drawing.Point(1, 1);
            this.btn_DataList.Margin = new System.Windows.Forms.Padding(1);
            this.btn_DataList.Name = "btn_DataList";
            this.btn_DataList.Size = new System.Drawing.Size(358, 25);
            this.btn_DataList.TabIndex = 1;
            this.btn_DataList.Text = "DATA LIST";
            this.btn_DataList.UseVisualStyleBackColor = false;
            this.btn_DataList.Click += new System.EventHandler(this.btn_DataList_Click);
            // 
            // btn_Trend
            // 
            this.btn_Trend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Trend.BackColor = System.Drawing.Color.White;
            this.btn_Trend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Trend.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold);
            this.btn_Trend.Location = new System.Drawing.Point(361, 1);
            this.btn_Trend.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Trend.Name = "btn_Trend";
            this.btn_Trend.Size = new System.Drawing.Size(358, 25);
            this.btn_Trend.TabIndex = 1;
            this.btn_Trend.Text = "TREND";
            this.btn_Trend.UseVisualStyleBackColor = false;
            this.btn_Trend.Click += new System.EventHandler(this.btn_Trend_Click);
            // 
            // tbl_DataList_Or_Trend
            // 
            this.tbl_DataList_Or_Trend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbl_DataList_Or_Trend.ColumnCount = 2;
            this.tbl_DataList_Or_Trend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tbl_DataList_Or_Trend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tbl_DataList_Or_Trend.Controls.Add(this.dgv_OneDayList, 0, 0);
            this.tbl_DataList_Or_Trend.Controls.Add(this.tbl_Trend, 1, 0);
            this.tbl_DataList_Or_Trend.Location = new System.Drawing.Point(0, 27);
            this.tbl_DataList_Or_Trend.Margin = new System.Windows.Forms.Padding(0);
            this.tbl_DataList_Or_Trend.Name = "tbl_DataList_Or_Trend";
            this.tbl_DataList_Or_Trend.RowCount = 1;
            this.tbl_DataList_Or_Trend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbl_DataList_Or_Trend.Size = new System.Drawing.Size(720, 290);
            this.tbl_DataList_Or_Trend.TabIndex = 6;
            // 
            // dgv_OneDayList
            // 
            this.dgv_OneDayList.AllowUserToAddRows = false;
            this.dgv_OneDayList.AllowUserToDeleteRows = false;
            this.dgv_OneDayList.AllowUserToResizeColumns = false;
            this.dgv_OneDayList.AllowUserToResizeRows = false;
            this.dgv_OneDayList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_OneDayList.ColumnHeadersHeight = 25;
            this.dgv_OneDayList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_No,
            this.col_StartTime,
            this.col_EndTime,
            this.col_Duration,
            this.col_RecipeInfo});
            this.dgv_OneDayList.Location = new System.Drawing.Point(1, 1);
            this.dgv_OneDayList.Margin = new System.Windows.Forms.Padding(1);
            this.dgv_OneDayList.MultiSelect = false;
            this.dgv_OneDayList.Name = "dgv_OneDayList";
            this.dgv_OneDayList.ReadOnly = true;
            this.dgv_OneDayList.RowHeadersVisible = false;
            this.dgv_OneDayList.RowTemplate.Height = 23;
            this.dgv_OneDayList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_OneDayList.Size = new System.Drawing.Size(214, 288);
            this.dgv_OneDayList.TabIndex = 6;
            this.dgv_OneDayList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_OneDayList_CellClick);
            this.dgv_OneDayList.SelectionChanged += new System.EventHandler(this.dgv_OneDayList_SelectionChanged);
            // 
            // col_No
            // 
            this.col_No.Frozen = true;
            this.col_No.HeaderText = "No";
            this.col_No.Name = "col_No";
            this.col_No.ReadOnly = true;
            this.col_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_No.Width = 50;
            // 
            // col_StartTime
            // 
            this.col_StartTime.Frozen = true;
            this.col_StartTime.HeaderText = "StartTime";
            this.col_StartTime.Name = "col_StartTime";
            this.col_StartTime.ReadOnly = true;
            this.col_StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_StartTime.Width = 175;
            // 
            // col_EndTime
            // 
            this.col_EndTime.Frozen = true;
            this.col_EndTime.HeaderText = "EndTime";
            this.col_EndTime.Name = "col_EndTime";
            this.col_EndTime.ReadOnly = true;
            this.col_EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_EndTime.Width = 175;
            // 
            // col_Duration
            // 
            this.col_Duration.Frozen = true;
            this.col_Duration.HeaderText = "Duration";
            this.col_Duration.Name = "col_Duration";
            this.col_Duration.ReadOnly = true;
            this.col_Duration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_Duration.Width = 90;
            // 
            // col_RecipeInfo
            // 
            this.col_RecipeInfo.Frozen = true;
            this.col_RecipeInfo.HeaderText = "RecipeInfo";
            this.col_RecipeInfo.Name = "col_RecipeInfo";
            this.col_RecipeInfo.ReadOnly = true;
            this.col_RecipeInfo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_RecipeInfo.Width = 200;
            // 
            // tbl_Trend
            // 
            this.tbl_Trend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbl_Trend.ColumnCount = 3;
            this.tbl_Trend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tbl_Trend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbl_Trend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tbl_Trend.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tbl_Trend.Controls.Add(this.tableLayoutPanel16, 2, 0);
            this.tbl_Trend.Controls.Add(this.panel1, 1, 0);
            this.tbl_Trend.Location = new System.Drawing.Point(216, 0);
            this.tbl_Trend.Margin = new System.Windows.Forms.Padding(0);
            this.tbl_Trend.Name = "tbl_Trend";
            this.tbl_Trend.RowCount = 1;
            this.tbl_Trend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tbl_Trend.Size = new System.Drawing.Size(504, 290);
            this.tbl_Trend.TabIndex = 8;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel11.ColumnCount = 1;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel13, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel14, 0, 1);
            this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(250, 290);
            this.tableLayoutPanel11.TabIndex = 10;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel13.ColumnCount = 1;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Controls.Add(this.chk_DayTrend, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.chk_MonthTrend, 0, 1);
            this.tableLayoutPanel13.Controls.Add(this.chk_PeriodTrend, 0, 2);
            this.tableLayoutPanel13.Controls.Add(this.tableLayoutPanel10, 0, 3);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 4;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(250, 182);
            this.tableLayoutPanel13.TabIndex = 10;
            // 
            // chk_DayTrend
            // 
            this.chk_DayTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_DayTrend.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_DayTrend.BackColor = System.Drawing.Color.White;
            this.chk_DayTrend.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSeaGreen;
            this.chk_DayTrend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_DayTrend.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_DayTrend.Location = new System.Drawing.Point(1, 1);
            this.chk_DayTrend.Margin = new System.Windows.Forms.Padding(1);
            this.chk_DayTrend.Name = "chk_DayTrend";
            this.chk_DayTrend.Size = new System.Drawing.Size(248, 25);
            this.chk_DayTrend.TabIndex = 2;
            this.chk_DayTrend.Text = "DAY";
            this.chk_DayTrend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chk_DayTrend.UseVisualStyleBackColor = false;
            this.chk_DayTrend.CheckedChanged += new System.EventHandler(this.chk_DayTrend_CheckedChanged);
            // 
            // chk_MonthTrend
            // 
            this.chk_MonthTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_MonthTrend.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_MonthTrend.BackColor = System.Drawing.Color.White;
            this.chk_MonthTrend.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSeaGreen;
            this.chk_MonthTrend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_MonthTrend.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_MonthTrend.Location = new System.Drawing.Point(1, 28);
            this.chk_MonthTrend.Margin = new System.Windows.Forms.Padding(1);
            this.chk_MonthTrend.Name = "chk_MonthTrend";
            this.chk_MonthTrend.Size = new System.Drawing.Size(248, 25);
            this.chk_MonthTrend.TabIndex = 2;
            this.chk_MonthTrend.Text = "MONTH";
            this.chk_MonthTrend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chk_MonthTrend.UseVisualStyleBackColor = false;
            this.chk_MonthTrend.CheckedChanged += new System.EventHandler(this.chk_MonthTrend_CheckedChanged);
            // 
            // chk_PeriodTrend
            // 
            this.chk_PeriodTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_PeriodTrend.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_PeriodTrend.BackColor = System.Drawing.Color.White;
            this.chk_PeriodTrend.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSeaGreen;
            this.chk_PeriodTrend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk_PeriodTrend.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_PeriodTrend.Location = new System.Drawing.Point(1, 55);
            this.chk_PeriodTrend.Margin = new System.Windows.Forms.Padding(1);
            this.chk_PeriodTrend.Name = "chk_PeriodTrend";
            this.chk_PeriodTrend.Size = new System.Drawing.Size(248, 25);
            this.chk_PeriodTrend.TabIndex = 2;
            this.chk_PeriodTrend.Text = "PERIOD";
            this.chk_PeriodTrend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chk_PeriodTrend.UseVisualStyleBackColor = false;
            this.chk_PeriodTrend.CheckedChanged += new System.EventHandler(this.chk_PeriodTrend_CheckedChanged);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel10.Controls.Add(this.btn_SetPeriod, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel15, 0, 0);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 81);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(250, 101);
            this.tableLayoutPanel10.TabIndex = 3;
            // 
            // btn_SetPeriod
            // 
            this.btn_SetPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SetPeriod.BackColor = System.Drawing.Color.White;
            this.btn_SetPeriod.Enabled = false;
            this.btn_SetPeriod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetPeriod.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold);
            this.btn_SetPeriod.Location = new System.Drawing.Point(206, 1);
            this.btn_SetPeriod.Margin = new System.Windows.Forms.Padding(1);
            this.btn_SetPeriod.Name = "btn_SetPeriod";
            this.btn_SetPeriod.Size = new System.Drawing.Size(43, 99);
            this.btn_SetPeriod.TabIndex = 1;
            this.btn_SetPeriod.Text = "SET";
            this.btn_SetPeriod.UseVisualStyleBackColor = false;
            this.btn_SetPeriod.Click += new System.EventHandler(this.btn_SetPeriod_Click);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel15.ColumnCount = 1;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.CmpVal_Period_To, 0, 1);
            this.tableLayoutPanel15.Controls.Add(this.CmpVal_Period_From, 0, 0);
            this.tableLayoutPanel15.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(205, 101);
            this.tableLayoutPanel15.TabIndex = 2;
            // 
            // CmpVal_Period_To
            // 
            this.CmpVal_Period_To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_Period_To.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_Period_To.Color_Value = System.Drawing.Color.White;
            this.CmpVal_Period_To.Enabled = false;
            this.CmpVal_Period_To.GenParam = null;
            this.CmpVal_Period_To.Location = new System.Drawing.Point(1, 51);
            this.CmpVal_Period_To.Margin = new System.Windows.Forms.Padding(1);
            this.CmpVal_Period_To.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpVal_Period_To.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpVal_Period_To.Name = "CmpVal_Period_To";
            this.CmpVal_Period_To.NameTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CmpVal_Period_To.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpVal_Period_To.Prm_Name = "To";
            this.CmpVal_Period_To.Prm_Type = ATIK.PrmCmp.PrmType.String;
            this.CmpVal_Period_To.Prm_Value = "";
            this.CmpVal_Period_To.Size = new System.Drawing.Size(203, 49);
            this.CmpVal_Period_To.SplitterDistance = 22;
            this.CmpVal_Period_To.TabIndex = 0;
            this.CmpVal_Period_To.UseKeyPadUI = true;
            this.CmpVal_Period_To.UseUserKeyPad = true;
            this.CmpVal_Period_To.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CmpVal_Period_To.ValueClickedEvent += new ATIK.PrmCmp_Value.ValueClicked(this.CmpVal_Period_To_ValueClickedEvent);
            this.CmpVal_Period_To.EnabledChanged += new System.EventHandler(this.CmpVal_ModifyPeriod_EnabledChanged);
            // 
            // CmpVal_Period_From
            // 
            this.CmpVal_Period_From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpVal_Period_From.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpVal_Period_From.Color_Value = System.Drawing.Color.White;
            this.CmpVal_Period_From.Enabled = false;
            this.CmpVal_Period_From.GenParam = null;
            this.CmpVal_Period_From.Location = new System.Drawing.Point(1, 1);
            this.CmpVal_Period_From.Margin = new System.Windows.Forms.Padding(1);
            this.CmpVal_Period_From.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpVal_Period_From.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpVal_Period_From.Name = "CmpVal_Period_From";
            this.CmpVal_Period_From.NameTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CmpVal_Period_From.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpVal_Period_From.Prm_Name = "From";
            this.CmpVal_Period_From.Prm_Type = ATIK.PrmCmp.PrmType.String;
            this.CmpVal_Period_From.Prm_Value = "";
            this.CmpVal_Period_From.Size = new System.Drawing.Size(203, 49);
            this.CmpVal_Period_From.SplitterDistance = 22;
            this.CmpVal_Period_From.TabIndex = 0;
            this.CmpVal_Period_From.UseKeyPadUI = true;
            this.CmpVal_Period_From.UseUserKeyPad = true;
            this.CmpVal_Period_From.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CmpVal_Period_From.ValueClickedEvent += new ATIK.PrmCmp_Value.ValueClicked(this.CmpVal_Period_From_ValueClickedEvent);
            this.CmpVal_Period_From.EnabledChanged += new System.EventHandler(this.CmpVal_ModifyPeriod_EnabledChanged);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Controls.Add(this.CmpCol_Recipe, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.CmpCol_Target, 0, 1);
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 182);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 2;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(250, 108);
            this.tableLayoutPanel14.TabIndex = 10;
            // 
            // CmpCol_Recipe
            // 
            this.CmpCol_Recipe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpCol_Recipe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpCol_Recipe.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpCol_Recipe.Color_Value = System.Drawing.SystemColors.Window;
            this.CmpCol_Recipe.GenParam = null;
            this.CmpCol_Recipe.Location = new System.Drawing.Point(1, 1);
            this.CmpCol_Recipe.Margin = new System.Windows.Forms.Padding(1);
            this.CmpCol_Recipe.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpCol_Recipe.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpCol_Recipe.Name = "CmpCol_Recipe";
            this.CmpCol_Recipe.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpCol_Recipe.Prm_Name = "Recipe";
            this.CmpCol_Recipe.Prm_Type = ATIK.PrmCmp.PrmType.Boolean;
            this.CmpCol_Recipe.Prm_Value = null;
            this.CmpCol_Recipe.Size = new System.Drawing.Size(248, 52);
            this.CmpCol_Recipe.SplitterDistance = 25;
            this.CmpCol_Recipe.TabIndex = 0;
            this.CmpCol_Recipe.SelectedUserItemChangedEvent += new ATIK.PrmCmp_Collection.SelectedUserItemChangedEventHandler(this.CmpCol_Recipe_SelectedUserItemChangedEvent);
            // 
            // CmpCol_Target
            // 
            this.CmpCol_Target.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmpCol_Target.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CmpCol_Target.Color_Name = System.Drawing.Color.LemonChiffon;
            this.CmpCol_Target.Color_Value = System.Drawing.SystemColors.Window;
            this.CmpCol_Target.GenParam = null;
            this.CmpCol_Target.Location = new System.Drawing.Point(1, 55);
            this.CmpCol_Target.Margin = new System.Windows.Forms.Padding(1);
            this.CmpCol_Target.MaximumSize = new System.Drawing.Size(1000, 96);
            this.CmpCol_Target.MinimumSize = new System.Drawing.Size(30, 49);
            this.CmpCol_Target.Name = "CmpCol_Target";
            this.CmpCol_Target.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.CmpCol_Target.Prm_Name = "Target";
            this.CmpCol_Target.Prm_Type = ATIK.PrmCmp.PrmType.Boolean;
            this.CmpCol_Target.Prm_Value = null;
            this.CmpCol_Target.Size = new System.Drawing.Size(248, 52);
            this.CmpCol_Target.SplitterDistance = 25;
            this.CmpCol_Target.TabIndex = 0;
            this.CmpCol_Target.SelectedUserItemChangedEvent += new ATIK.PrmCmp_Collection.SelectedUserItemChangedEventHandler(this.CmpCol_Target_SelectedUserItemChangedEvent);
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Controls.Add(this.btn_ZoomIn, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.btn_ZoomOut, 0, 2);
            this.tableLayoutPanel16.Location = new System.Drawing.Point(462, 0);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 3;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(42, 290);
            this.tableLayoutPanel16.TabIndex = 12;
            // 
            // btn_ZoomIn
            // 
            this.btn_ZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ZoomIn.BackColor = System.Drawing.Color.White;
            this.btn_ZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ZoomIn.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Bold);
            this.btn_ZoomIn.Location = new System.Drawing.Point(1, 1);
            this.btn_ZoomIn.Margin = new System.Windows.Forms.Padding(1);
            this.btn_ZoomIn.Name = "btn_ZoomIn";
            this.btn_ZoomIn.Size = new System.Drawing.Size(40, 40);
            this.btn_ZoomIn.TabIndex = 1;
            this.btn_ZoomIn.Text = "+";
            this.btn_ZoomIn.UseVisualStyleBackColor = false;
            // 
            // btn_ZoomOut
            // 
            this.btn_ZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ZoomOut.BackColor = System.Drawing.Color.White;
            this.btn_ZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ZoomOut.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Bold);
            this.btn_ZoomOut.Location = new System.Drawing.Point(1, 249);
            this.btn_ZoomOut.Margin = new System.Windows.Forms.Padding(1);
            this.btn_ZoomOut.Name = "btn_ZoomOut";
            this.btn_ZoomOut.Size = new System.Drawing.Size(40, 40);
            this.btn_ZoomOut.TabIndex = 2;
            this.btn_ZoomOut.Text = "-";
            this.btn_ZoomOut.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btn_Expand);
            this.panel1.Controls.Add(this.TrendChart);
            this.panel1.Location = new System.Drawing.Point(250, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 290);
            this.panel1.TabIndex = 13;
            // 
            // btn_Expand
            // 
            this.btn_Expand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Expand.BackColor = System.Drawing.Color.White;
            this.btn_Expand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Expand.BackgroundImage")));
            this.btn_Expand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Expand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Expand.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.btn_Expand.Location = new System.Drawing.Point(1, 257);
            this.btn_Expand.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Expand.Name = "btn_Expand";
            this.btn_Expand.Size = new System.Drawing.Size(32, 32);
            this.btn_Expand.TabIndex = 2;
            this.btn_Expand.UseVisualStyleBackColor = false;
            this.btn_Expand.Click += new System.EventHandler(this.btn_Expand_Click);
            // 
            // TrendChart
            // 
            this.TrendChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrendChart.Location = new System.Drawing.Point(1, 1);
            this.TrendChart.Margin = new System.Windows.Forms.Padding(1);
            this.TrendChart.Name = "TrendChart";
            this.TrendChart.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.TrendChart.Size = new System.Drawing.Size(210, 288);
            this.TrendChart.TabIndex = 11;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 304F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 317);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1024, 317);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lsv_SelectedInfoSummary, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel12, 0, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(304, 317);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.LemonChiffon;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(302, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Summary";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lsv_SelectedInfoSummary
            // 
            this.lsv_SelectedInfoSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsv_SelectedInfoSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsv_SelectedInfoSummary.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsv_SelectedInfoSummary.GridLines = true;
            this.lsv_SelectedInfoSummary.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsv_SelectedInfoSummary.HideSelection = false;
            this.lsv_SelectedInfoSummary.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19});
            this.lsv_SelectedInfoSummary.Location = new System.Drawing.Point(1, 55);
            this.lsv_SelectedInfoSummary.Margin = new System.Windows.Forms.Padding(1);
            this.lsv_SelectedInfoSummary.MultiSelect = false;
            this.lsv_SelectedInfoSummary.Name = "lsv_SelectedInfoSummary";
            this.lsv_SelectedInfoSummary.Size = new System.Drawing.Size(302, 261);
            this.lsv_SelectedInfoSummary.TabIndex = 6;
            this.lsv_SelectedInfoSummary.UseCompatibleStateImageBehavior = false;
            this.lsv_SelectedInfoSummary.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Subject";
            this.columnHeader1.Width = 136;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 145;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel12.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.cmb_TitrationList, 1, 0);
            this.tableLayoutPanel12.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(304, 27);
            this.tableLayoutPanel12.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.Color.LemonChiffon;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1, 1);
            this.label7.Margin = new System.Windows.Forms.Padding(1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 25);
            this.label7.TabIndex = 1;
            this.label7.Text = "List";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmb_TitrationList
            // 
            this.cmb_TitrationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_TitrationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_TitrationList.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.cmb_TitrationList.FormattingEnabled = true;
            this.cmb_TitrationList.Location = new System.Drawing.Point(92, 1);
            this.cmb_TitrationList.Margin = new System.Windows.Forms.Padding(1);
            this.cmb_TitrationList.Name = "cmb_TitrationList";
            this.cmb_TitrationList.Size = new System.Drawing.Size(211, 25);
            this.cmb_TitrationList.TabIndex = 8;
            this.cmb_TitrationList.SelectedIndexChanged += new System.EventHandler(this.cmb_TitrationList_SelectedIndexChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.dgv_InjectionInfo, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(304, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(360, 317);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // dgv_InjectionInfo
            // 
            this.dgv_InjectionInfo.AllowUserToAddRows = false;
            this.dgv_InjectionInfo.AllowUserToDeleteRows = false;
            this.dgv_InjectionInfo.AllowUserToResizeColumns = false;
            this.dgv_InjectionInfo.AllowUserToResizeRows = false;
            this.dgv_InjectionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_InjectionInfo.ColumnHeadersHeight = 25;
            this.dgv_InjectionInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.col_Total,
            this.col_Analog});
            this.dgv_InjectionInfo.Location = new System.Drawing.Point(1, 28);
            this.dgv_InjectionInfo.Margin = new System.Windows.Forms.Padding(1);
            this.dgv_InjectionInfo.MultiSelect = false;
            this.dgv_InjectionInfo.Name = "dgv_InjectionInfo";
            this.dgv_InjectionInfo.ReadOnly = true;
            this.dgv_InjectionInfo.RowHeadersVisible = false;
            this.dgv_InjectionInfo.RowTemplate.Height = 23;
            this.dgv_InjectionInfo.Size = new System.Drawing.Size(358, 288);
            this.dgv_InjectionInfo.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "No";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "Time";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Inject";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 75;
            // 
            // col_Total
            // 
            this.col_Total.HeaderText = "Total";
            this.col_Total.Name = "col_Total";
            this.col_Total.ReadOnly = true;
            this.col_Total.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_Total.Width = 75;
            // 
            // col_Analog
            // 
            this.col_Analog.HeaderText = "Analog";
            this.col_Analog.Name = "col_Analog";
            this.col_Analog.ReadOnly = true;
            this.col_Analog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.col_Analog.Width = 75;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.LemonChiffon;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(358, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Injection";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.usrCtrl_TitrationGraph1, 0, 1);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(664, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(360, 317);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.LemonChiffon;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(358, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Graph";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // usrCtrl_TitrationGraph1
            // 
            this.usrCtrl_TitrationGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usrCtrl_TitrationGraph1.Location = new System.Drawing.Point(1, 28);
            this.usrCtrl_TitrationGraph1.Margin = new System.Windows.Forms.Padding(1);
            this.usrCtrl_TitrationGraph1.Name = "usrCtrl_TitrationGraph1";
            this.usrCtrl_TitrationGraph1.Size = new System.Drawing.Size(358, 288);
            this.usrCtrl_TitrationGraph1.TabIndex = 1;
            // 
            // Page_DataHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Consolas", 9F);
            this.Name = "Page_DataHistory";
            this.Size = new System.Drawing.Size(1024, 634);
            this.VisibleChanged += new System.EventHandler(this.Page_DataHistory_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Calendar.CalendarTimeProperties)).EndInit();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tbl_DataList_Or_Trend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OneDayList)).EndInit();
            this.tbl_Trend.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TrendChart)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InjectionInfo)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lsv_SelectedInfoSummary;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgv_InjectionInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label4;
        private Controls.UsrCtrl_TitrationGraph usrCtrl_TitrationGraph1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tbl_DataList_Or_Trend;
        private System.Windows.Forms.DataGridView dgv_OneDayList;
        private System.Windows.Forms.Button btn_DataList;
        private System.Windows.Forms.Button btn_Trend;
        private System.Windows.Forms.TableLayoutPanel tbl_Trend;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private ATIK.PrmCmp_Collection CmpCol_Recipe;
        private ATIK.PrmCmp_Collection CmpCol_Target;
        private DevExpress.XtraCharts.ChartControl TrendChart;
        private DevExpress.XtraEditors.Controls.CalendarControl Calendar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_TitrationList;
        private System.Windows.Forms.CheckBox chk_DayTrend;
        private System.Windows.Forms.CheckBox chk_MonthTrend;
        private System.Windows.Forms.CheckBox chk_PeriodTrend;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Button btn_SetPeriod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private ATIK.PrmCmp_Value CmpVal_Period_To;
        private ATIK.PrmCmp_Value CmpVal_Period_From;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.Button btn_ZoomIn;
        private System.Windows.Forms.Button btn_ZoomOut;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Expand;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_EndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Duration;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_RecipeInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Analog;
    }
}
