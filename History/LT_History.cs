using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

using ATIK;

namespace L_Titrator
{
    public static class LT_History
    {
        public static List<HistoryObj> LoadAll()
        {
            List<HistoryObj> allHistory = new List<HistoryObj>();
            return allHistory;
        }

        public static Dictionary<int, List<string>> LoadMonthSummary(int year, int month)
        {
            Dictionary<int, List<string>> monthHistorySummary = new Dictionary<int, List<string>>();
            string monthPath = Path.Combine(PreDef.Path_History_Data, year.ToString("0000"), month.ToString("00"));
            for (int i = 1; i < DateTime.DaysInMonth(year, month); i++)
            {
                string dayPath = Path.Combine(monthPath, i.ToString("00"));
                if (LoadDaySummary(year, month, i, out List<string> dayHistorys) == true)
                {
                    monthHistorySummary.Add(i, dayHistorys);
                }

            }
            return monthHistorySummary;
        }

        public static Dictionary<int, List<HistoryObj>> LoadMonth(int year, int month)
        {
            if (year <= 0 || month <= 0 || month > 12)
            {
                return null;
            }
            Dictionary<int, List<HistoryObj>> monthHistory = new Dictionary<int, List<HistoryObj>>();
            string monthPath = Path.Combine(PreDef.Path_History_Data, year.ToString("0000"), month.ToString("00"));            
            for (int i = 1; i < DateTime.DaysInMonth(year, month); i++)
            {
                string dayPath = Path.Combine(monthPath, i.ToString("00"));                
                if (LoadDay(dayPath, out List<HistoryObj> dayHistorys) == true)
                {
                    monthHistory.Add(i, dayHistorys);
                }

            }
            return monthHistory;
        }

        public static bool LoadDaySummary(int year, int month, int day, out List<string> historySummaryInDay)
        {
            historySummaryInDay = new List<string>();
            string FilePath = Path.Combine(PreDef.Path_History_Data, year.ToString("00"), month.ToString("00"), day.ToString("00"));
            if (Directory.Exists(FilePath) == false)
            {
                return false;
            }

            historySummaryInDay = Directory.GetFiles(FilePath).Where(file => file.EndsWith("log")).ToList();
            return true;
        }

        public static bool LoadDay(string dayPath, out List<HistoryObj> dayHistorys)
        {
            dayHistorys = new List<HistoryObj>();
            if (Directory.Exists(dayPath) == false)
            {
                return false;
            }

            List<string> fileInDay = Directory.GetFiles(dayPath).ToList();
            for (int i = 0; i < fileInDay.Count; i++)
            {
                try
                {
                    if (fileInDay[i].EndsWith("log") == false)
                    {
                        continue;
                    }
                    HistoryObj history = new HistoryObj(fileInDay[i]);
                    dayHistorys.Add(history);
                }
                catch
                { 
                }
            }

            return true;
        }

        public static bool LoadDay(int year, int month, int day, out List<HistoryObj> historyInDay)
        {
            string FilePath = Path.Combine(PreDef.Path_History_Data, year.ToString("00"), month.ToString("00"), day.ToString("00"));
            if (LoadDay(FilePath, out historyInDay) == true)
            {
                return true;
            }

            return false;
        }

        public static bool LoadHistory(string fileName, out HistoryObj history)
        {
            history = null;
            if (File.Exists(fileName) == false)
            {
                return false;
            }
            if (fileName.EndsWith("log") == false)
            {
                return false;
            }

            try
            {
                history = new HistoryObj(fileName);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
