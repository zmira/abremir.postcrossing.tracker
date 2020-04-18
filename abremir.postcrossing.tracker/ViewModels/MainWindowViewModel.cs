using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Services;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaProgressRing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace abremir.postcrossing.tracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Send> PostcrossingEventsSend { get; } = new ObservableCollection<Send>(new List<Send>());
        public ObservableCollection<Register> PostcrossingEventsRegister { get; } = new ObservableCollection<Register>(new List<Register>());

        private readonly ToggleButton _persistToggle;
        private readonly ToggleButton _trackingToggle;
        private readonly ProgressRing _progressRing;
        private readonly Slider _frequencySlider;
        private readonly IPostcrossingEventService _postcrossingEventService;
        private readonly IPostcrossingEngineSettingsService _postcrossingEngineSettingsService;
        private readonly SemaphoreSlim _semaphore;

        private Timer _timer;

        public MainWindowViewModel(
            ToggleButton persistToggle,
            ToggleButton trackingToggle,
            ProgressRing progressRing,
            Slider frequencySlider)
        {
            _persistToggle = persistToggle;
            _trackingToggle = trackingToggle;
            _progressRing = progressRing;
            _frequencySlider = frequencySlider;

            _persistToggle.Click += OnPersistToggleClick;
            _trackingToggle.Click += OnTrackingToggleClick;

            _postcrossingEngineSettingsService = App.ServiceProvider.GetService<IPostcrossingEngineSettingsService>();
            _postcrossingEventService = App.ServiceProvider.GetService<IPostcrossingEventService>();

            _semaphore = new SemaphoreSlim(1);
        }

        private void OnTrackingToggleClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton))
            {
                return;
            }

            var startTracking = toggleButton.IsChecked ?? false;

            _frequencySlider.IsEnabled = !startTracking;

            if (startTracking)
            {
                toggleButton.Content = "Stop Tracking";
                GetEvents();
                _timer = new Timer(_frequencySlider.Value * 1000);
                _timer.Elapsed += Timer_Elapsed;
                _timer.Start();
            }
            else
            {
                toggleButton.Content = "Start Tracking";
                _timer?.Stop();
                _timer?.Dispose();
                _timer = null;
            }
        }

        private void OnPersistToggleClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton))
            {
                return;
            }

            _postcrossingEngineSettingsService.PersistData = toggleButton.IsChecked ?? false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ((Timer)sender).Stop();
            GetEvents();
            ((Timer)sender).Start();
        }

        private async void GetEvents()
        {
            await _semaphore.WaitAsync();
            await Dispatcher.UIThread.InvokeAsync(() => _progressRing.IsActive = true);

            foreach (var @event in await _postcrossingEventService.GetLatestEventsAsync())
            {
                switch (@event.EventType)
                {
                    case PostcrossingEventTypeEnum.Send:
                        await Dispatcher.UIThread.InvokeAsync(() => PostcrossingEventsSend.Insert(0, @event as Send));
                        break;
                    case PostcrossingEventTypeEnum.Register:
                        await Dispatcher.UIThread.InvokeAsync(() => PostcrossingEventsRegister.Insert(0, @event as Register));
                        break;
                }
            }

            await Dispatcher.UIThread.InvokeAsync(() => _progressRing.IsActive = false);
            _semaphore.Release();
        }
    }
}
