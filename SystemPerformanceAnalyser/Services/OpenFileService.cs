using Microsoft.Win32;
using System.IO;
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
                File = _openFileDialog.FileNames[0];
                FileName = Path.GetFileName(File);
            }
            return choosenFile;
        }

        public string File { get; private set; }
        public string FileName { get; private set; }
    }
}
