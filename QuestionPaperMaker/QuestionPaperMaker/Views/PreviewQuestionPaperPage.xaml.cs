namespace QuestionPaperMaker.Views
{
    public partial class PreviewQuestionPaperPage : ContentPage
    {
        private PreviewQuestionPaperViewModel _viewModel;

        public PreviewQuestionPaperPage(PreviewQuestionPaperViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }
    }
} 