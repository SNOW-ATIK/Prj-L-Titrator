using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using ATIK;

namespace L_Titrator
{
    public enum AlarmName
    {
        None,
        Com_Internal,
        Com_External,
        Leak,
        OverFlow,
        LifeTimeExpired,
        Syringe1,
        Syringe2,
        OutOfRange_Low,
        OutOfRange_High,
    }

    public enum AlarmMsg
    { 
        Set,
        Reset,
        Clear,
        TestMode_Enable,
        TestMode_Set,
        TestMode_Reset,
    }

    public enum AlarmLevel
    { 
        NoUse,
        Notify,
        Warning,
        Critical,
    }

    public class LT_Alarm
    {
        public static string[] AlarmCodes = new string[] { "0x00000001", "0x00000002", "0x00000004", "0x00000008",
                                                           "0x00000010", "0x00000020", "0x00000040", "0x00000080",
                                                           "0x00000100", "0x00000200", "0x00000400", "0x00000800",
                                                           "0x00001000", "0x00002000", "0x00004000", "0x00008000",
                                                           "0x00010000", "0x00020000", "0x00040000", "0x00080000",
                                                           "0x00100000", "0x00200000", "0x00400000", "0x00800000",
                                                           "0x01000000", "0x02000000", "0x04000000", "0x08000000",
                                                           "0x10000000", "0x20000000", "0x40000000", "0x80000000"};

        public delegate void AlarmUpdatedDelegate();
        public static AlarmUpdatedDelegate AlarmUpdateEvent;

        private static XmlCfgPrm Cfg_LT;

        private static object objLock_AccessFile = new object();
        private static List<AlarmObject> AllAlarms = new List<AlarmObject>();
        private static object objLock_OnAlarmList = new object();
        private static List<AlarmObject> OnAlarmList = new List<AlarmObject>();

        private static Thread SetAlarmThread;
        private static ConcurrentQueue<object> qSetAlarm = new ConcurrentQueue<object>();

        private static List<string> AlarmString_Prv = new List<string>();
        private static List<string> AlarmString_Cur = new List<string>();

        public static bool Load()
        {
            Cfg_LT = new XmlCfgPrm("Config", "AlarmList.xml", "AlarmList");
            if (Cfg_LT.XmlLoaded == false)
            {
                return false;
            }

            int noOfAlarms = int.Parse(Cfg_LT.Get_Item("NoOfAlarms")); 
            for (int i = 0; i < noOfAlarms; i++)
            {
                AlarmName name = (AlarmName)Enum.Parse(typeof(AlarmName), Cfg_LT.Get_Item($"Alarm_{i}", "Name"));
                GenericParam<AlarmLevel> level = new GenericParam<AlarmLevel>(Cfg_LT, $"Alarm_{i}", "Level");
                GenericParam<string> code = new GenericParam<string>(Cfg_LT, $"Alarm_{i}", "Code");
                string desc = Cfg_LT.Get_Item($"Alarm_{i}", "Description");
                int inRelay = 0;
                GenericParam<int> outRelay = new GenericParam<int>(Cfg_LT, $"Alarm_{i}", "OutRelayChannelNo");

                AlarmObject alarm = new AlarmObject(name, level, code, desc, inRelay, outRelay);
                AllAlarms.Add(alarm);
            }

            SetAlarmThread = new Thread(SetAlarmProcess) { IsBackground = true };
            SetAlarmThread.Start();

            InitThisMonthHistory();

            return true;
        }

        public static List<AlarmObject> GetAllAlarms()
        {
            return AllAlarms;
        }

        public static AlarmObject GetAlarmObject(AlarmName category)
        {
            var alarmObj = AllAlarms.Where(alarm => alarm.Name == category).ToList();
            if (alarmObj.Count == 1)
            {
                return alarmObj[0];
            }
            return null;
        }

