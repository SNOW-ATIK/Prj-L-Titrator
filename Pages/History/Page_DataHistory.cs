using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraEditors.Calendar;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraCharts;

using ATIK;
using L_Titrator.Pages.History;

namespace L_Titrator.Pages
{

    public partial class Page_DataHistory : UserControl, IPage
    {
        private enum TopViewList
        {
            None,
            DataList,
            Trend
        }

        public enum TrendType
        { 
            None,
            Day,
            Month,
            Period,
        }

        private TopViewList _SelectedTopViewItem;
        private TopViewList SelectedTopViewItem
        {
            get
            {
                return _SelectedTopViewItem;
            }
            set 
            {
                _SelectedTopViewItem = value;
                switch (_SelectedTopViewItem)
                {
                    case TopViewList.None:
                        btn_DataList.BackColor = Color.White;
                        btn_Trend.BackColor = Color.White;
                        tbl_DataList_Or_Trend.Visible = false;
                        break;

                    case TopViewList.DataList:
                        btn_DataList.BackColor = Color.MediumSeaGreen;
                        btn_Trend.BackColor = Color.White;

                        tbl_DataList_Or_Trend.ColumnStyles[0].Width = 100;
                        tbl_DataList_Or_Trend.ColumnStyles[1].Width = 0;

                        SelectedDataListIdx = -1;
                        tbl_DataList_Or_Trend.Visible = true;
                        break;

                    case TopViewList.Trend:
                        btn_DataList.BackColor = Color.White;
                        btn_Trend.BackColor = Color.MediumSeaGreen;

                        tbl_DataList_Or_Trend.ColumnStyles[0].Width = 0;
                        tbl_DataList_Or_Trend.ColumnStyles[1].Width = 100;
                        tbl_Trend.ColumnStyles[0].Width = 250;
                        tbl_DataList_Or_Trend.Visible = true;
                        break;
                }
            }
        }

        private TrendType SelectedTrendType;

        private List<HistoryObj> DayHistory = new List<HistoryObj>();
        //private Dictionary<int, List<HistoryObj>> MonthHistory = new Dictionary<int, List<HistoryObj>>();
        private List<HistoryObj> MonthHistory = new List<HistoryObj>();
        private List<HistoryObj> PeriodHistory = new List<HistoryObj>();
        private Dictionary<DateTime, double> TrendSource = new Dictionary<DateTime, double>();
        private (int CurYear, int CurMonth) CurYearMonthInfo = (0, 0);
        private int SelectedDataListIdx = -1;
        private HistoryObj SelectedHistory;

        public Page_DataHistory()
        {
            InitializeComponent();

            Calendar.DateTime = DateTime.Now;

            InitPage();
        }

        public void InitPage()
        {
            SelectedTopViewItem = TopViewList.DataList;

            dgv_OneDayList.Rows.Clear();
            for (int i = 0; i < lsv_SelectedInfoSummary.Items.Count; i++)
            {
                lsv_SelectedInfoSummary.Items[i].SubItems[1].Text = "";
            }
            dgv_InjectionInfo.Rows.Clear();
            usrCtrl_TitrationGraph1.ClearAll();

            InitCalendar();

            CmpVal_Period_From.Enabled = false;
            CmpVal_Period_To.Enabled = false;
            //calendarControl1.CellStyleProvider = new CustomCellStyleProvider();
        }

        private void InitCalendar()
        {
            Calendar.DateTime = DateTime.Now;

            ContextButton cb = new ContextButton()
            {
                Alignment = ContextItemAlignment.TopNear,
                Visibility = ContextItemVisibility.Visible,
                Enabled = false,
            };
            Calendar.ContextButtons.Add(cb);
            Calendar.ContextButtonCustomize += CalendarControl1_ContextButtonCustomize;
        }

