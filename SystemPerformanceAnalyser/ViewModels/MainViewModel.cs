using OxyPlot;
using OxyPlot.Axes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemPerformanceAnalyser.Helper;
using SystemPerformanceAnalyser.Interfaces;
using SystemPerformanceAnalyser.Model;
using SystemPerformanceAnalyser.Services;

namespace SystemPerformanceAnalyser.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private const int FixPlotWidth = 1000;
        private const int FixPlotHeight = 600;

        private readonly IOpenFileService _openFileService;
        private CsvFromFileService? CsvFromFileService;

        private string _title = "";
        private string _plotTitle = "";
        private int _plotWidth;
        private int _plotHeight;
        private PlotModel _plotModel = new();
        private string _information = "";
        private List<CheckableItem<string>> _checkableItems = new();

        public string Information
        {
            get => _information; 
            set => SetProperty(ref _information, value); 
        }   

        public List<CheckableItem<string>> CheckableItems
        {
            get => _checkableItems ??= new List<CheckableItem<string>>();
            set => SetProperty(ref _checkableItems, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string PlotTitle
        {
            get => _plotTitle;
            set => SetProperty(ref _plotTitle, value);
        }

        public int PlotWidth
        {
            get => _plotWidth;
            set => SetProperty(ref _plotWidth, value);
        }

        public int PlotHeight
        {
            get => _plotHeight;
            set => SetProperty(ref _plotHeight, value);
        }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetProperty(ref _plotModel, value);
        }
        public DelegateCommand LoadFileCommand { get; set; }
        public DelegateCommand<CheckableItem<string>> CheckBoxActivity { get; set; }
        public MainViewModel(IOpenFileService openFileService)
        {
            Title = "System Performance Anaylser";

            PlotWidth = FixPlotWidth;
            PlotHeight = FixPlotHeight;

            _openFileService = openFileService;

            LoadFileCommand = new DelegateCommand(LoadFileCommandHandler);
            CheckBoxActivity = new DelegateCommand<CheckableItem<string>>(CheckBoxActivityHandler);
        }

        private void CheckBoxActivityHandler(CheckableItem<string> obj)
        {
            var dataSets = new Dictionary<string, string>();
            foreach(var item in CheckableItems)
            {
                if (item.IsCheckedLeftAxis)
                {
                    dataSets.Add(item.Value, "primary");
                } else if (item.IsCheckedRightAxis)
                {
                    dataSets.Add(item.Value, "secondary");
                }
            }
            if (CsvFromFileService is not null && dataSets.Count > 0)
            {
                PlotModel = PlotModelHelper.CreateTimeSeriesPlot(dataSets, CsvFromFileService.DataFrame);
            } else
            {
                PlotModel.Series.Clear();
                PlotModel.InvalidatePlot(true);
            }
        }

        private void LoadFileCommandHandler()
        {
            if (_openFileService.OpenFile() == true)
            {
                try
                {
                    CsvFromFileService = new CsvFromFileService(_openFileService.FileNames[0]);

                    Information = $"Start Time: {CsvFromFileService.DataFrame.Columns["TimeStamp"].Cast<DateTime>().First()}" +
                        $" End Time: {CsvFromFileService.DataFrame.Columns["TimeStamp"].Cast<DateTime>().Last()}" +
                        $" Datapoints: {CsvFromFileService.DataFrame.Columns["TimeStamp"].Cast<DateTime>().Count()}";

                    var headers = CsvFromFileService.GetCsvHeader();
                    var checkableItems = new List<CheckableItem<string>>();
                    foreach (var header in headers)
                    {
                        if (CsvFromFileService.DataFrame.Columns[header].IsNumericColumn() && header != "TimeStamp")
                        {
                            checkableItems.Add(new CheckableItem<string>(header));
                        }
                    }
                    CheckableItems = checkableItems;
                } catch (Exception ex)
                {
                    Information = ex.Message;
                }
            }
        }
    }
}
