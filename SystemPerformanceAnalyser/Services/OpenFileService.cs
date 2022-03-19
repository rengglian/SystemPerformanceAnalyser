using Microsoft.Win32;
using SystemPerformanceAnalyser.Interfaces;

namespace SystemPerformanceAnalyser.Services
{
    public class OpenFileService : IOpenFileService
    {
        private readonly OpenFileDialog _openFileDialog = new();

        public bool? OpenFile()
        {
            _openFileDialog.Filter = "Pattern Files(*.csv)|*.csv|All files (*.*)|*.*";
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.RestoreDirectory = true;

            var choosenFile = _openFileDialog.ShowDialog();
            if (choosenFile.HasValue && choosenFile.Value)
            {
                FileNames = _openFileDialog.FileNames;
            }
            return choosenFile;
        }

        public string[] FileNames { get; private set; }
    }
}
