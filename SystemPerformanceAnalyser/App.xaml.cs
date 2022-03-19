using Prism.Ioc;
using Prism.Unity;
using System.Windows;
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
        }
    }
}
