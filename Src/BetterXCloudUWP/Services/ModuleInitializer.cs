using System;

namespace BetterXCloudUWP.Services
{
    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            // Инициализация настроек
            var settings = SettingsManager.Instance;

            // Инициализация состояния приложения
            var state = AppState.Instance;

            // Инициализация событий
            var eventBus = EventBus.Instance;

            // Инициализация потокового менеджера
            var streamManager = StreamManager.Instance;

            // Можно добавить инициализацию других сервисов и модулей по мере переноса
        }
    }
}
