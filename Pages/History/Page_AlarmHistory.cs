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

namespace L_Titrator.Pages
{
    public partial class Page_AlarmHistory : UserControl, IPage
    {
        private (int CurYear, int CurMonth) CurYearMonthInfo = (0, 0);
        private System.Windows.Forms.Timer tmr_UpdateCurrentAlarmState;

        private Dictionary<AlarmName, DataGridViewRow> DicAlarmStatus = new Dictionary<AlarmName, DataGridViewRow>();

        public Page_AlarmHistory()
        {
            InitializeComponent();

            InitCalendar();
            InitAlarmStatus();
            InitAlarmStatusTimer();
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

        private void InitAlarmStatus()
        {
            DicAlarmStatus.Clear();
            dgv_AlarmStatus.Rows.Clear();

            var allAlarms = LT_Alarm.GetAllAlarms();
            for (int i = 0; i < allAlarms.Count; i++)
            {
                dgv_AlarmStatus.Rows.Add(allAlarms[i].Name, "");
                DicAlarmStatus.Add(allAlarms[i].Name, dgv_AlarmStatus.Rows[i]);
            }
        }

        private void InitAlarmStatusTimer()
        {
            tmr_UpdateCurrentAlarmState = new Timer();
            tmr_UpdateCurrentAlarmState.Enabled = false;
            tmr_UpdateCurrentAlarmState.Interval = 1000;
            tmr_UpdateCurrentAlarmState.Tick += Tmr_UpdateCurrentAlarmState_Tick;
        }

        private void Tmr_UpdateCurrentAlarmState_Tick(object sender, EventArgs e)
        {
            //if (dgv_AlarmStatus.RowCount == 0)
            //{
            //    return;
            //}

            //var allAlarms = LT_Alarm.GetAllAlarms();
            //for (int i = 0; i < allAlarms.Count; i++)
            //{
            //    if (allAlarms[i].IsSet == true)
            //    {
            //        switch (allAlarms[i].Gen_Level.Value)
            //        {
            //            case AlarmLevel.NoUse:
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Value = "NoUse";
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Style = new DataGridViewCellStyle() { BackColor = Color.DarkGray };
            //                break;

            //            case AlarmLevel.Notify:
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Value = "Notify";
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Style = new DataGridViewCellStyle() { BackColor = Color.LightGray };
            //                break;

            //            case AlarmLevel.Warning:
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Value = "Warning";
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Style = new DataGridViewCellStyle() { BackColor = Color.DarkOrange };
            //                break;

            //            case AlarmLevel.Critical:
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Value = "Critical";
            //                DicAlarmStatus[allAlarms[i].Name].Cells[1].Style = new DataGridViewCellStyle() { BackColor = Color.Crimson };
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        DicAlarmStatus[allAlarms[i].Name].Cells[1].Value = "";
            //        DicAlarmStatus[allAlarms[i].Name].Cells[1].Style = new DataGridViewCellStyle() { BackColor = Color.White };
            //    }
            //}
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
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

        private void Calendar_MouseUp(object sender, MouseEventArgs e)
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
                            ClearAll();

                            if (CurYearMonthInfo != (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month))
                            {
                                CurYearMonthInfo = (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month);
                            }

                            if (LT_Alarm.ReadDayHistorys(hitInfo.Cell.Date, out var dayHistorys) == true)
                            {
                                for (int i = 0; i < dayHistorys.Count; i++)
                                {
                                    var alarmObj = dayHistorys[i];
                                    dgv_OneDayList.Rows.Add(alarmObj.OccurTime, alarmObj.Name, alarmObj.sCode, alarmObj.eLevel, alarmObj.Description);
                                }
                            }
                            break;

                        case DateEditCalendarViewType.YearInfo:
                            if (CurYearMonthInfo != (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month))
                            {
                                ClearAll();

                                CurYearMonthInfo = (hitInfo.Cell.Date.Year, hitInfo.Cell.Date.Month);
                            }
                            break;
                    }
                    break;
            }
        }

        private void ClearAll()
        {
            dgv_OneDayList.Rows.Clear();
        }

        private bool DataExist(DateTime dt)
        {
            if (LT_Alarm.ReadDayHistorys(dt, out var alarmObjs) == true)
            {
                return alarmObjs.Count > 0;
            }
            return false;
        }

        private void Page_AlarmHistory_VisibleChanged(object sender, EventArgs e)
        {
            tmr_UpdateCurrentAlarmState.Enabled = this.Visible;

            if (this.Visible == true)
            {
                ClearAll();
                Calendar.Refresh();

                InitAlarmStatus();
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

    }
}
