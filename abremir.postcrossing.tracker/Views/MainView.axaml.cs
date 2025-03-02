using abremir.postcrossing.tracker.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaProgressRing;

namespace abremir.postcrossing.tracker.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel(
                this.FindControl<ToggleButton>("PersistData"),
                this.FindControl<ToggleButton>("StartTracking"),
                this.FindControl<ProgressRing>("ProgressRing"),
                this.FindControl<Slider>("RefreshFrequency"));
        }
    }
}
