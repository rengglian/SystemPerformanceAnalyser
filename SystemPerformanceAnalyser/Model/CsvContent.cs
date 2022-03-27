using Microsoft.Data.Analysis;
using System.Collections.Generic;

namespace SystemPerformanceAnalyser.Model
{
    public class CsvContent
    {
        public string FileName { get; set; } = "";
        public DataFrame DataFrame { get; set; } = new();
        public List<string> ColumnHeaders { get; set; } = new();
    }
}
