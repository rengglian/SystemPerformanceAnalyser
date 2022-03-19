using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.IO;

namespace SystemPerformanceAnalyser.Services
{
    public class CsvFromFileService
    {
        private readonly DataFrame _dataFrame = new();
        public DataFrame DataFrame => _dataFrame;
        public CsvFromFileService(string fileName)
        {
            if (File.Exists(fileName))
            {
                _dataFrame = DataFrame.LoadCsv(fileName, separator: ';', header:true);
                // _dataFrame needed to SetName https://github.com/dotnet/machinelearning/issues/6129 
                _dataFrame.Columns[0].SetName("TimeStamp", _dataFrame);
                if(_dataFrame.Columns["TimeStamp"].DataType.Name != "DateTime")
                {
                    throw new Exception($"TimeStamp Column is of type {_dataFrame.Columns["TimeStamp"].DataType.Name}");
                }
            }
            else
            {
                throw new Exception($"{fileName} does not exists");
            }
        }

        public List<string> GetCsvHeader()
        {
            var headers = new List<string>();
            foreach (var column in _dataFrame.Columns)
            {
                 headers.Add(column.Name);
            }
            return headers;
        }
    }
}
