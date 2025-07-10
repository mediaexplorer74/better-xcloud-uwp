using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BetterXCloudUWP.Services;
using System.Threading.Tasks;
using System;
using Windows.UI.Xaml.Media;

namespace BetterXCloudUWP.Views
{
    public sealed partial class StreamPage : Page
    {
        public StreamPage()
        {
            this.InitializeComponent();
        }

        private async void PlayMediaElement_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = string.Empty;
            StatusTextBlock.Text = "Инициализация...";
            LoadingProgressBar.Visibility = Visibility.Visible;

            var url = StreamUrlBox.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;

            string licenseUrl = null;
            string token = null;
            if (url.StartsWith("xcloud://"))
            {
                StatusTextBlock.Text = "Запрос параметров сессии Xbox Cloud...";
                try
                {
                    var parts = url.Substring(9).Split(':');
                    if (parts.Length == 3)
                    {
                        var gameId = parts[0];
                        var region = parts[1];
                        var xsts = parts[2];
                        var sessionService = new XCloudSessionService();
                        var session = await sessionService.GetStreamSessionAsync(xsts, gameId, region);
                        url = session.streamUrl;
                        licenseUrl = session.licenseUrl;
                        token = session.token;
                    }
                }
                catch (Exception ex)
                {
                    ErrorTextBlock.Text = "Ошибка получения сессии Xbox Cloud: " + ex.Message;
                    LoadingProgressBar.Visibility = Visibility.Collapsed;
                    StatusTextBlock.Text = "";
                    return;
                }
            }

            StreamWebView.Visibility = Visibility.Collapsed;
            StreamMediaElement.Visibility = Visibility.Visible;
            if (!string.IsNullOrEmpty(licenseUrl))
            {
                StatusTextBlock.Text = "Инициализация DRM (PlayReady)...";
                try
                {
                    PlayReadyHelper.EnablePlayReady(StreamMediaElement, licenseUrl, token);
                }
                catch (Exception ex)
                {
                    ErrorTextBlock.Text = "Ошибка DRM: " + ex.Message;
                    LoadingProgressBar.Visibility = Visibility.Collapsed;
                    StatusTextBlock.Text = "";
                    return;
                }
            }
            StatusTextBlock.Text = "Загрузка потока...";
            StreamMediaElement.Source = new Uri(url);
            StreamMediaElement.Play();
        }

        private async void PlayWebView2_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = string.Empty;
            StatusTextBlock.Text = "Инициализация WebView2...";
            LoadingProgressBar.Visibility = Visibility.Visible;

            var url = StreamUrlBox.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;
            StreamMediaElement.Visibility = Visibility.Collapsed;
            StreamWebView.Visibility = Visibility.Visible;
            if (url.StartsWith("xcloud://"))
            {
                StatusTextBlock.Text = "Запрос параметров сессии Xbox Cloud...";
                try
                {
                    var parts = url.Substring(9).Split(':');
                    if (parts.Length == 3)
                    {
                        var gameId = parts[0];
                        var region = parts[1];
                        var xsts = parts[2];
                        var sessionService = new XCloudSessionService();
                        var session = await sessionService.GetStreamSessionAsync(xsts, gameId, region);
                        url = session.streamUrl;
                        StreamWebView.Navigate(new Uri(url));
                        StreamWebView.NavigationCompleted += (s, e2) =>
                        {
                            StatusTextBlock.Text = "Передача токена авторизации через JS...";
                            string js = $@"
                                var xhr = new XMLHttpRequest();
                                xhr.open('GET', '{url}', true);
                                xhr.setRequestHeader('Authorization', '{session.token}');
                                xhr.send();";
                            StreamWebView.InvokeScriptAsync("eval", new[] { js });
                        };
                        LoadingProgressBar.Visibility = Visibility.Collapsed;
                        StatusTextBlock.Text = "";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ErrorTextBlock.Text = "Ошибка получения сессии Xbox Cloud: " + ex.Message;
                    LoadingProgressBar.Visibility = Visibility.Collapsed;
                    StatusTextBlock.Text = "";
                    return;
                }
            }
            StreamWebView.Navigate(new Uri(url));
            LoadingProgressBar.Visibility = Visibility.Collapsed;
            StatusTextBlock.Text = "";
        }
        private void StreamMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "Поток успешно загружен.";
            LoadingProgressBar.Visibility = Visibility.Collapsed;
        }

        private void StreamMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ErrorTextBlock.Text = "Ошибка воспроизведения: " + e.ErrorMessage;
            LoadingProgressBar.Visibility = Visibility.Collapsed;
            StatusTextBlock.Text = "";
        }

        private void StreamMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (StreamMediaElement.CurrentState)
            {
                case MediaElementState.Buffering:
                    StatusTextBlock.Text = "Буферизация...";
                    LoadingProgressBar.Visibility = Visibility.Visible;
                    break;
                case MediaElementState.Playing:
                    StatusTextBlock.Text = "Воспроизведение...";
                    LoadingProgressBar.Visibility = Visibility.Collapsed;
                    break;
                case MediaElementState.Paused:
                    StatusTextBlock.Text = "Пауза.";
                    break;
                case MediaElementState.Stopped:
                    StatusTextBlock.Text = "Остановлено.";
                    break;
                default:
                    break;
            }
        }
    }
}
