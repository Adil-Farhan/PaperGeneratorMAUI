using System.Collections.ObjectModel;
using QuestionPaperMaker.Models;
using QuestionPaperMaker.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace QuestionPaperMaker.ViewModels
{
    [QueryProperty(nameof(QuestionPaperId), "id")]
    public class PreviewQuestionPaperViewModel : BaseViewModel
    {
        private readonly IQuestionPaperService _questionPaperService;
        private int _questionPaperId;
        private QuestionPaper _questionPaper;
        private bool _isGeneratingPdf;

        public int QuestionPaperId
        {
            get => _questionPaperId;
            set
            {
                _questionPaperId = value;
                LoadQuestionPaperAsync().ConfigureAwait(false);
            }
        }

        public QuestionPaper QuestionPaper
        {
            get => _questionPaper;
            set => SetProperty(ref _questionPaper, value);
        }

        public bool IsGeneratingPdf
        {
            get => _isGeneratingPdf;
            set => SetProperty(ref _isGeneratingPdf, value);
        }

        public ICommand GeneratePdfCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public PreviewQuestionPaperViewModel(IQuestionPaperService questionPaperService)
        {
            Title = "Preview Question Paper";
            _questionPaperService = questionPaperService;

            GeneratePdfCommand = new AsyncRelayCommand(OnGeneratePdf);
            EditCommand = new AsyncRelayCommand(OnEdit);
            DeleteCommand = new AsyncRelayCommand(OnDelete);
        }

        private async Task LoadQuestionPaperAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                QuestionPaper = await _questionPaperService.GetQuestionPaperAsync(QuestionPaperId);
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

        private async Task OnGeneratePdf()
        {
            if (IsGeneratingPdf) return;

            try
            {
                IsGeneratingPdf = true;
                var pdfBytes = await _questionPaperService.GeneratePdfAsync(QuestionPaperId);

                var fileName = $"QuestionPaper_{QuestionPaperId}.pdf";
                var file = Path.Combine(FileSystem.CacheDirectory, fileName);
                await File.WriteAllBytesAsync(file, pdfBytes);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = QuestionPaper?.Title ?? "Question Paper",
                    File = new ShareFile(file)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsGeneratingPdf = false;
            }
        }

        private async Task OnEdit()
        {
            await Shell.Current.GoToAsync($"../EditQuestionPaper?id={QuestionPaperId}");
        }

        private async Task OnDelete()
        {
            if (IsBusy) return;

            var confirm = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this question paper?",
                "Yes",
                "No"
            );

            if (!confirm) return;

            try
            {
                IsBusy = true;
                await _questionPaperService.DeleteQuestionPaperAsync(QuestionPaperId);
                await Shell.Current.GoToAsync("../..");
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