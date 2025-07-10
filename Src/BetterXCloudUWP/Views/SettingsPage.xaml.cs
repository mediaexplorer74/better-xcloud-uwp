using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BetterXCloudUWP.Services;

namespace BetterXCloudUWP.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            UserNameBox.Text = SettingsManager.Instance.Get<string>("UserName", "");
            XCloudEndpointBox.Text = SettingsManager.Instance.Get<string>("XCloudEndpoint", "https://xbox-cloud-gaming-endpoint/sessions/cloud/play");
            DefaultRegionBox.Text = SettingsManager.Instance.Get<string>("XCloudDefaultRegion", "eastus");
            DefaultTitleIdBox.Text = SettingsManager.Instance.Get<string>("XCloudDefaultTitleId", "");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.Instance.Set("UserName", UserNameBox.Text);
            SettingsManager.Instance.Set("XCloudEndpoint", XCloudEndpointBox.Text);
            SettingsManager.Instance.Set("XCloudDefaultRegion", DefaultRegionBox.Text);
            SettingsManager.Instance.Set("XCloudDefaultTitleId", DefaultTitleIdBox.Text);
            SaveStatus.Text = "Сохранено!";
            SaveStatus.Visibility = Visibility.Visible;

            // Показать уведомление на главной странице при возврате
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                var mainPage = (MainPage)((Frame)Window.Current.Content).Content;
                mainPage?.ShowNotification("Настройки сохранены");
            }
        }
    }
}
