namespace SystemPerformanceAnalyser.Model
{
    public class SeriesCharacteristics
    {
        public string Name { get; set; } = string.Empty;
        public float Min { get; set; }
        public float Max { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double Slope { get; set; }
        public double StandardDeviation { get; set; }
    }
}
