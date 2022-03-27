using Microsoft.Win32;
using System;
using SystemPerformanceAnalyser.Interfaces;

namespace SystemPerformanceAnalyser.Services
{
    public class SaveFileService : ISaveFileService
    {
        private readonly SaveFileDialog _saveFileDialog = new();

        public bool? SaveFile()
        {
            _saveFileDialog.Filter = "Pattern Files(*.png)|*.png|All files (*.*)|*.*";
            _saveFileDialog.FilterIndex = 1;
            _saveFileDialog.DefaultExt = ".png";
            _saveFileDialog.FileName = $"Export_{DateTime.Now:yyMMdd_HHmmss}";
            _saveFileDialog.RestoreDirectory = true;

            var choosenFile = _saveFileDialog.ShowDialog();
            if (choosenFile == true)
            {
                File = _saveFileDialog.FileNames[0];
            }
            return choosenFile;
        }

        public string File { get; private set; }
    }
}
