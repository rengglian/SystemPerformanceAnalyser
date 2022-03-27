using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemPerformanceAnalyser.Model
{
    public class DataStatistics
    {
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public long TotalDataPoints { get; set; }
        public string SectionStart { get; set; } = "";
        public string SectionEnd { get; set; } = "";
        public long SectionDataPoints { get; set; }
        public List<SeriesCharacteristics> SeriesCharacteristics { get; set; } = new();

        public DataStatistics(DataFrame dataFrame, DateTime start, DateTime end, List<CheckableItem<string>> items )
        {
            StartTime = dataFrame.Columns["TimeStamp"].Cast<DateTime>().First().ToString();
            EndTime = dataFrame.Columns["TimeStamp"].Cast<DateTime>().Last().ToString();
            TotalDataPoints = dataFrame.Columns["TimeStamp"].Cast<DateTime>().LongCount();
            SectionStart = start.ToString();
            SectionEnd = end.ToString();

            var df = BetweenTimesFilter(dataFrame, start, end);
            SectionDataPoints = df.Columns["TimeStamp"].Cast<DateTime>().LongCount();

            SeriesCharacteristics.Clear();
            foreach (var (item, characteristics) in from item in items
                                                    where item.IsCheckedLeftAxis || item.IsCheckedRightAxis
                                                    let test = new SeriesCharacteristics
                                                    {
                                                        Name = item.Value,
                                                        Mean = df.Columns[item.Value].Mean(),
                                                        Max = (float)df.Columns[item.Value].Max(),
                                                        Min = (float)df.Columns[item.Value].Min(),
                                                        Median = df.Columns[item.Value].Median(),
                                                        StandardDeviation = CalculateStandardDeviation(df.Columns[item.Value]),
                                                        Slope = 0.0
                                                    }
                                                    select (item, test))
            {
                characteristics.Name = item.Value;
                SeriesCharacteristics.Add(characteristics);
            }
        }

        private static double CalculateStandardDeviation(DataFrameColumn dataFrameColumn)
        {
            var mean = dataFrameColumn.Mean();
            var square = dataFrameColumn.Subtract(mean) * dataFrameColumn.Subtract(mean);
            var variance = square.Mean();
            return Math.Sqrt(variance);
        }

        private static DataFrame BetweenTimesFilter(DataFrame dataFrame, DateTime start, DateTime end)
        {
            var lastHeader = dataFrame.Columns.Count - 1;
            var mask1 = dataFrame.Columns[lastHeader].ElementwiseGreaterThanOrEqual(start.Ticks);
            var mask2 = dataFrame.Columns[lastHeader].ElementwiseLessThanOrEqual(end.Ticks);
            var mask3 = mask1.ElementwiseEquals(mask2);
            return dataFrame.Filter(mask3);
        }

    }
}