        private void LoadDayHistory(DateTime selectedDay)
        {
            dgv_OneDayList.Rows.Clear();
            if (LT_History.LoadDaySummary(selectedDay.Year, selectedDay.Month, selectedDay.Day, out List<string> historyFiles) == false)
            {
                return;
            }

            DayHistory.Clear();
            for (int i = 0; i < historyFiles.Count; i++)
            {
                if (LT_History.LoadHistory(historyFiles[i], out HistoryObj history) == true)
                {
                    dgv_OneDayList.Rows.Add(i, history.StartTime_Sequence, history.EndTime_Sequence, history.Duration_Sequence, $"{history.RecipeNo}:{history.RecipeName}");
                    DayHistory.Add(history);
                }
                else
                {
                    dgv_OneDayList.Rows.Add(i, "Invalid", "", "", "");
                }
            }
            dgv_OneDayList.ClearSelection();
            SelectedTopViewItem = TopViewList.DataList;
        }

        private void calendarControl1_MouseUp(object sender, MouseEventArgs e)
        {
            CalendarHitInfo hitInfo = Calendar.GetHitInfo(e.Location);
            switch (hitInfo.HitTest)
            {
                case CalendarHitInfoType.Caption:
                    break;

                case CalendarHitInfoType.MonthNumber:
                    switch (Calendar.View)
                    {
                        case DateEditCalendarViewType.MonthInfo:
                            if (CurYearMonthInfo != (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month))
                            {
                                ClearAll();
                                SelectedDataListIdx = -1;

                                CurYearMonthInfo = (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month);
                            }

                            if (DataExist(hitInfo.Cell.Date) == true)
                            {
                                LoadDayHistory(hitInfo.Cell.Date);
                                SelectedDataListIdx = -1;
                            }
                            else
                            {
                                ClearAll();
                                SelectedDataListIdx = -1;
                            }
                            SelectedTopViewItem = TopViewList.DataList;
                            break;
                                                        
                        case DateEditCalendarViewType.YearInfo:
                            if (CurYearMonthInfo != (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month))
                            {
                                ClearAll();
                                SelectedDataListIdx = -1;

                                CurYearMonthInfo = (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month);
                            }
                            break;
                    }
                    break;
            }
        }

        private bool DataExist(DateTime dt)
        {
            if (LT_History.LoadDaySummary(dt.Year, dt.Month, dt.Day, out var historyInDay) == true)
            {
                return historyInDay.Count > 0;
            }
            return false;
        }

        private void dgv_OneDayList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedDayListItemChanged(e.RowIndex);
        }

