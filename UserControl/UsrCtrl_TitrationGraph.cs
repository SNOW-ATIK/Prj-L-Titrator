using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.XtraCharts;

namespace L_Titrator.Controls
{
    public partial class UsrCtrl_TitrationGraph : UserControl
    {
        private RecipeObj CurRecipe;
        private string CurDisplayRecipeName = "";
        private Dictionary<string, ChartControl> GraphDic = new Dictionary<string, ChartControl>();

        public UsrCtrl_TitrationGraph()
        {
            InitializeComponent();
        }

        public void Init_Background(RecipeObj rcpObj, bool showLabel = true)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Init_Background(rcpObj)));
                return;
            }
            CurRecipe = rcpObj;

            if (GraphDic.Count > 0)
            {
                GraphDic.Values.ToList().ForEach(chart =>
                {
                    chart.Series.Clear();
                    chart.ClearCache();
                    chart.Dispose();
                });
                GraphDic.Clear();
            }

            TableLayoutPanel tbl_BG = new TableLayoutPanel();
            tbl_BG.RowStyles.Add(new RowStyle(SizeType.Absolute));
            if (showLabel == true)
            {
                tbl_BG.RowStyles[0].Height = 22;
            }
            else
            {
                tbl_BG.RowStyles[0].Height = 0;
            }
            tbl_BG.Margin = new Padding(0, 0, 0, 0);
            tbl_BG.Dock = DockStyle.Fill;

            pnl_BG.Controls.Clear();
            pnl_BG.Controls.Add(tbl_BG);

            List<Step> ValidTitrationStep = new List<Step>();
            CurRecipe.Sequences.ForEach(seq => 
            {
                List<Step> StepAll = seq.Steps.ToList();
                for (int i = 0; i < StepAll.Count; i++)
                {
                    if (StepAll[i].IsTitration == true && StepAll[i].Enabled == true)
                    {
                        ValidTitrationStep.Add(StepAll[i]);

                        ChartControl chart = new ChartControl();
                        chart.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                        chart.Margin = new Padding(1, 1, 1, 1);
                        chart.Location = new Point(1, 1);
                        chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
                        chart.Series.Add(StepAll[i].Name, ViewType.Line);
                        ((LineSeriesView)chart.Series[StepAll[i].Name].View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                        ((LineSeriesView)chart.Series[StepAll[i].Name].View).LineMarkerOptions.Size = 5;
                        XYDiagram xyDiagram = (XYDiagram)chart.Diagram;
                        xyDiagram.EnableAxisXZooming = true;
                        xyDiagram.EnableAxisYZooming = true;
                        xyDiagram.ZoomingOptions.UseKeyboard = true;
                        xyDiagram.ZoomingOptions.UseKeyboardWithMouse = true;
                        xyDiagram.ZoomingOptions.UseMouseWheel = true;
                        xyDiagram.ZoomingOptions.UseTouchDevice = true;

                        ((XYDiagram)chart.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                        ((XYDiagram)chart.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                        ((XYDiagram)chart.Diagram).AxisX.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
                        ((XYDiagram)chart.Diagram).AxisY.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
                        ((XYDiagram)chart.Diagram).AxisX.Title.Text = "Injection Volume [mL]";
                        ((XYDiagram)chart.Diagram).AxisY.Title.Text = "Sensor Output [mV]";

                        GraphDic.Add(StepAll[i].Name, chart);
                    }
                }
            });

            if (ValidTitrationStep.Count == 0)
            {
                return;
            }

            if (GraphDic.Count > 0)
            {
                for (int i = 0; i < GraphDic.Count; i++)
                {
                    tbl_BG.ColumnStyles.Add(new ColumnStyle(SizeType.Percent));
                }                
            }

            int cmpCount = 0;
            GraphDic.Keys.ToList().ForEach(tgtName =>
            {
                Label lbl = new Label();
                lbl.Font = new Font("Consolas", 11f, FontStyle.Bold);
                lbl.Text = tgtName;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Margin = new Padding(1, 1, 1, 1);
                lbl.Location = new Point(1, 1);
                lbl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                lbl.BackColor = Color.LemonChiffon;
                lbl.BorderStyle = BorderStyle.FixedSingle;

                tbl_BG.Controls.Add(lbl, cmpCount, 0);
                tbl_BG.Controls.Add(GraphDic[tgtName], cmpCount, 1);

                tbl_BG.ColumnStyles[cmpCount].Width = 100 / GraphDic.Count;

                ++cmpCount;
            });

            chk_ViewMuliti.Enabled = GraphDic.Count > 1;
            btn_Prev.Enabled = GraphDic.Count > 1;
            btn_Next.Enabled = GraphDic.Count > 1;

            chk_Include_1st_mV.Checked = true;
            chk_Include_1st_mV.Enabled = false;

            CurDisplayRecipeName = ValidTitrationStep[0].Name;
        }

        public void Init_Background(string sampleName, bool showLabel = true)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => Init_Background(sampleName, showLabel)));
                return;
            }

            if (GraphDic.Count > 0)
            {
                GraphDic.Values.ToList().ForEach(old =>
                {
                    old.Series.Clear();
                    old.ClearCache();
                    old.Dispose();
                });
                GraphDic.Clear();
            }

            TableLayoutPanel tbl_BG = new TableLayoutPanel();
            tbl_BG.RowStyles.Add(new RowStyle(SizeType.Absolute));
            if (showLabel == true)
            {
                tbl_BG.RowStyles[0].Height = 22;
            }
            else
            {
                tbl_BG.RowStyles[0].Height = 0;
            }
            tbl_BG.Margin = new Padding(0, 0, 0, 0);
            tbl_BG.Dock = DockStyle.Fill;

            pnl_BG.Controls.Clear();
            pnl_BG.Controls.Add(tbl_BG);

            ChartControl chart = new ChartControl();
            chart.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            chart.Margin = new Padding(1, 1, 1, 1);
            chart.Location = new Point(1, 1);
            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chart.Series.Add(sampleName, ViewType.Line);
            ((LineSeriesView)chart.Series[sampleName].View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((LineSeriesView)chart.Series[sampleName].View).LineMarkerOptions.Size = 5;
            XYDiagram xyDiagram = (XYDiagram)chart.Diagram;
            xyDiagram.EnableAxisXZooming = true;
            xyDiagram.EnableAxisYZooming = true;
            xyDiagram.ZoomingOptions.UseKeyboard = true;
            xyDiagram.ZoomingOptions.UseKeyboardWithMouse = true;
            xyDiagram.ZoomingOptions.UseMouseWheel = true;
            xyDiagram.ZoomingOptions.UseTouchDevice = true;

            ((XYDiagram)chart.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)chart.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)chart.Diagram).AxisX.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
            ((XYDiagram)chart.Diagram).AxisY.Title.Font = new Font("Consolas", 10f, FontStyle.Regular);
            ((XYDiagram)chart.Diagram).AxisX.Title.Text = "Injection Volume [mL]";
            ((XYDiagram)chart.Diagram).AxisY.Title.Text = "Sensor Output [mV]";

            GraphDic.Add(sampleName, chart);

            if (GraphDic.Count > 0)
            {
                for (int i = 0; i < GraphDic.Count; i++)
                {
                    tbl_BG.ColumnStyles.Add(new ColumnStyle(SizeType.Percent));
                }
            }

            int cmpCount = 0;
            GraphDic.Keys.ToList().ForEach(tgtName =>
            {
                Label lbl = new Label();
                lbl.Font = new Font("Consolas", 11f, FontStyle.Bold);
                lbl.Text = tgtName;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Margin = new Padding(1, 1, 1, 1);
                lbl.Location = new Point(1, 1);
                lbl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                lbl.BackColor = Color.LemonChiffon;
                lbl.BorderStyle = BorderStyle.FixedSingle;

                tbl_BG.Controls.Add(lbl, cmpCount, 0);
                tbl_BG.Controls.Add(GraphDic[tgtName], cmpCount, 1);

                tbl_BG.ColumnStyles[cmpCount].Width = 100 / GraphDic.Count;

                ++cmpCount;
            });

            chk_ViewMuliti.Enabled = GraphDic.Count > 1;
            btn_Prev.Enabled = GraphDic.Count > 1;
            btn_Next.Enabled = GraphDic.Count > 1;

            chk_Include_1st_mV.Checked = true;
            chk_Include_1st_mV.Enabled = false;

            CurDisplayRecipeName = sampleName;
        }

        public void AddPoint(string tgtName, double injVol, double analog_mV)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => AddPoint(tgtName, injVol, analog_mV)));
                return;
            }
            if (GraphDic.ContainsKey(tgtName) == false)
            {
                return;
            }
            GraphDic[tgtName].Series[tgtName].Points.AddPoint(injVol, analog_mV);

            chk_Include_1st_mV.Enabled = GraphDic[tgtName].Series[tgtName].Points.Count > 1;
            Set_GraphRange();
        }

        public void ClearAll()
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new Action(() => ClearAll()));
                return;
            }

            pnl_BG.Controls.Clear();
        }

        private void Set_GraphRange()
        {
            var series = GraphDic[CurDisplayRecipeName].Series[CurDisplayRecipeName];
            double dMinVal = 0;
            double dMaxVal = double.Parse(series.Points[series.Points.Count - 1].Argument);
            var diagram = (XYDiagram)GraphDic[CurDisplayRecipeName].Diagram;
            if (chk_Include_1st_mV.Checked == true)
            {
                dMinVal = double.Parse(series.Points[0].Argument);

            }
            else
            {
                dMinVal = double.Parse(series.Points[1].Argument);
            }
            diagram.AxisX.WholeRange.SetMinMaxValues(dMinVal, dMaxVal);
        }

        private void chk_ViewMuliti_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ViewMuliti.Checked == true)
            {
                chk_ViewMuliti.Text = "MULTI";
            }
            else
            {
                chk_ViewMuliti.Text = "SINGLE";
            }
        }

        private void chk_Include_1st_mV_CheckedChanged(object sender, EventArgs e)
        {
            if (GraphDic[CurDisplayRecipeName].Series[CurDisplayRecipeName].Points.Count == 0)
            {
                return;
            }
            Set_GraphRange();
        }

        private void btn_Prev_Click(object sender, EventArgs e)
        {

        }

        private void btn_Next_Click(object sender, EventArgs e)
        {

        }

        private void btn_ZoomIn_Click(object sender, EventArgs e)
        {

        }

        private void btn_ZoomOut_Click(object sender, EventArgs e)
        {

        }
    }
}
