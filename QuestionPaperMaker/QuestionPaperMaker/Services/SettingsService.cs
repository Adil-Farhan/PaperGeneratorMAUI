using System.Text.Json;

namespace QuestionPaperMaker.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsKey = "app_settings";
        private readonly IPreferences _preferences;

        public SettingsService(IPreferences preferences)
        {
            _preferences = preferences;
        }

        public Settings GetSettings()
        {
            var json = _preferences.Get(SettingsKey, string.Empty);
            if (string.IsNullOrEmpty(json))
                return new Settings();

            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }

        public void SaveSettings(Settings settings)
        {
            var json = JsonSerializer.Serialize(settings);
            _preferences.Set(SettingsKey, json);
        }
    }
} 