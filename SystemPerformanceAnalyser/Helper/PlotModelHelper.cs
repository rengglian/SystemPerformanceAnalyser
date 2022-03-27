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
        public enum AxisType { PrimaryY, SecondaryY, XAxis };
        public static PlotModel CreateTimeSeriesPlot(Dictionary<string, AxisType> dataSets, DataFrame dataFrame)
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

            bool hasLeftAxis = false;
            bool hasRightAxis = false;
            
            foreach (KeyValuePair<string, AxisType> item in dataSets)
            {
                if (item.Value == AxisType.PrimaryY) hasLeftAxis = true;
                if (item.Value == AxisType.SecondaryY) hasRightAxis = true;

                var ls = new LineSeries { 
                    Title = item.Key,
                    LineStyle = LineStyle.Solid,
                    YAxisKey = $"{item.Value}",
                };

                var values = dataFrame.Columns[item.Key].Cast<float>().ToArray();
                for (int i = 0; i < values.Length; i++)
                {
                    ls.Points.Add(DateTimeAxis.CreateDataPoint(timeStamps[i], values[i]));
                }
                tmp.Series.Add(ls);
            }

            if(hasLeftAxis)
            {
                var y_axis_left = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.LongDash,
                    Key = $"{AxisType.PrimaryY}",
                };
                tmp.Axes.Add(y_axis_left);
            }

            if(hasRightAxis)
            {
                var y_axis_right = new LinearAxis
                {
                    Position = AxisPosition.Right,
                    Key = $"{AxisType.SecondaryY}",
                };
                tmp.Axes.Add(y_axis_right);
            }

            var x_axis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = minValue,
                Maximum = maxValue,
                StringFormat = "yyyy.MM.dd HH:mm:ss",
                Angle = 15,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.LongDash,
                Key = $"{AxisType.XAxis}",
            };

            tmp.Axes.Add(x_axis);

            tmp.Legends.Add(new Legend
            {
                LegendTitle = "Legend",
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendMargin = 35
            });

            return tmp;
        }

        public static void ChangeAxisTitle(PlotModel plotModel, string title, AxisType axisType)
        {
            var axis = plotModel.Axes.FirstOrDefault(axis => axis.Key == $"{axisType}");
            if (axis == null)
                return;
            axis.Title = title;
        }

        public static string GetAxisTitle(PlotModel plotModel, AxisType axisType)
        {
            var axis = plotModel.Axes.FirstOrDefault(axis => axis.Key == $"{axisType}");
            if (axis == null)
                return "";
            return axis.Title;
        }

        public static DateTime GetDateTimeFromAxis(double xAxisPosition)
        {
           return DateTimeAxis.ToDateTime(xAxisPosition);
        }
    }
}
