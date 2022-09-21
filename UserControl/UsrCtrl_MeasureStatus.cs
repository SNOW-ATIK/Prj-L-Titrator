using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L_Titrator.Controls
{
    public partial class UsrCtrl_MeasureStatus : UserControl
    {
        public enum Contents
        { 
            Clear,
            RecipeName,
            StartTime,
            TitrationEndTime,
            SequenceEndTime,
            Duration,
            Log,
        }

        public UsrCtrl_MeasureStatus()
        {
            InitializeComponent();
        }

        private DateTime DtStart;
        private DateTime DtEnd;
        public void Set_Content(Contents content, object info)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Set_Content(content, info)));
                return;
            }
            RecipeObj rcpObj;
            switch (content)
            {
                case Contents.Clear:
                    lbl_RecipeName.Text = "-";
                    lbl_StartTime.Text = "-";
                    lbl_Titration_End.Text = "-";
                    lbl_Titration_Duration.Text = "-";
                    lbl_Sequence_End.Text = "-";
                    lbl_Sequence_Duration.Text = "-";
                    lsb_Log.Items.Clear();
                    break;

                case Contents.RecipeName:
                    if (LT_Recipe.Get_RecipeObj((int)info, out rcpObj) == true)
                    {
                        lbl_RecipeName.Text = rcpObj.Name;
                    }
                    break;

                case Contents.StartTime:
                    object[] infos = (object[])info;
                    DtStart = (DateTime)infos[0];
                    lbl_StartTime.Text = DtStart.ToString("yyyy-MM-dd HH:mm:ss");
                    if (LT_Recipe.Get_RecipeObj((int)infos[1], out rcpObj) == true)
                    {
                        lbl_RecipeName.Text = rcpObj.Name;
                    }
                    break;

                case Contents.TitrationEndTime:
                    DtEnd = (DateTime)info;
                    lbl_Titration_End.Text = DtEnd.ToString("yyyy-MM-dd HH:mm:ss");

                    TimeSpan tsTitration = DtEnd - DtStart;
                    lbl_Titration_Duration.Text = $"{tsTitration.Minutes:00}:{tsTitration.Seconds:00}";
                    break;

                case Contents.SequenceEndTime:
                    DtEnd = (DateTime)info;
                    lbl_Sequence_End.Text = DtEnd.ToString("yyyy-MM-dd HH:mm:ss");

                    TimeSpan tsSequence = DtEnd - DtStart;
                    lbl_Sequence_Duration.Text = $"{tsSequence.Minutes:00}:{tsSequence.Seconds:00}";
                    break;

                case Contents.Duration:
                    break;

                case Contents.Log:
                    lsb_Log.Items.Add($"{DateTime.Now:HH:mm:ss}>{(string)info}");
                    lsb_Log.SelectedIndex = lsb_Log.Items.Count - 1;
                    lsb_Log.SelectedItem = null;
                    break;
            }
        }
    }
}
