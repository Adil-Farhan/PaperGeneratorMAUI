namespace QuestionPaperMaker.Services
{
    public interface ISettingsService
    {
        Settings GetSettings();
        void SaveSettings(Settings settings);
    }

    public class Settings
    {
        public string ApiBaseUrl { get; set; } = AppConstants.DefaultApiBaseUrl;
        public bool DarkMode { get; set; }
        public string DefaultPaperTitle { get; set; } = "New Question Paper";
        public int DefaultQuestionsPerConfiguration { get; set; } = 5;
    }
} 