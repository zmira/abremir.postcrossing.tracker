using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
