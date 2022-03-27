using OxyPlot;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using SystemPerformanceAnalyser.Dialogs;
using SystemPerformanceAnalyser.Helper;
using SystemPerformanceAnalyser.Interfaces;
using SystemPerformanceAnalyser.Model;
using SystemPerformanceAnalyser.Services;

namespace SystemPerformanceAnalyser.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IOpenFileService _openFileService;
        private readonly ISaveFileService _saveFileService;

        private readonly IDialogService _dialogService;

        private CsvContent? CsvContent;
        private DataStatistics? _dataStatistics;

        private string _title = "";
        private string _plotTitle = "";
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

        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetProperty(ref _plotModel, value);
        }

        public DataStatistics? DataStatistics
        {
            get => _dataStatistics;
            set => SetProperty(ref _dataStatistics, value);
        }

        public DelegateCommand LoadFileCommand { get; set; }
        public DelegateCommand EditAnnotationCommand { get; set; }
        public DelegateCommand UpdateStatisticsCommand { get; set; }
        public DelegateCommand<object> ExportCommand { get; set; }
        public DelegateCommand<CheckableItem<string>> CheckBoxActivity { get; set; }
        public MainViewModel(IOpenFileService openFileService, 
            ISaveFileService saveFileService,
            IDialogService dialogService)
        {
            Title = "System Performance Anaylser";

            _openFileService = openFileService;
            _saveFileService = saveFileService;
            _dialogService = dialogService;

            LoadFileCommand = new DelegateCommand(LoadFileCommandHandlerAsync);
            EditAnnotationCommand = new DelegateCommand(EditAnnotationCommandHandler);
            UpdateStatisticsCommand = new DelegateCommand(UpdateStatisticsCommandHandler);
            ExportCommand = new DelegateCommand<object>(ExportCommandHandler);
            CheckBoxActivity = new DelegateCommand<CheckableItem<string>>(CheckBoxActivityHandler);
        }

        private void UpdateStatisticsCommandHandler()
        {
            if (CsvContent is not null)
            {
                DataStatistics = new DataStatistics(CsvContent.DataFrame,
                    PlotModelHelper.GetDateTimeFromAxis(PlotModel.DefaultXAxis.ActualMinimum),
                    PlotModelHelper.GetDateTimeFromAxis(PlotModel.DefaultXAxis.ActualMaximum),
                    CheckableItems);
            }
        }

        private void EditAnnotationCommandHandler()
        {
            var annotationSettings = new AnnotationSettings
            {
                Title = PlotModel.Title,
                PrimaryYAxisLabel = PlotModelHelper.GetAxisTitle(PlotModel, PlotModelHelper.AxisType.PrimaryY),
                SecondaryYAxisLabel = PlotModelHelper.GetAxisTitle(PlotModel, PlotModelHelper.AxisType.SecondaryY)
            };

            _dialogService.ShowAnnotationDialogDialog(annotationSettings, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var annotations = r.Parameters.GetValue<AnnotationSettings>("AnnotationSettings");
                    PlotModel.Title = annotations.Title;
                    PlotModelHelper.ChangeAxisTitle(PlotModel, annotations.PrimaryYAxisLabel, PlotModelHelper.AxisType.PrimaryY);
                    PlotModelHelper.ChangeAxisTitle(PlotModel, annotations.SecondaryYAxisLabel, PlotModelHelper.AxisType.SecondaryY);
                    PlotModel.InvalidatePlot(true);
                }
            });
        }

        private void ExportCommandHandler(object obj)
        {
            if (_saveFileService.SaveFile() == true)
                ExportFrameworkElement.AsPng(obj, _saveFileService.File);
        }

        private void CheckBoxActivityHandler(CheckableItem<string> obj)
        {
            var dataSets = new Dictionary<string, PlotModelHelper.AxisType>();
            foreach(var item in CheckableItems)
            {
                if (item.IsCheckedLeftAxis)
                {
                    dataSets.Add(item.Value, PlotModelHelper.AxisType.PrimaryY);
                } else if (item.IsCheckedRightAxis)
                {
                    dataSets.Add(item.Value, PlotModelHelper.AxisType.SecondaryY);
                }
            }
            if (CsvContent is not null && dataSets.Count > 0)
            {
                PlotModel = PlotModelHelper.CreateTimeSeriesPlot(dataSets, CsvContent.DataFrame);
            } else
            {
                PlotModel.Series.Clear();
                PlotModel.InvalidatePlot(true);
            }
        }

        private async void LoadFileCommandHandlerAsync()
        {
            if (_openFileService.OpenFile() == true)
            {
                try
                {
                    CsvContent = await CsvFromFileService.LoadFileAsync(_openFileService.File);
                    var headers = CsvContent.ColumnHeaders;

                    var checkableItems = new List<CheckableItem<string>>();
                    foreach (var header in headers)
                    {
                        if (CsvContent.DataFrame.Columns[header].IsNumericColumn() && header != "TimeStamp")
                        {
                            checkableItems.Add(new CheckableItem<string>(header));
                        }
                    }
                    CheckableItems = checkableItems;
                    Information = _openFileService.FileName;
                } catch (Exception ex)
                {
                    Information = ex.Message;
                }
            }
        }
    }
}
