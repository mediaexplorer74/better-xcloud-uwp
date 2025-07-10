using System.Collections.Generic;

namespace BetterXCloudUWP.Services
{
    public class SettingsManager
    {
        private static SettingsManager _instance;
        private readonly Dictionary<string, object> _settings = new Dictionary<string, object>();
        private readonly Windows.Storage.ApplicationDataContainer _localSettings;

        private SettingsManager() {
            _localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            LoadAll();
        }

        public static SettingsManager Instance => _instance ?? (_instance = new SettingsManager());

        public T Get<T>(string key, T defaultValue = default)
        {
            if (_settings.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return defaultValue;
        }

        public void Set<T>(string key, T value)
        {
            _settings[key] = value;
            Save(key);
        }

        public void Save(string key)
        {
            if (_settings.TryGetValue(key, out object value))
            {
                _localSettings.Values[key] = value;
            }
        }

        public void Load(string key)
        {
            if (_localSettings.Values.TryGetValue(key, out object value))
            {
                _settings[key] = value;
            }
        }

        public void SaveAll()
        {
            foreach (var key in _settings.Keys)
            {
                Save(key);
            }
        }

        public void LoadAll()
        {
            foreach (var pair in _localSettings.Values)
            {
                _settings[pair.Key] = pair.Value;
            }
        }
    }
}
