using System;
using Windows.UI.Xaml.Controls;

namespace BetterXCloudUWP
{
    public sealed partial class MainPage : Page
    {
        private ViewModels.MainViewModel ViewModel;
        private Services.VirtualStreamService StreamService;
        private Services.GamepadService GamepadService;
        private Windows.UI.Xaml.DispatcherTimer GamepadTimer;
        private Services.InputService InputService;

        public MainPage()
        {
            this.InitializeComponent();
            Services.ModuleInitializer.Initialize();
            ViewModel = new ViewModels.MainViewModel();
            UpdateStatus();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // Stream service
            StreamService = new Services.VirtualStreamService();
            StreamService.StatusChanged += (status) =>
            {
                StatusText.Text = status;
            };
            StreamService.FpsChanged += (fps) =>
            {
                FpsText.Text = $"FPS: {fps}";
            };

            // Gamepad service
            GamepadService = new Services.GamepadService();
            GamepadService.GamepadStatusChanged += (msg) =>
            {
                GamepadStatusText.Text = $"Геймпад: {msg}";
                ShowNotification($"{msg}");
            };
            GamepadService.GamepadButtonPressed += (btn) =>
            {
                ShowNotification($"Нажата кнопка: {btn}");
            };
            GamepadStatusText.Text = "Геймпад: не подключен";

            // Gamepad polling timer
            GamepadTimer = new Windows.UI.Xaml.DispatcherTimer();
            GamepadTimer.Interval = TimeSpan.FromMilliseconds(300);
            GamepadTimer.Tick += (s, e) => GamepadService.PollGamepads();
            GamepadTimer.Start();

            // Input service
            InputService = new Services.InputService();
            InputService.InputEvent += (msg) =>
            {
                InputText.Text = msg;
                ShowNotification(msg);
            };
            InputService.Attach(InputArea);
            //InputArea.Focus(FocusState.Programmatic);
        }

        // Обработчики для XAML, чтобы не терять фокус и поддерживать ввод
        private void InputArea_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            InputService?.GetType().GetMethod("Element_KeyDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(InputService, new object[] { sender, e });
        }
        private void InputArea_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            InputService?.GetType().GetMethod("Element_KeyUp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(InputService, new object[] { sender, e });
        }
        private void InputArea_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //InputArea.Focus(FocusState.Programmatic);
            InputService?.GetType().GetMethod("Element_PointerPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(InputService, new object[] { sender, e });
        }
        private void InputArea_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            InputService?.GetType().GetMethod("Element_PointerMoved", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(InputService, new object[] { sender, e });
        }
        private void InputArea_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            InputService?.GetType().GetMethod("Element_PointerReleased", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(InputService, new object[] { sender, e });
        }

        private void SettingsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Views.SettingsPage));
        }

        private void XboxAuthButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Views.XboxAuthPage));
        }

        private void StreamPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Views.StreamPage));
        }

        private void StartStreamButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.StartStream();
            StreamService.Start();
            ShowNotification("Стрим запущен");
        }

        private void StopStreamButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.StopStream();
            StreamService.Stop();
            ShowNotification("Стрим остановлен");
            FpsText.Text = "FPS: 0";
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModels.MainViewModel.IsPlaying))
                UpdateStatus();
        }

        private void UpdateStatus()
        {
            StatusText.Text = ViewModel.IsPlaying ? "Статус: Запущен" : "Статус: Остановлен";
        }

        public async void ShowNotification(string message)
        {
            Notification.Show(message);
            await System.Threading.Tasks.Task.Delay(2000);
            Notification.Hide();
        }
    }
}
