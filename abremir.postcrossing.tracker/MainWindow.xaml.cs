using abremir.postcrossing.tracker.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
//using AvaloniaProgressRing;

namespace abremir.postcrossing.tracker
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            DataContext = new MainWindowViewModel(
                this.FindControl<ToggleButton>("PersistData"),
                this.FindControl<ToggleButton>("StartTracking"),
                //this.FindControl<ProgressRing>("ProgressRing"),
                this.FindControl<Slider>("RefreshFrequency"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
