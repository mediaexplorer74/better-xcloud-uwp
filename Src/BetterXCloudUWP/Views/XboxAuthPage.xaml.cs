using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BetterXCloudUWP.Services;

namespace BetterXCloudUWP.Views
{
    public sealed partial class XboxAuthPage : Page
    {
        private XboxAuthService _authService = new XboxAuthService();
        public XboxAuthPage()
        {
            this.InitializeComponent();
            _authService.AuthStatusChanged += (msg) =>
            {
                StatusText.Text = msg;
            };
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Вход...";
            bool success = await _authService.SignInAsync(Dispatcher); // Pass the CoreDispatcher instance
            if (success)
            {
                StatusText.Text = "Вход выполнен!";
                Frame.Navigate(typeof(XboxProfilePage));
            }
            else
            {
                StatusText.Text = "Ошибка входа.";
            }
        }
    }
}
