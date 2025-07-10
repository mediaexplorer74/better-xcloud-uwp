using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BetterXCloudUWP.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BetterXCloudUWP.Views
{
    public sealed partial class XboxProfilePage : Page
    {
        private XboxAuthService _authService = new XboxAuthService();
        private XboxProfileService _profileService;
        private ObservableCollection<string> _games = new ObservableCollection<string>();
        private ObservableCollection<string> _achievements = new ObservableCollection<string>();

        public XboxProfilePage()
        {
            this.InitializeComponent();
            LoadProfile();
        }

        private async void LoadProfile()
        {
            if (!_authService.IsSignedIn)
            {
                GamerTagText.Text = "Не авторизовано";
                return;
            }
            _profileService = new XboxProfileService(_authService.User);
            GamerTagText.Text = await _profileService.GetGamerTagAsync();
        }

        private async void ShowGamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesList.Visibility = Visibility.Visible;
            AchievementsList.Visibility = Visibility.Collapsed;
            _games.Clear();
            var games = await _profileService.GetGamesAsync();
            foreach (var g in games)
                _games.Add(g);
            GamesList.ItemsSource = _games;
        }

        private async void ShowAchievementsButton_Click(object sender, RoutedEventArgs e)
        {
            AchievementsList.Visibility = Visibility.Visible;
            GamesList.Visibility = Visibility.Collapsed;
            _achievements.Clear();
            var achs = await _profileService.GetAchievementsAsync();
            foreach (var a in achs)
                _achievements.Add(a);
            AchievementsList.ItemsSource = _achievements;
        }
    }
}
