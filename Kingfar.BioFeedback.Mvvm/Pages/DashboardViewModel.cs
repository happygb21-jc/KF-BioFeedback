using System;
using System.Linq;
using System.Windows.Media.Media3D;
using Dm;
using NewLife.Security;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Kingfar.BioFeedback.Mvvm
{
    public partial class DashboardViewModel : ViewModelBase
    {
        #region Property

        [ObservableProperty]
        private PlotModel _dailyAverageChart;

        [ObservableProperty]
        private PlotModel _ageChart;

        [ObservableProperty]
        private PlotModel _sexChart;

        [ObservableProperty]
        private int _patientCount;

        //Individual
        [ObservableProperty]
        private int _individualCount;

        [ObservableProperty]
        private int _groupCount;

        #endregion Property

        public DashboardViewModel()
        {
            var rand = new Random();
            PatientCount =(int)(rand.NextDouble() * 100);
            IndividualCount = (int)(rand.NextDouble() * 100);
            GroupCount = (int)(rand.NextDouble() * 100);

            #region 折线图

            LineChart(rand);

            #endregion 折线图

            #region 柱状图

            BarChart(rand);

            #endregion 柱状图

            #region 饼图

            PieChart(rand);

            #endregion 饼图
        }

        private void LineChart(Random rand)
        {
            this.DailyAverageChart = new PlotModel();

            this.DailyAverageChart.Axes.Add(new DateTimeAxis
            {
                Title = "日期",
                Position = AxisPosition.Bottom,
                StringFormat = "M/d",
                MajorStep = 1, // 设置主要刻度间隔为1天
                MinorStep = 1, // 设置次要刻度间隔为1天
                IntervalType = DateTimeIntervalType.Days, // 设置刻度间隔类型为天
                IsZoomEnabled = false, // 禁用缩放
                IsPanEnabled = false // 禁用平移
            });
            this.DailyAverageChart.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "训练次数"
            });
            var lineSeries = new LineSeries() { Title = "训练次数", Color = OxyColor.FromRgb(58, 160, 255), StrokeThickness = 2, CanTrackerInterpolatePoints = false, MarkerType = MarkerType.Circle };

            for (int i = 0; i <= 7; i++)
            {
                var currentData = DateTime.Now.AddDays(i);
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(currentData.Year, currentData.Month, currentData.Day)), (int)(rand.NextDouble() * 100)));
            }

            this.DailyAverageChart.Series.Add(lineSeries);
        }

        private void BarChart(Random rand)
        {
            this.AgeChart = new PlotModel();
            double[] cakePopularity = new double[8];
            for (int i = 0; i < 8; ++i)
            {
                cakePopularity[i] = rand.NextDouble();
            }
            var sum = cakePopularity.Sum();
            var barSeries = new BarSeries
            {
                ItemsSource = new List<BarItem>(new[]
                {
                        new BarItem{ Value = (cakePopularity[0] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[1] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[2] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[3] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[4] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[5] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[6] / sum * 100) },
                        new BarItem{ Value = (cakePopularity[7] / sum * 100) }
                }),
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:0}人",
                FillColor = OxyColor.FromRgb(58, 160, 255)
            };
            //左侧柱状图
            this.AgeChart.Series.Add(barSeries);
            this.AgeChart.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom, // X轴
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
            this.AgeChart.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = new[]
                {
                    "90岁以上",
                    "81-90岁",
                    "71-80岁",
                    "61-70岁",
                    "51-60岁",
                    "41-50岁",
                    "31-40岁",
                    "30岁以下",
                },
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
        }

        private void PieChart(Random rand)
        {
            this.SexChart = new PlotModel();
            dynamic seriesP1 = new PieSeries() { StrokeThickness = 2.0, InsideLabelPosition = 1, AngleSpan = 360, StartAngle = 0 };
            var manPercentage = (rand.NextDouble() * 1000) / 1200.0 * 100;
            seriesP1.Slices.Add(new PieSlice("男", manPercentage) { IsExploded = false, Fill = OxyColor.FromRgb(58, 160, 255) });
            seriesP1.Slices.Add(new PieSlice("女", 100 - manPercentage) { IsExploded = true, Fill = OxyColor.FromRgb(54, 203, 203) });
            this.SexChart.Series.Add(seriesP1);
        }
    }
}