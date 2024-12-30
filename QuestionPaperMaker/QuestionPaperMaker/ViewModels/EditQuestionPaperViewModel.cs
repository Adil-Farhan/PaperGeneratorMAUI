using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace QuestionPaperMaker.ViewModels
{
    [QueryProperty(nameof(QuestionPaperId), "id")]
    public class EditQuestionPaperViewModel : BaseViewModel
    {
        private readonly IQuestionPaperService _questionPaperService;
        private int _questionPaperId;
        private string _paperTitle;
        private string _description;
        private Category _selectedCategory;
        private Difficulty _selectedDifficulty;
        private QuestionType _selectedQuestionType;
        private int _numQuestions;

        public int QuestionPaperId
        {
            get => _questionPaperId;
            set
            {
                _questionPaperId = value;
                LoadQuestionPaperAsync().ConfigureAwait(false);
            }
        }

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

        public EditQuestionPaperViewModel(IQuestionPaperService questionPaperService)
        {
            Title = "Edit Question Paper";
            _questionPaperService = questionPaperService;

            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            AddConfigurationCommand = new AsyncRelayCommand(OnAddConfiguration);
            RemoveConfigurationCommand = new AsyncRelayCommand<QuestionConfiguration>(OnRemoveConfiguration);
            SaveCommand = new AsyncRelayCommand(OnSave);
        }

        private async Task LoadQuestionPaperAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await LoadDataAsync();

                var paper = await _questionPaperService.GetQuestionPaperAsync(QuestionPaperId);
                PaperTitle = paper.Title;
                Description = paper.Description;

                Configurations.Clear();
                foreach (var config in paper.Configurations)
                {
                    Configurations.Add(config);
                }
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

        private async Task LoadDataAsync()
        {
            try
            {
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
                    Id = QuestionPaperId,
                    Title = PaperTitle,
                    Description = Description,
                    Configurations = Configurations.ToList()
                };

                await _questionPaperService.UpdateQuestionPaperAsync(QuestionPaperId, paper);
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