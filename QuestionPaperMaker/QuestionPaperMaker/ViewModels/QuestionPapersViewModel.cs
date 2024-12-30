using System.Collections.ObjectModel;
using QuestionPaperMaker.Models;
using QuestionPaperMaker.Services;
using System.Windows.Input;

namespace QuestionPaperMaker.ViewModels
{
    public class QuestionPapersViewModel : BaseViewModel
    {
        private readonly IQuestionPaperService _questionPaperService;
        public ObservableCollection<QuestionPaper> QuestionPapers { get; }
        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PreviewCommand { get; }

        public QuestionPapersViewModel(IQuestionPaperService questionPaperService)
        {
            Title = "Question Papers";
            _questionPaperService = questionPaperService;
            QuestionPapers = new ObservableCollection<QuestionPaper>();

            LoadCommand = new AsyncRelayCommand(LoadQuestionPapersAsync);
            AddCommand = new AsyncRelayCommand(OnAddQuestionPaper);
            EditCommand = new AsyncRelayCommand<QuestionPaper>(OnEditQuestionPaper);
            DeleteCommand = new AsyncRelayCommand<QuestionPaper>(OnDeleteQuestionPaper);
            PreviewCommand = new AsyncRelayCommand<QuestionPaper>(OnPreviewQuestionPaper);
        }

        private async Task LoadQuestionPapersAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var papers = await _questionPaperService.GetQuestionPapersAsync();
                QuestionPapers.Clear();
                foreach (var paper in papers)
                {
                    QuestionPapers.Add(paper);
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

        private async Task OnAddQuestionPaper()
        {
            await Shell.Current.GoToAsync("CreateQuestionPaper");
        }

        private async Task OnEditQuestionPaper(QuestionPaper paper)
        {
            if (paper == null)
                return;

            await Shell.Current.GoToAsync($"EditQuestionPaper?id={paper.Id}");
        }

        private async Task OnDeleteQuestionPaper(QuestionPaper paper)
        {
            if (paper == null)
                return;

            try
            {
                IsBusy = true;
                await _questionPaperService.DeleteQuestionPaperAsync(paper.Id);
                QuestionPapers.Remove(paper);
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

        private async Task OnPreviewQuestionPaper(QuestionPaper paper)
        {
            if (paper == null)
                return;

            await Shell.Current.GoToAsync($"PreviewQuestionPaper?id={paper.Id}");
        }
    }
} 