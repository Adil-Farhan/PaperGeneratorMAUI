namespace QuestionPaperMaker.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private Settings _settings;

        public string ApiBaseUrl
        {
            get => _settings.ApiBaseUrl;
            set
            {
                if (_settings.ApiBaseUrl != value)
                {
                    _settings.ApiBaseUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DarkMode
        {
            get => _settings.DarkMode;
            set
            {
                if (_settings.DarkMode != value)
                {
                    _settings.DarkMode = value;
                    OnPropertyChanged();
                    Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
                }
            }
        }

        public string DefaultPaperTitle
        {
            get => _settings.DefaultPaperTitle;
            set
            {
                if (_settings.DefaultPaperTitle != value)
                {
                    _settings.DefaultPaperTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DefaultQuestionsPerConfiguration
        {
            get => _settings.DefaultQuestionsPerConfiguration;
            set
            {
                if (_settings.DefaultQuestionsPerConfiguration != value)
                {
                    _settings.DefaultQuestionsPerConfiguration = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand ResetCommand { get; }

        public SettingsViewModel(ISettingsService settingsService)
        {
            Title = "Settings";
            _settingsService = settingsService;
            _settings = _settingsService.GetSettings();

            SaveCommand = new AsyncRelayCommand(OnSave);
            ResetCommand = new AsyncRelayCommand(OnReset);
        }

        private async Task OnSave()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                _settingsService.SaveSettings(_settings);
                await Shell.Current.DisplayAlert("Success", "Settings saved successfully", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnReset()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                _settings = new Settings();
                OnPropertyChanged(nameof(ApiBaseUrl));
                OnPropertyChanged(nameof(DarkMode));
                OnPropertyChanged(nameof(DefaultPaperTitle));
                OnPropertyChanged(nameof(DefaultQuestionsPerConfiguration));
                await OnSave();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 