using System.ComponentModel;
using System.Runtime.CompilerServices;
using BetterXCloudUWP.Services;

namespace BetterXCloudUWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            IsPlaying = AppState.Instance.IsPlaying;
            StreamManager.Instance.StreamStarted += () => IsPlaying = true;
            StreamManager.Instance.StreamStopped += () => IsPlaying = false;
        }

        public void StartStream()
        {
            StreamManager.Instance.StartStream();
        }

        public void StopStream()
        {
            StreamManager.Instance.StopStream();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
