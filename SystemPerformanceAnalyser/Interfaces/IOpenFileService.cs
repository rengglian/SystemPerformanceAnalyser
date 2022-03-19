namespace SystemPerformanceAnalyser.Interfaces
{
    public interface IOpenFileService
    {
        public bool? OpenFile();
        public string[] FileNames { get; }
    }
}
