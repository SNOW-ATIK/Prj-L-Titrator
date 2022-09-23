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

namespace L_Titrator.Pages
{

    public partial class Page_Data : UserControl, IPage
    {
        private enum TopViewList
        {
            None,
            DataList,
            Trend
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

                        tbl_DataList_Or_Trend.Visible = true;
                        tbl_DataList_Or_Trend.ColumnStyles[0].Width = 100;
                        tbl_DataList_Or_Trend.ColumnStyles[1].Width = 0;

                        SelectedDataListIdx = -1;
                        break;

                    case TopViewList.Trend:
                        btn_DataList.BackColor = Color.White;
                        btn_Trend.BackColor = Color.MediumSeaGreen;

                        tbl_DataList_Or_Trend.Visible = true;
                        tbl_DataList_Or_Trend.ColumnStyles[0].Width = 0;
                        tbl_DataList_Or_Trend.ColumnStyles[1].Width = 100;
                        break;
                }
            }
        }

        private Dictionary<int, List<HistoryObj>> MonthHistory = new Dictionary<int, List<HistoryObj>>();
        private (int CurYear, int CurMonth) CurDateInfo = (0, 0);
        private int SelectedDataListIdx = -1;
        private HistoryObj SelectedHistory;

        public Page_Data()
        {
            InitializeComponent();

            Calendar.DateTime = DateTime.Now;

            LoadMonthHistory();

            InitPage();
        }

        public void InitPage()
        {
            SelectedTopViewItem = TopViewList.None;

            dgv_OneDayList.Rows.Clear();
            for (int i = 0; i < lsv_SelectedInfoSummary.Items.Count; i++)
            {
                lsv_SelectedInfoSummary.Items[i].SubItems[1].Text = "";
            }
            dgv_InjectionInfo.Rows.Clear();
            usrCtrl_TitrationGraph1.ClearAll();

            ContextButton cb = new ContextButton()
            {
                Alignment = ContextItemAlignment.TopNear,
                Visibility = ContextItemVisibility.Visible,
                Enabled = false,
            };
            Calendar.ContextButtons.Add(cb);
            Calendar.ContextButtonCustomize += CalendarControl1_ContextButtonCustomize;

            //calendarControl1.CellStyleProvider = new CustomCellStyleProvider();
        }

        public void LoadMonthHistory()
        {
            int nThisYear = Calendar.DateTime.Year;
            int nThisMonth = Calendar.DateTime.Month;

            MonthHistory.Clear();
            MonthHistory = LT_History.LoadMonth(nThisYear, nThisMonth);
        }

        private void LoadDayHistory(DateTime selectedDay)
        {
            dgv_OneDayList.Rows.Clear();

            if (MonthHistory.ContainsKey(selectedDay.Day) == false)
            {
                return;
            }

            var oneDayList = MonthHistory[selectedDay.Day];
            if (oneDayList.Count > 0)
            {
                for (int i = 0; i < oneDayList.Count; i++)
                {
                    HistoryObj data = oneDayList[i];
                    dgv_OneDayList.Rows.Add(i, data.StartTime_Sequence, data.EndTime_Sequence, data.Duration_Sequence, $"{data.RecipeNo}:{data.RecipeName}");
                }
                dgv_OneDayList.ClearSelection();
                SelectedTopViewItem = TopViewList.DataList;
            }
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
                            if (CurDateInfo != (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month))
                            {
                                LoadMonthHistory();
                                ClearBottom();
                                SelectedDataListIdx = -1;
                            }

                            SelectedTopViewItem = TopViewList.None;
                            if (DataExist(hitInfo.Cell.Date) == true)
                            {
                                LoadDayHistory(hitInfo.Cell.Date);

                                btn_DataList.Enabled = true;
                                btn_Trend.Enabled = true;
                            }
                            else
                            {
                                ClearAll();
                                btn_DataList.Enabled = false;
                                btn_Trend.Enabled = false;
                            }
                            break;
                                                        
                        case DateEditCalendarViewType.YearInfo:
                            if (CurDateInfo != (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month))
                            {
                                LoadMonthHistory();
                            }
                            break;
                    }
                    break;
            }
        }

        private bool DataExist(DateTime dt)
        {
            if (LT_History.LoadDay(dt.Year, dt.Month, dt.Day, out var historyInDay) == true)
            {
                return historyInDay.Count > 0;
            }
            return false;
        }

        private void dgv_OneDayList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRowIdx = e.RowIndex;
            if (nRowIdx < 0)
            {
                return;
            }
            if (nRowIdx == SelectedDataListIdx)
            {
                return;
            }

            SelectedDataListIdx = nRowIdx;

            cmb_TitrationList.Items.Clear();

            DateTime startTime = (DateTime)dgv_OneDayList.Rows[nRowIdx].Cells[1].Value;
            var data = MonthHistory[startTime.Day].Where(history => history.StartTime_Sequence == startTime).ToList();
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
                Load_SelectedData_Summary(ttrObj);
                if (ttrObj.Get_InjectedObjList(out var injObjs) == true)
                {
                    Add_SelectedData_InjectionList(injObjs);
                    Draw_SelectedData_TitrationGraph(ttrObj.SampleName, injObjs);
                }
            }
        }

        private void Load_SelectedData_Summary(HistoryObj.TitrationObj ttrObj)
        {
            for (int i = 0; i < lsv_SelectedInfoSummary.Items.Count; i++)
            {
                lsv_SelectedInfoSummary.Items[i].SubItems[1].Text = "";
            }

            lsv_SelectedInfoSummary.Items[0].SubItems[1].Text = Util.GetStringFromDateTime(ttrObj.StartTime_Titration);
            lsv_SelectedInfoSummary.Items[1].SubItems[1].Text = Util.GetStringFromDateTime(ttrObj.EndTime_Titration);
            lsv_SelectedInfoSummary.Items[2].SubItems[1].Text = Util.GetStringFromTimeSpan(ttrObj.Duration_Titration);
            lsv_SelectedInfoSummary.Items[3].SubItems[1].Text = ttrObj.SampleName;
            if (ttrObj.Get_ProperResult(out var injObj) == true)
            {
                double value = injObj.InjVol_Accum * ttrObj.ScaleFactor_VolumeToConcentration;
                lsv_SelectedInfoSummary.Items[4].SubItems[1].Text = injObj.InjVol_Accum.ToString("0.00") + " mL";
                lsv_SelectedInfoSummary.Items[5].SubItems[1].Text = value.ToString("0.000");
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
                dgv_InjectionInfo.Rows.Add(i, data.Time.ToString("HH:mm:ss"), data.InjVol_Single, data.InjVol_Accum, data.Analog);
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
            dgv_OneDayList.Rows.Clear();
            cmb_TitrationList.Items.Clear();
            for (int i = 0; i < lsv_SelectedInfoSummary.Items.Count; i++)
            {
                lsv_SelectedInfoSummary.Items[i].SubItems[1].Text = "";
            }
            dgv_InjectionInfo.Rows.Clear();
            usrCtrl_TitrationGraph1.ClearAll();
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
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
            if (visible == true)
            {
                ClearAll();
                Calendar.Refresh();
                LoadMonthHistory();
            }
            else
            { 
            }
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
    }
}