        public static void InitThisMonthHistory()
        {
            string filePath = $@"{PreDef.Path_History_Alarm}\{Get_ThisYearMonthString()}";
            string fileName = $@"{filePath}\{PreDef.FileName_AlarmHistory}";
            if (File.Exists(fileName) == true)
            {
                return;
            }
                
            if (Directory.Exists($@"{filePath}") == false)
            {
                Directory.CreateDirectory(filePath);
                Thread.Sleep(100);
            }

            lock (objLock_AccessFile)
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine("OccurTime,ClearTime,Duration,Name,LevelNo,Description,Code,Relay_In,Relay_Out");
                }
            }
        }

        public static bool ReadMonthHistoryAll(DateTime dt, out List<AlarmObject> alarmObjs)
        {
            alarmObjs = new List<AlarmObject>();
            string fileName = $@"{PreDef.Path_History_Alarm}\{Get_YearMonthString(dt)}\{PreDef.FileName_AlarmHistory}";
            if (File.Exists(fileName) == false)
            {
                return false;
            }

            lock (objLock_AccessFile)
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string data = sr.ReadToEnd();
                    List<string> lineAll = data.Split('\n').ToList();
                    lineAll.RemoveAt(0);    // Remove Header
                    lineAll.Remove("");     // Remove EmptyLine
                    for (int i = 0; i < lineAll.Count; i++)
                    {
                        alarmObjs.Add(Parse(lineAll[i]));
                    }
                }
            }
            if (alarmObjs.Count == 0)
            {
                return false;
            }

            return true;
        }

        public static bool ReadDayHistorys(DateTime dtDay, out List<AlarmObject> outAlarmObjs)
        {
            outAlarmObjs = new List<AlarmObject>();
            if (ReadMonthHistoryAll(dtDay, out var monthHistorys) == false)
            {
                return false;
            }
            DateTime dtRef = new DateTime(dtDay.Year, dtDay.Month, dtDay.Day, 0, 0, 0);
            for (int i = 0; i < monthHistorys.Count; i++)
            {
                var alarmObj = monthHistorys[i];
                if (alarmObj.OccurTime >= dtRef && alarmObj.OccurTime < dtRef.AddDays(1))
                {
                    outAlarmObjs.Add(alarmObj);
                }
            }
            if (outAlarmObjs.Count == 0)
            {
                return false;
            }

            return true;
        }

        public static void SaveHistory(AlarmObject alarm)
        {
            InitThisMonthHistory();

            string history = alarm.ToString();
            string fileName = $@"{PreDef.Path_History_Alarm}\{Get_YearMonthString(alarm.OccurTime)}\{PreDef.FileName_AlarmHistory}";
            lock (objLock_AccessFile)
            {
                using (StreamWriter sw = new StreamWriter(fileName, append: true))
                {
                    sw.WriteLine(history);
                }
            }
        }

        public static string Backup(int backupTargetYear, int backupTargetMonth)
        {
            string backupFileName = $"{backupTargetYear:0000}{backupTargetMonth:00}";
            string backupPath = @"History\Backup\Alarm";
            string scrFileName = $@"{PreDef.FileName_AlarmHistory}\{backupTargetYear:0000}{backupTargetMonth:00}\{PreDef.FileName_AlarmHistory}";
            if (Directory.Exists(backupPath) == false)
            {
                Directory.CreateDirectory(backupPath);
                System.Threading.Thread.Sleep(100);
            }
            lock (objLock_AccessFile)
            {
                if (File.Exists(scrFileName) == true)
                {
                    string backDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    backupFileName = Path.Combine(backupPath, $"Backup{backDate}_Org{backupFileName}.csv");
                    File.Move(scrFileName, backupFileName);
                    Thread.Sleep(100);
                }
            }
            return backupFileName;
        }

        public static AlarmObject Parse(string line)
        {
            string[] items = line.Split(',');
            if (items.Length != 9)
            {
                return null;
            }
            AlarmObject info = new AlarmObject(items);

            return info;
        }

        private static string Get_YearMonthString(DateTime dt)
        {
            return $"{dt.Year:0000}{dt.Month:00}";
        }

        private static string Get_ThisYearMonthString()
        {
            return Get_YearMonthString(DateTime.Now);
        }

        private static void SetAlarmProcess()
        {
            bool TestModeEnabled = false;
            while (true)
            {
                if (qSetAlarm.TryDequeue(out object input) == true)
                {
                    object[] alarmInfo = (object[])input;
                    AlarmMsg msg = (AlarmMsg)alarmInfo[0];
                    switch (msg)
                    {
                        case AlarmMsg.Set:
                        case AlarmMsg.Reset:
                            if (TestModeEnabled == true)
                            {
                                Thread.Sleep(1);
                                continue;
                            }

                            HandleAlarmMsg(msg, (AlarmName)alarmInfo[1], TestModeEnabled);
                            break;

                        case AlarmMsg.TestMode_Enable:
                            TestModeEnabled = (bool)alarmInfo[1];
                            lock (objLock_OnAlarmList)
                            {
                                OnAlarmList.Clear();
                                AlarmUpdateEvent?.Invoke();
                            }
                            break;

                        case AlarmMsg.TestMode_Set:
                        case AlarmMsg.TestMode_Reset:
                            if (TestModeEnabled == false)
                            {
                                Thread.Sleep(1);
                                continue;
                            }

                            HandleAlarmMsg(msg, (AlarmName)alarmInfo[1], TestModeEnabled);
                            break;

                        case AlarmMsg.Clear:
                            break;
                    }
                    continue;
                }
                Thread.Sleep(1);
            }
        }

        private static void HandleAlarmMsg(AlarmMsg msg, AlarmName name, bool testMode)
        {
            // Get Alarm Object
            var alarmObj = GetAlarmObject(name);
            if (alarmObj == null)
            {
                // #. Invalid
                Thread.Sleep(1);
                return;
            }

            // Check Enabled
            if (alarmObj.Gen_Level.Value == AlarmLevel.NoUse)
            {
                // Alarm is disabled
                // Remove On-Alarm List
                if (OnAlarmList.Contains(alarmObj) == true)
                {
                    alarmObj.Reset();
                    OnAlarmList.Remove(alarmObj);
                }
                Thread.Sleep(1);
                return;
            }

            AlarmString_Cur.Clear();

            // Set or Reset Alarm
            lock (objLock_OnAlarmList)
            {
                if (msg == AlarmMsg.Set)
                {
                    // Add On-Alarm List
                    if (OnAlarmList.Contains(alarmObj) == false)
                    {
                        bool durationCheck = testMode == false;
                        alarmObj.Set(durationCheck);
                        OnAlarmList.Add(alarmObj);
                    }
                }
                else // if (msg == AlarmMsg.Reset)
                {
                    // Remove On-Alarm List
                    if (OnAlarmList.Contains(alarmObj) == true)
                    {
                        bool saveHistory = testMode == false;
                        alarmObj.Reset(saveHistory);
                        OnAlarmList.Remove(alarmObj);
                    }
                }

                OnAlarmList.ForEach(alarm => AlarmString_Cur.Add(alarm.ToString()));
            }

            if (testMode == false)
            {
                if (AlarmString_Cur.SequenceEqual(AlarmString_Prv) == false)
                {
                    // TBD: Notify Alarm Update 
                    AlarmUpdateEvent?.Invoke();
                }
                AlarmString_Prv.Clear();
                AlarmString_Cur.ForEach(alarmString => AlarmString_Prv.Add(alarmString));
            }
        }

        public static void EnableTestMode(bool enb)
        {
            qSetAlarm.Enqueue(new object[] { AlarmMsg.TestMode_Enable, enb });
        }

        public static void Set_Alarm(AlarmName name, bool testMode = false)
        {
            if (AllAlarms.Count(alarm => alarm.Name == name) == 0)
            {
                return;
            }
                        
            qSetAlarm.Enqueue(new object[] { testMode == true? AlarmMsg.TestMode_Set : AlarmMsg.Set, name });
        }

        public static void Reset_Alarm(AlarmName name, bool testMode = false)
        {
            if (AllAlarms.Count(alarm => alarm.Name == name) == 0)
            {
                return;
            }
            qSetAlarm.Enqueue(new object[] { testMode == true ? AlarmMsg.TestMode_Reset : AlarmMsg.Reset, name });
        }

        public static int Get_AlarmCode()
        {
            int rtnCode = 0;
            lock (objLock_OnAlarmList)
            {
                OnAlarmList.ForEach(alarm => rtnCode |= alarm.nCode);
            }
            return rtnCode;
        }

        public static List<int> Get_AlarmRelays()
        {
            List<int> alarmsRelayChannelNo = new List<int>();
            lock (objLock_OnAlarmList)
            {
                OnAlarmList.ForEach(alarm =>
                {
                    if (alarm.OutRelayChannelNo > -1)
                    {
                        alarmsRelayChannelNo.Add(alarm.InRelayNo);
                    }
                });
            }
            return alarmsRelayChannelNo;
        }

        public static bool IsAlarmExist()
        {
            bool bExist = false;
            lock (objLock_OnAlarmList)
            {
                if (OnAlarmList == null || OnAlarmList.Count == 0)
                {
                    return false;
                }
                int exceptNone = OnAlarmList.Count(alarm => alarm.Gen_Level.Value != AlarmLevel.NoUse && alarm.Gen_Level.Value != AlarmLevel.Notify);
                bExist = exceptNone > 0;
            }
            return bExist;
        }

        public static bool IsCriticalAlarmExist()
        {
            bool bExist = false;
            lock (objLock_OnAlarmList)
            {
                if (OnAlarmList == null || OnAlarmList.Count == 0)
                {
                    return false;
                }
                bExist = OnAlarmList.Count(alarm => alarm.Gen_Level.Value == AlarmLevel.Critical) > 0;
            }
            return bExist;
        }

        public static bool IsAlarmOn(AlarmName alarmName)
        {
            lock (objLock_OnAlarmList)
            {
                if (OnAlarmList.Count > 0 && OnAlarmList.Count(alarm => alarm.Name == alarmName) == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class AlarmObject
    {
        public AlarmName Name { get; private set; }

        public GenericParam<AlarmLevel> Gen_Level { get; private set; }
        private int _LevelNo = 0;
        public int LevelNo
        {
            get
            {
                if (Gen_Level != null)
                {
                    return (int)Gen_Level.Value;
                }
                return _LevelNo;
            }
        }
        public AlarmLevel eLevel
        {
            get
            {
                if (Gen_Level != null)
                {
                    return Gen_Level.Value;
                }
                return (AlarmLevel)_LevelNo;
            }
        }

        public GenericParam<string> Gen_Code { get; private set; }
        private string _sCode = string.Empty;
        public string sCode
        {
            get
            {
                if (Gen_Code != null)
                {
                    return Gen_Code.Value;
                }
                return _sCode;
            }
        }
        public int nCode
        {
            get
            {
                if (Gen_Code != null)
                {
                    return string.IsNullOrEmpty(Gen_Code.Value) == true ? 0 : Convert.ToInt32(Gen_Code.Value, 16);
                }
                return string.IsNullOrEmpty(_sCode) == true ? 0 : Convert.ToInt32(_sCode, 16);
            }
        }

        public string Description { get; private set; }
        public int InRelayNo { get; private set; }
        public DateTime OccurTime;
        public DateTime ClearTime;
        public TimeSpan Duration;
        public GenericParam<int> Gen_OutRelayChannelNo { get; private set; }
        private int _OutRelayChannelNo = 0;
        public int OutRelayChannelNo
        {
            get
            {
                if (Gen_OutRelayChannelNo != null)
                {
                    return Gen_OutRelayChannelNo.Value;
                }
                return _OutRelayChannelNo;
            }
        }

        public bool _IsSet = false;
        public object objLock_IsSet = new object();
        public bool IsSet
        {
            get
            {
                lock (objLock_IsSet)
                {
                    return _IsSet;
                }
            }
            set
            {
                lock (objLock_IsSet)
                {
                    _IsSet = value;
                }
            }
        }
        private System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();

        public AlarmObject(AlarmName name, GenericParam<AlarmLevel> level, GenericParam<string> code, string desc, int inRelay, GenericParam<int> outRelay)
        {
            Name = name;
            Gen_Level = level;
            Gen_Code = code;
            Description = desc;
            InRelayNo = inRelay - 1;
            Gen_OutRelayChannelNo = outRelay;
        }

        public AlarmObject(string[] items)
        {
            OccurTime = DateTime.Parse(items[0]);
            ClearTime = DateTime.Parse(items[1]);
            Duration = TimeSpan.Parse(items[2]);
            if (Enum.TryParse(items[3], out AlarmName cat) == true)
            {
                Name = cat;
            }
            else
            {
                Name = AlarmName.None;
            }
            if (Enum.TryParse(items[4], out AlarmLevel eLevel) == true)
            {
                _LevelNo = (int)eLevel;
            }
            else if (int.TryParse(items[4], out int level) == true)
            {
                _LevelNo = level;
            }
            Description = items[5];
            _sCode = items[6];
            //InRelayNo = int.Parse(items[7]);
            _OutRelayChannelNo = int.Parse(items[8]);
        }

        public void Init(AlarmLevel alarmLevel, string code, int outRelayChNo)
        {
            if (Gen_Level != null)
            {
                Gen_Level.Set_Value(alarmLevel);
            }
        }

        public void Set(bool timerEnable = true)
        {
            OccurTime = DateTime.Now;
            if (timerEnable == true)
            {
                if (Watch.IsRunning == false)
                {
                    Watch.Start();
                }
            }
            IsSet = true;
        }

        public void Reset(bool saveHistory = true)
        {
            ClearTime = DateTime.Now;
            if (Watch.IsRunning == true)
            {
                Watch.Stop();
                Duration = ClearTime - OccurTime;
            }

            if (saveHistory == true)
            {
                LT_Alarm.SaveHistory(this);
            }
            IsSet = false;
        }

        public override string ToString()
        {
            string line = $"{OccurTime:yyyy-MM-dd HH:mm:ss},{ClearTime:yyyy-MM-dd HH:mm:ss},{Duration.Hours:00}:{Duration.Minutes:00}:{Duration.Seconds:00}," +
                          $"{Name},{LevelNo},{Description},{sCode},{InRelayNo},{OutRelayChannelNo}";
            return line;
        }
    }
}
