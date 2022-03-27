using Prism.Services.Dialogs;
using System;
using SystemPerformanceAnalyser.Model;

namespace SystemPerformanceAnalyser.Dialogs
{
    public static class IDialogServiceExtensions
    {
        public static void ShowAnnotationDialogDialog(this IDialogService dialogService, AnnotationSettings annotationSettings, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "AnnotationSettings", annotationSettings}
            };

            dialogService.ShowDialog("AnnotationDialogView", p, callback);
        }
    }
}
