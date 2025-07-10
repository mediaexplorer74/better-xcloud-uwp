using Windows.UI.Xaml.Controls;

namespace BetterXCloudUWP.Views
{
    public sealed partial class NotificationControl : UserControl
    {
        public NotificationControl()
        {
            this.InitializeComponent();
        }

        public void Show(string message)
        {
            NotificationText.Text = message;
            this.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
