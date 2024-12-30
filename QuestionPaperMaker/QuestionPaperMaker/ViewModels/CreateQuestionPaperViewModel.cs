using System.Collections.ObjectModel;
using QuestionPaperMaker.Models;
using QuestionPaperMaker.Services;
using System.Windows.Input;

namespace QuestionPaperMaker.ViewModels
{
    public class CreateQuestionPaperViewModel : BaseViewModel
    {
        private readonly IQuestionPaperService _questionPaperService;
        private readonly ISettingsService _settingsService;

        private string _paperTitle;
        private string _description;
        private Category _selectedCategory;
        private Difficulty _selectedDifficulty;
        private QuestionType _selectedQuestionType;
        private int _numQuestions;

        public string PaperTitle
        {
            get => _paperTitle;
            set => SetProperty(ref _paperTitle, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public Difficulty SelectedDifficulty
        {
            get => _selectedDifficulty;
            set => SetProperty(ref _selectedDifficulty, value);
        }

        public QuestionType SelectedQuestionType
        {
            get => _selectedQuestionType;
            set => SetProperty(ref _selectedQuestionType, value);
        }

        public int NumQuestions
        {
            get => _numQuestions;
            set => SetProperty(ref _numQuestions, value);
        }

        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Difficulty> Difficulties { get; } = new();
        public ObservableCollection<QuestionType> QuestionTypes { get; } = new();
        public ObservableCollection<QuestionConfiguration> Configurations { get; } = new();

        public ICommand LoadDataCommand { get; }
        public ICommand AddConfigurationCommand { get; }
        public ICommand RemoveConfigurationCommand { get; }
        public ICommand SaveCommand { get; }

        public CreateQuestionPaperViewModel(IQuestionPaperService questionPaperService, ISettingsService settingsService)
        {
            Title = "Create Question Paper";
            _questionPaperService = questionPaperService;
            _settingsService = settingsService;

            var settings = settingsService.GetSettings();
            PaperTitle = settings.DefaultPaperTitle;
            NumQuestions = settings.DefaultQuestionsPerConfiguration;

            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            AddConfigurationCommand = new AsyncRelayCommand(OnAddConfiguration);
            RemoveConfigurationCommand = new AsyncRelayCommand<QuestionConfiguration>(OnRemoveConfiguration);
            SaveCommand = new AsyncRelayCommand(OnSave);
        }

        private async Task LoadDataAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var categories = await _questionPaperService.GetCategoriesAsync();
                var difficulties = await _questionPaperService.GetDifficultiesAsync();
                var types = await _questionPaperService.GetQuestionTypesAsync();

                Categories.Clear();
                Difficulties.Clear();
                QuestionTypes.Clear();

                foreach (var category in categories)
                    Categories.Add(category);
                foreach (var difficulty in difficulties)
                    Difficulties.Add(difficulty);
                foreach (var type in types)
                    QuestionTypes.Add(type);
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

        private async Task OnAddConfiguration()
        {
            if (SelectedCategory == null)
            {
                await Shell.Current.DisplayAlert("Error", "Please select a category", "OK");
                return;
            }

            var config = new QuestionConfiguration
            {
                CategoryId = SelectedCategory.Id,
                DifficultyId = SelectedDifficulty?.Id ?? 0,
                QuestionTypeId = SelectedQuestionType?.Id ?? 0,
                NumQuestions = NumQuestions
            };

            Configurations.Add(config);
        }

        private Task OnRemoveConfiguration(QuestionConfiguration config)
        {
            if (config != null)
                Configurations.Remove(config);
            return Task.CompletedTask;
        }

        private async Task OnSave()
        {
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(PaperTitle))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a title", "OK");
                return;
            }

            if (!Configurations.Any())
            {
                await Shell.Current.DisplayAlert("Error", "Please add at least one configuration", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var paper = new QuestionPaper
                {
                    Title = PaperTitle,
                    Description = Description,
                    Configurations = Configurations.ToList()
                };

                await _questionPaperService.CreateQuestionPaperAsync(paper);
                await Shell.Current.GoToAsync("..");
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
    }
} 