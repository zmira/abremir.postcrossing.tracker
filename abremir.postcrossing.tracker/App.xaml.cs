using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace abremir.postcrossing.tracker
{
    public class App : Application
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

            base.OnFrameworkInitializationCompleted();
        }

        public static ServiceProvider ServiceProvider { get; set; }
    }
}
