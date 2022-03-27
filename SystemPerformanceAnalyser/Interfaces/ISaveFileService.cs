namespace SystemPerformanceAnalyser.Interfaces
{
    public interface ISaveFileService
    {
        public bool? SaveFile();
        public string File { get; }
    }
}
