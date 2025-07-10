namespace BetterXCloudUWP.Services
{
    public class AppState
    {
        private static AppState _instance;
        public static AppState Instance => _instance ?? (_instance = new AppState());

        public bool IsPlaying { get; set; } = false;
        public object CurrentStream { get; set; } = null;
        // Можно добавить дополнительные свойства по мере переноса логики
    }
}
