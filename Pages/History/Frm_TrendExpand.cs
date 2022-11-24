using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK;
using DevExpress.XtraCharts;

namespace L_Titrator.Pages.History
{
    public partial class Frm_TrendExpand : Form
    {
        public Frm_TrendExpand()
        {
            InitializeComponent();
        }

        public void Init(string sampleName, Page_DataHistory.TrendType trendType)
        {
            TrendChart.Series.Clear();
            TrendChart.ClearCache();

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

            switch (trendType)
            {
                case Page_DataHistory.TrendType.Day:
                    xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                    xyDiagram.AxisX.Label.TextPattern = "{A:MMM-dd HH}";
                    break;

                case Page_DataHistory.TrendType.Month:
                    xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                    xyDiagram.AxisX.Label.TextPattern = "{A:MMM-dd}";
                    break;

                case Page_DataHistory.TrendType.Period:
                    xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                    xyDiagram.AxisX.Label.TextPattern = "{A:MMM-dd}";
                    break;
            }

            ((XYDiagram)TrendChart.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)TrendChart.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)TrendChart.Diagram).AxisX.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
            ((XYDiagram)TrendChart.Diagram).AxisY.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
            ((XYDiagram)TrendChart.Diagram).AxisX.Title.Text = "Time";
            ((XYDiagram)TrendChart.Diagram).AxisY.Title.Text = "Concentration";
        }

        public void DrawGraph(string sampleName, Dictionary<DateTime, double> trendSource)
        {
            List<int> xIdx = new List<int>();
            int idx = 0;
            trendSource.Keys.ToList().ForEach(dt =>
            {
                TrendChart.Series[sampleName].Points.AddPoint(dt, trendSource[dt]);
                xIdx.Add(idx++);
            });

            var series = TrendChart.Series[sampleName];
            double dMinVal = trendSource.Values.Min() * 0.9;
            double dMaxVal = trendSource.Values.Max() * 1.1;
            var diagram = (XYDiagram)TrendChart.Diagram;
            diagram.AxisY.WholeRange.SetMinMaxValues(dMinVal, dMaxVal);

            CmpVal_Min.Prm_Value = trendSource.Values.Min().ToString("0.0###");
            CmpVal_Max.Prm_Value = trendSource.Values.Max().ToString("0.0###");

            double dAvg = trendSource.Values.Average();
            CmpVal_Avg.Prm_Value = dAvg.ToString("0.0###");

            double dStd = DataHandling.CalculateStandardDeviation(trendSource.Values);
            double dRsd = dStd / dAvg * 100;
            CmpVal_RSD.Prm_Value = dRsd.ToString("0.0000");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Frm_TrendExpand_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Location.X, this.Location.Y - 2);
        }
    }
}
