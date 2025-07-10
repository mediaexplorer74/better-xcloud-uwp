using System;

namespace BetterXCloudUWP.Services
{
    public class StreamManager
    {
        private static StreamManager _instance;
        public static StreamManager Instance => _instance ?? (_instance = new StreamManager());

        public event Action StreamStarted;
        public event Action StreamStopped;

        public void StartStream()
        {
            // TODO: Реализовать запуск потоковой передачи
            AppState.Instance.IsPlaying = true;
            StreamStarted?.Invoke();
        }

        public void StopStream()
        {
            // TODO: Реализовать остановку потоковой передачи
            AppState.Instance.IsPlaying = false;
            StreamStopped?.Invoke();
        }
    }
}
