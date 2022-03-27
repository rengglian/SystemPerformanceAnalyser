using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SystemPerformanceAnalyser.Model;

namespace SystemPerformanceAnalyser.Services
{
    public class CsvFromFileService
    {
        public static async Task<CsvContent> LoadFileAsync(string fileName)
        {
            return await Task.Run(() =>
            {
                CsvContent csvContent = new();
                if (File.Exists(fileName))
                {
                    csvContent.DataFrame = DataFrame.LoadCsv(fileName, separator: ';', header: true);
                    // _dataFrame needed to SetName
                    // https://github.com/dotnet/machinelearning/issues/6129 
                    csvContent.DataFrame.Columns[0].SetName("TimeStamp", csvContent.DataFrame);

                    // add additional column with ticks for filtering
                    // https://stackoverflow.com/questions/69995708/filter-a-microsoft-data-analysis-dataframe-by-datetime
                    var ticks = csvContent.DataFrame.Columns["TimeStamp"].Cast<DateTime>().Select(x => x.Ticks);
                    csvContent.DataFrame["TimeStampTicks"] = new Int64DataFrameColumn("TimeStampTicks", ticks);

                    if (csvContent.DataFrame.Columns["TimeStamp"].DataType.Name != "DateTime")
                    {
                        throw new Exception($"TimeStamp Column is of type {csvContent.DataFrame.Columns["TimeStamp"].DataType.Name}");
                    }

                    csvContent.ColumnHeaders = GetCsvHeader(csvContent.DataFrame);
                    csvContent.FileName = fileName;
                }
                else
                {
                    throw new Exception($"{fileName} does not exists");
                }
                return csvContent;
            });
        }

        private static List<string> GetCsvHeader(DataFrame dataFrame)
        {
            var headers = new List<string>();
            foreach (var column in dataFrame.Columns)
            {
                 headers.Add(column.Name);
            }
            return headers;
        }
    }
}
