namespace SystemPerformanceAnalyser.Interfaces
{
    public interface IOpenFileService
    {
        public bool? OpenFile();
        public string File { get; }
        public string FileName { get; }
    }
}