        private void dgv_OneDayList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_OneDayList.SelectedRows.Count == 1)
            {
                SelectedDayListItemChanged(dgv_OneDayList.SelectedRows[0].Index);
            }
        }

        private void SelectedDayListItemChanged(int nRowIdx)
        {
            if (nRowIdx < 0)
            {
                return;
            }
            if (nRowIdx == SelectedDataListIdx)
            {
                return;
            }

            SelectedDataListIdx = nRowIdx;

            ClearBottom();

            if (dgv_OneDayList.Rows[nRowIdx].Cells[1].Value.GetType() != typeof(DateTime))
            {
                SelectedDataListIdx = -1;
                return;
            }

            DateTime startTime = (DateTime)dgv_OneDayList.Rows[nRowIdx].Cells[1].Value;
            var data = DayHistory.Where(history => history.StartTime_Sequence == startTime).ToList();
            if (data.Count == 1)
            {
                SelectedHistory = data[0];

                if (SelectedHistory.NoOfTitrations > 0)
                {
                    for (int i = 0; i < SelectedHistory.NoOfTitrations; i++)
                    {
                        string sectionName = $"{HistoryObj.Section.Titration}-{i}";
                        cmb_TitrationList.Items.Add(sectionName);
                    }
                    cmb_TitrationList.SelectedIndex = 0;
                }
            }
        }

        private void cmb_TitrationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nOrder = int.Parse(((string)cmb_TitrationList.SelectedItem).Split('-')[1]);
            if (SelectedHistory.Get_TitrationObj(nOrder, out HistoryObj.TitrationObj ttrObj) == true)
            {
                Load_SelectedData_Summary(SelectedHistory.StartTime_Sequence, ttrObj);
                if (ttrObj.Get_InjectedObjList(out var injObjs) == true)
                {
                    Add_SelectedData_InjectionList(injObjs);
                    Draw_SelectedData_TitrationGraph(ttrObj.SampleName, injObjs);
                }
            }
        }

        private void Load_SelectedData_Summary(DateTime startSeqTime, HistoryObj.TitrationObj ttrObj)
        {
            for (int i = 0; i < lsv_SelectedInfoSummary.Items.Count; i++)
            {
                lsv_SelectedInfoSummary.Items[i].SubItems[1].Text = "";
            }

            lsv_SelectedInfoSummary.Items[0].SubItems[1].Text = Util.GetStringFromDateTime(startSeqTime);
            lsv_SelectedInfoSummary.Items[1].SubItems[1].Text = Util.GetStringFromDateTime(ttrObj.EndTime_Titration);
            lsv_SelectedInfoSummary.Items[2].SubItems[1].Text = Util.GetStringFromTimeSpan(ttrObj.EndTime_Titration - startSeqTime);
            lsv_SelectedInfoSummary.Items[3].SubItems[1].Text = ttrObj.SampleName;
            if (ttrObj.Get_ProperResult(out var injObj) == true)
            {
                lsv_SelectedInfoSummary.Items[4].SubItems[1].Text = injObj.InjVol_Accum.ToString("0.00##") + " mL";

                double concentration = injObj.InjVol_Accum * ttrObj.ScaleFactor_VolumeToConcentration;
                lsv_SelectedInfoSummary.Items[5].SubItems[1].Text = concentration.ToString("0.0000");
            }
            else
            {
                lsv_SelectedInfoSummary.Items[4].SubItems[1].Text = "Invalid";
                lsv_SelectedInfoSummary.Items[5].SubItems[1].Text = "Invalid";
            }
            lsv_SelectedInfoSummary.Items[6].SubItems[1].Text = ttrObj.ReagentName;
            lsv_SelectedInfoSummary.Items[7].SubItems[1].Text = ttrObj.MaxIterationCount.ToString();
            lsv_SelectedInfoSummary.Items[8].SubItems[1].Text = ttrObj.Offset_mL.ToString("0.00") + " mL";
            lsv_SelectedInfoSummary.Items[9].SubItems[1].Text = ttrObj.MixingTime_AfterOffset_ms.ToString() + " s";
            lsv_SelectedInfoSummary.Items[10].SubItems[1].Text = ttrObj.MixingTime_General_ms.ToString() + " s";
            lsv_SelectedInfoSummary.Items[11].SubItems[1].Text = ttrObj.ScaleFactor_VolumeToConcentration.ToString("0.0000");
            lsv_SelectedInfoSummary.Items[12].SubItems[1].Text = ttrObj.AnalogValue_Target.ToString() + " mV";
            lsv_SelectedInfoSummary.Items[13].SubItems[1].Text = ttrObj.AnalogValue_End.ToString() + " mV";
            lsv_SelectedInfoSummary.Items[14].SubItems[1].Text = ttrObj.IncThr_ToMiddle.ToString() + " mV";
            lsv_SelectedInfoSummary.Items[15].SubItems[1].Text = ttrObj.IncThr_ToSmall.ToString() + " mV";
            lsv_SelectedInfoSummary.Items[16].SubItems[1].Text = ttrObj.Inc_Large_mL.ToString() + " mL";
            lsv_SelectedInfoSummary.Items[17].SubItems[1].Text = ttrObj.Inc_Middle_mL.ToString() + " mL";
            lsv_SelectedInfoSummary.Items[18].SubItems[1].Text = ttrObj.Inc_Small_mL.ToString() + " mL";
        }

        public void Add_SelectedData_InjectionList(List<HistoryObj.TitrationObj.InjectedObj> injObjs)
        {
            dgv_InjectionInfo.Rows.Clear();

            for (int i = 0; i < injObjs.Count; i++)
            {
                var data = injObjs[i];
                dgv_InjectionInfo.Rows.Add(i, data.Time.ToString("HH:mm:ss"), data.InjVol_Single.ToString("0.00##"), data.InjVol_Accum.ToString("0.00#"), data.Analog);
            }
        }

        public void Draw_SelectedData_TitrationGraph(string sampleName, List<HistoryObj.TitrationObj.InjectedObj> injObjs)
        {
            usrCtrl_TitrationGraph1.ClearAll();

            usrCtrl_TitrationGraph1.Init_Background(sampleName, false);
            for (int i = 0; i < injObjs.Count; i++)
            {
                usrCtrl_TitrationGraph1.AddPoint(sampleName, injObjs[i].InjVol_Accum, injObjs[i].Analog);
            }
        }

        private void ClearAll()
        {
            DayHistory.Clear();
            MonthHistory.Clear();
            PeriodHistory.Clear();
            dgv_OneDayList.Rows.Clear();

            ClearBottom();
        }

        private void ClearBottom()
        {
            cmb_TitrationList.Items.Clear();
            for (int i = 0; i < lsv_SelectedInfoSummary.Items.Count; i++)
            {
                lsv_SelectedInfoSummary.Items[i].SubItems[1].Text = "";
            }
            dgv_InjectionInfo.Rows.Clear();
            usrCtrl_TitrationGraph1.ClearAll();
        }

        private void btn_DataList_Click(object sender, EventArgs e)
        {
            SelectedTopViewItem = TopViewList.DataList;
        }

        private void btn_Trend_Click(object sender, EventArgs e)
        {
            SelectedTopViewItem = TopViewList.Trend;

            ClearChart();

            chk_DayTrend.Checked = false;
            chk_MonthTrend.Checked = false;
            chk_MonthTrend.Checked = false;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void ShowSubPage(string subPageName)
        {
            throw new NotImplementedException();
        }

        public void PagingNext()
        {
            throw new NotImplementedException();
        }

        public void PagingPrev()
        {
            throw new NotImplementedException();
        }

        private void btn_TrendDay_Click(object sender, EventArgs e)
        {
        }

        private void CmpCol_Recipe_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            string selectedRcpName = (string)changedValue;
            List<string> sampleInDay = new List<string>();
            List<HistoryObj> historySource;
            if (chk_DayTrend.Checked == true)
            {
                historySource = DayHistory;
            }
            else if (chk_MonthTrend.Checked == true)
            {
                historySource = MonthHistory;
            }
            else if (chk_PeriodTrend.Checked == true)
            {
                historySource = PeriodHistory;
            }
            else
            {
                return;
            }
            historySource.ForEach(history => 
            {
                string item = $"{history.RecipeNo}:{history.RecipeName}";
                if (item == selectedRcpName)
                {
                    if (history.NoOfTitrations > 0)
                    {
                        for (int i = 0; i < history.NoOfTitrations; i++)
                        {
                            if (history.Get_TitrationObj(i, out var ttrObj) == true)
                            {
                                if (sampleInDay.Contains(ttrObj.SampleName) == false)
                                {
                                    sampleInDay.Add(ttrObj.SampleName);
                                }
                            }
                        }
                    }
                }
            });

            string initValue = sampleInDay.Count == 1 ? sampleInDay[0] : null;
            CmpCol_Target.Init_WithOutGenPrm("Sample", sampleInDay.ToArray(), initValue);

        }

        private void CmpCol_Target_SelectedUserItemChangedEvent(object sender, object changedValue)
        {
            string[] rcpInfo = ((string)CmpCol_Recipe.Prm_Value).Split(':');
            int rcpNo = int.Parse(rcpInfo[0]);
            string rcpName = rcpInfo[1];
            string sampleName = (string)changedValue;
            List<HistoryObj> selectedHistorys = new List<HistoryObj>();
            List<HistoryObj> historySource;
            if (chk_DayTrend.Checked == true)
            {
                historySource = DayHistory;
            }
            else if (chk_MonthTrend.Checked == true)
            {
                historySource = MonthHistory;
            }
            else if (chk_PeriodTrend.Checked == true)
            {
                historySource = PeriodHistory;
            }
            else
            {
                return;
            }
            historySource.ForEach(history =>
            {
                if (history.RecipeNo == rcpNo && history.RecipeName == rcpName)
                {
                    if (history.NoOfTitrations > 0)
                    {
                        for (int i = 0; i < history.NoOfTitrations; i++)
                        {
                            if (history.Get_TitrationObj(i, out var ttrObj) == true)
                            {
                                if (ttrObj.SampleName == sampleName)
                                {
                                    selectedHistorys.Add(history);
                                }
                            }
                        }
                    }
                }
            });

            if (selectedHistorys.Count == 0)
            {
                return;
            }

            InitChart(sampleName);

            TrendSource.Clear();
            for (int i = 0; i < selectedHistorys.Count; i++)
            {
                for (int j = 0; j < selectedHistorys[i].NoOfTitrations; j++)
                {
                    if (selectedHistorys[i].Get_TitrationObj(j, out var ttrObj) == true)
                    {
                        if (ttrObj.SampleName == sampleName)
                        {
                            if (ttrObj.Get_ProperResult(out var injObj) == true)
                            {
                                double value = injObj.InjVol_Accum * ttrObj.ScaleFactor_VolumeToConcentration;
                                TrendChart.Series[sampleName].Points.AddPoint(selectedHistorys[i].StartTime_Sequence, value);

                                TrendSource.Add(selectedHistorys[i].StartTime_Sequence, value);
                            }
                        }
                    }
                }
            }

            if (TrendSource.Count == 0)
            {
                MsgFrm_NotifyOnly frm = new MsgFrm_NotifyOnly("Valid results are not found.\r\nCheck data in DATA LIST.");
                frm.ShowDialog();
                return;
            }

            var series = TrendChart.Series[sampleName];
            double dMinVal = TrendSource.Values.Min() * 0.9;
            double dMaxVal = TrendSource.Values.Max() * 1.1;
            var diagram = (XYDiagram)TrendChart.Diagram;
            diagram.AxisY.WholeRange.SetMinMaxValues(dMinVal, dMaxVal);
        }

        private void Load_DayTrend()
        {
            CmpCol_Recipe.Init_WithOutGenPrm("Recipe", null);
            CmpCol_Target.Init_WithOutGenPrm("Sample", null);

            if (DayHistory.Count == 0)
            {
                SelectedTrendType = TrendType.None;
                return;
            }
            SelectedTrendType = TrendType.Day;

            List<string> rcpInDay = new List<string>();
            DayHistory.ForEach(history =>
            {
                if (history.NoOfTitrations > 0)
                {
                    string item = $"{history.RecipeNo}:{history.RecipeName}";
                    if (rcpInDay.Contains(item) == false)
                    {
                        rcpInDay.Add(item);
                    }
                }
            });

            string initValue = rcpInDay.Count == 1 ? rcpInDay[0] : null;
            CmpCol_Recipe.Init_WithOutGenPrm("Recipe", rcpInDay.ToArray(), initValue);
        }

        private void Load_MonthTrend()
        {
            CmpCol_Recipe.Init_WithOutGenPrm("Recipe", null);
            CmpCol_Target.Init_WithOutGenPrm("Sample", null);

            int year = CurYearMonthInfo.CurYear;
            int month = CurYearMonthInfo.CurMonth;

            LT_History.LoadMonth(year, month).Values.ToList().ForEach(historys => MonthHistory.AddRange(historys));
            if (MonthHistory.Count == 0)
            {
                return;
            }
            SelectedTrendType = TrendType.Month;

            List<string> rcpInMonth = new List<string>();
            MonthHistory.ForEach(history =>
            {
                if (history.NoOfTitrations > 0)
                {
                    string item = $"{history.RecipeNo}:{history.RecipeName}";
                    if (rcpInMonth.Contains(item) == false)
                    {
                        rcpInMonth.Add(item);
                    }
                }
            });

            string initValue = rcpInMonth.Count == 1 ? rcpInMonth[0] : null;
            CmpCol_Recipe.Init_WithOutGenPrm("Recipe", rcpInMonth.ToArray(), initValue);
        }

        private void chk_DayTrend_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DayTrend.Checked == true)
            {
                chk_MonthTrend.Checked = !chk_DayTrend.Checked;
                chk_PeriodTrend.Checked = !chk_DayTrend.Checked;

                Load_DayTrend();
            }
        }

        private void chk_MonthTrend_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_MonthTrend.Checked == true)
            {
                chk_DayTrend.Checked = !chk_MonthTrend.Checked;
                chk_PeriodTrend.Checked = !chk_MonthTrend.Checked;

                Load_MonthTrend();
            }
        }

        private void chk_PeriodTrend_CheckedChanged(object sender, EventArgs e)
        {
            CmpVal_Period_From.Enabled = chk_PeriodTrend.Checked;
            CmpVal_Period_To.Enabled = chk_PeriodTrend.Checked;
            btn_SetPeriod.Enabled = chk_PeriodTrend.Checked;

            if (chk_PeriodTrend.Checked == true)
            {
                chk_DayTrend.Checked = !chk_PeriodTrend.Checked;
                chk_MonthTrend.Checked = !chk_PeriodTrend.Checked;

                CmpCol_Recipe.Init_WithOutGenPrm("Recipe", null);
                CmpCol_Target.Init_WithOutGenPrm("Sample", null);
            }
        }

        private void CmpVal_Period_From_ValueClickedEvent(object sender, object oldValue)
        {
            Frm_ModifyDate frm = new Frm_ModifyDate("From", DateTime.Now.AddDays(-1), true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CmpVal_Period_From.Prm_Value = frm.NewValue;
            }
        }

        private void CmpVal_Period_To_ValueClickedEvent(object sender, object oldValue)
        {
            Frm_ModifyDate frm = new Frm_ModifyDate("To", DateTime.Now, true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CmpVal_Period_To.Prm_Value = frm.NewValue;
            }
        }

        private void btn_SetPeriod_Click(object sender, EventArgs e)
        {
            if (DateTime.TryParse((string)CmpVal_Period_From.Prm_Value, out DateTime dtFrom) == false)
            {
                SelectedTrendType = TrendType.None;
                MsgFrm_NotifyOnly frm = new MsgFrm_NotifyOnly("Invalid Format.");
                frm.ShowDialog();
                return;
            }
            if (DateTime.TryParse((string)CmpVal_Period_To.Prm_Value, out DateTime dtTo) == false)
            {
                SelectedTrendType = TrendType.None;
                MsgFrm_NotifyOnly frm = new MsgFrm_NotifyOnly("Invalid Format.");
                frm.ShowDialog();
                return;
            }
            TimeSpan tsPeriod = dtTo - dtFrom;
            if (tsPeriod.TotalSeconds < 0)
            {
                SelectedTrendType = TrendType.None;
                MsgFrm_NotifyOnly frm = new MsgFrm_NotifyOnly("Invalid Period.");
                frm.ShowDialog();
                return;
            }

            PeriodHistory.Clear();

            int totalDays = (int)(tsPeriod.TotalDays + 1);

            for (int i = 0; i < totalDays; i++)
            {
                DateTime dt = dtFrom.AddDays(i);
                if (LT_History.LoadDaySummary(dt.Year, dt.Month, dt.Day, out var files) == true)
                {
                    for (int j = 0; j < files.Count; j++)
                    {
                        string[] parsed = files[j].Split('\\');
                        string fileName = parsed[parsed.Length - 1].Split('.')[0];
                        DateTime dtCur = DateTime.ParseExact(fileName, "yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtCur >= dtFrom && dtCur <= dtTo)
                        {
                            if (LT_History.LoadHistory(files[j], out var history) == true)
                            {
                                PeriodHistory.Add(history);
                            }
                        }
                    }
                }
            }

            if (PeriodHistory.Count == 0)
            {
                SelectedTrendType = TrendType.None;
                return;
            }
            SelectedTrendType = TrendType.Period;

            List<string> rcpInPeriod = new List<string>();
            PeriodHistory.ForEach(history =>
            {
                if (history.NoOfTitrations > 0)
                {
                    string item = $"{history.RecipeNo}:{history.RecipeName}";
                    if (rcpInPeriod.Contains(item) == false)
                    {
                        rcpInPeriod.Add(item);
                    }
                }
            });

            string initValue = rcpInPeriod.Count == 1 ? rcpInPeriod[0] : null;
            CmpCol_Recipe.Init_WithOutGenPrm("Recipe", rcpInPeriod.ToArray(), initValue);
        }

        private void ClearChart()
        {
            TrendChart.Series.Clear();
            TrendChart.ClearCache();
        }

        private void InitChart(string sampleName)
        {
            ClearChart();

            TrendChart.Series.Add(sampleName, DevExpress.XtraCharts.ViewType.Line);
            TrendChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            TrendChart.Series[sampleName].ArgumentScaleType = ScaleType.DateTime;
            TrendChart.Series[sampleName].ValueScaleType = ScaleType.Numerical;
            ((LineSeriesView)TrendChart.Series[sampleName].View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((LineSeriesView)TrendChart.Series[sampleName].View).LineMarkerOptions.Size = 5;
            XYDiagram xyDiagram = (XYDiagram)TrendChart.Diagram;
            xyDiagram.EnableAxisXZooming = true;
            xyDiagram.EnableAxisYZooming = true;
            xyDiagram.ZoomingOptions.UseKeyboard = true;
            xyDiagram.ZoomingOptions.UseKeyboardWithMouse = true;
            xyDiagram.ZoomingOptions.UseMouseWheel = true;
            xyDiagram.ZoomingOptions.UseTouchDevice = true;

            xyDiagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
            xyDiagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            xyDiagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
            xyDiagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;

            if (chk_DayTrend.Checked == true)
            {
                xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                xyDiagram.AxisX.Label.TextPattern = "{A:MMM-dd HH}";
            }
            else if (chk_MonthTrend.Checked == true)
            {
                xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                xyDiagram.AxisX.Label.TextPattern = "{A:MMM-dd}";
            }
            else if (chk_PeriodTrend.Checked == true)
            {
                xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                xyDiagram.AxisX.Label.TextPattern = "{A:MMM-dd}";
            }

            ((XYDiagram)TrendChart.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)TrendChart.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)TrendChart.Diagram).AxisX.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
            ((XYDiagram)TrendChart.Diagram).AxisY.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
            ((XYDiagram)TrendChart.Diagram).AxisX.Title.Text = "Time";
            ((XYDiagram)TrendChart.Diagram).AxisY.Title.Text = "Concentration";
        }

        private void btn_Expand_Click(object sender, EventArgs e)
        {
            if (TrendSource.Count == 0)
            {
                return;
            }

            Frm_TrendExpand frm = new Frm_TrendExpand();
            //frm.Location = new Point(0, 91);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Init((string)CmpCol_Target.Prm_Value, SelectedTrendType);
            frm.DrawGraph((string)CmpCol_Target.Prm_Value, TrendSource);
            frm.ShowDialog();
        }

        private void CmpVal_ModifyPeriod_EnabledChanged(object sender, EventArgs e)
        {
            PrmCmp_Value ctrl = (PrmCmp_Value)sender;
            ctrl.Color_Name = ctrl.Enabled == true ? Color.LemonChiffon : Color.DarkGray;
        }

        private void CalendarControl1_ContextButtonCustomize(object sender, CalendarContextButtonCustomizeEventArgs e)
        {
            if (Calendar.View == DateEditCalendarViewType.MonthInfo)
            {
                if (DataExist(e.Cell.Date) == true)
                {
                    e.Item.Glyph = Properties.Resources.Apply16;
                }
                else
                {
                    //e.Item.Glyph = Properties.Resources.Cancel16;
                }
            }
        }

        public class CustomCellStyleProvider : ICalendarCellStyleProvider
        {
            public void UpdateAppearance(CalendarCellStyle cell)
            {
                cell.Appearance.ForeColor = Color.Black;
                cell.Appearance.Font = new Font("Tahoma", 9f);
                if (cell.Active)
                {
                    if (cell.Date.Day % 2 == 0)
                    {
                        cell.Appearance.BackColor = Color.LightGray;
                    }
                    else
                    {
                        cell.Appearance.BackColor = Color.LightBlue;
                    }
                    //if (DataExist(cell.Date) == true)
                    //{

                    //}
                    //else
                    //{
                    //    cell.Appearance.BackColor = Color.White;
                    //}                    
                }
                else
                {
                    cell.Appearance.BackColor = Color.DarkGray;
                }
            }
        }

        private void Page_DataHistory_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                ClearAll();
                Calendar.Refresh();

                CmpVal_Period_From.Prm_Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                CmpVal_Period_To.Prm_Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                SelectedTopViewItem = TopViewList.None;
            }
        }
    }
}
