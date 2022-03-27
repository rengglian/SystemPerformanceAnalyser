using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using SystemPerformanceAnalyser.Dialogs.Annotations.ViewModels;
using SystemPerformanceAnalyser.Dialogs.Annotations.Views;
using SystemPerformanceAnalyser.Interfaces;
using SystemPerformanceAnalyser.Services;
using SystemPerformanceAnalyser.Views;

namespace SystemPerformanceAnalyser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<OpenFileService>(typeof(IOpenFileService));
            containerRegistry.RegisterManySingleton<SaveFileService>(typeof(ISaveFileService));

            containerRegistry.RegisterDialog<AnnotationDialogView, AnnotationDialogViewModel>();
        }
    }
}
