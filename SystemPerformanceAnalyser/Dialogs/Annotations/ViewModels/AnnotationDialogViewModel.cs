using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using SystemPerformanceAnalyser.Model;

namespace SystemPerformanceAnalyser.Dialogs.Annotations.ViewModels
{
    public class AnnotationDialogViewModel : BindableBase, IDialogAware
    {
        private string _title = "Annotation Settings";

        private DelegateCommand<string>? _closeDialogCommand;
        private AnnotationSettings? _annotationSettings;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public AnnotationSettings? AnnotationSettings
        {
            get => _annotationSettings;
            set => SetProperty(ref _annotationSettings, value);
        }

        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ??= new DelegateCommand<string>(CloseDialog);

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            AnnotationSettings = parameters.GetValue<AnnotationSettings>("AnnotationSettings");
        }

        protected virtual void CloseDialog(string parameter)
        {
            var p = new DialogParameters
            {
                {"AnnotationSettings", AnnotationSettings }
            };
            RaiseRequestClose(new DialogResult(parameter == "true" ? ButtonResult.OK : ButtonResult.Cancel, p));
        }
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}
