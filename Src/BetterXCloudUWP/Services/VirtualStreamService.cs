using System;
using System.Threading.Tasks;

namespace BetterXCloudUWP.Services
{
    public class VirtualStreamService
    {
        public event Action<string> StatusChanged;
        public event Action<int> FpsChanged;

        private bool _isStreaming = false;
        private int _fps = 0;
        private Task _streamTask;

        public void Start()
        {
            if (_isStreaming) return;
            _isStreaming = true;
            StatusChanged?.Invoke("Стрим запущен");
            _streamTask = Task.Run(async () =>
            {
                Random rnd = new Random();
                while (_isStreaming)
                {
                    _fps = rnd.Next(50, 61); // Имитация FPS
                    FpsChanged?.Invoke(_fps);
                    await Task.Delay(1000);
                }
            });
        }

        public void Stop()
        {
            if (!_isStreaming) return;
            _isStreaming = false;
            StatusChanged?.Invoke("Стрим остановлен");
        }
    }
}
