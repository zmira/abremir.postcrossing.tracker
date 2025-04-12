using abremir.postcrossing.tracker.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace abremir.postcrossing.tracker
{
    public partial class App : Application
    {
        public App()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.RegisterPostcrossingEngine();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public static ServiceProvider ServiceProvider { get; set; }
    }
}
