using Microsoft.Data.Analysis;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemPerformanceAnalyser.Helper
{
    public class PlotModelHelper
    {
        public static PlotModel CreateTimeSeriesPlot(List<string> legend, DataFrame dataFrame)
        {
            var timeStamps = dataFrame.Columns["TimeStamp"].Cast<DateTime>().ToArray();
            var startDate = timeStamps.First();
            var endDate = timeStamps.Last();

            var minValue = DateTimeAxis.ToDouble(startDate);
            var maxValue = DateTimeAxis.ToDouble(endDate);

            var tmp = new PlotModel
            {
                PlotMargins = new OxyThickness(50, 0, 0, 40)
            };

            foreach (string item in legend)
            {
                var ls = new LineSeries { 
                    Title = item,
                    LineStyle = LineStyle.Solid,
                   
                };

                var values = dataFrame.Columns[item].Cast<float>().ToArray();
                for (int i = 0; i < values.Length; i++)
                {
                    ls.Points.Add(DateTimeAxis.CreateDataPoint(timeStamps[i], values[i]));
                }
                tmp.Series.Add(ls);
            }

            var y_axis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.LongDash,
            };

            tmp.Axes.Add(y_axis);

            var x_axis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = minValue,
                Maximum = maxValue,
                StringFormat = "yyyy.MM.dd HH:mm:ss",
                Angle = 15,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.LongDash
            };
            tmp.Axes.Add(x_axis);

            tmp.Legends.Add(new Legend
            {
                LegendTitle = "Legend",
                LegendPosition = LegendPosition.TopRight,
                LegendPlacement = LegendPlacement.Outside
            });

            return tmp;
        }
    }
}
