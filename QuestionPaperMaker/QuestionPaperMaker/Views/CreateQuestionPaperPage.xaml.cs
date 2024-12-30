namespace QuestionPaperMaker.Views
{
    public partial class CreateQuestionPaperPage : ContentPage
    {
        private CreateQuestionPaperViewModel _viewModel;

        public CreateQuestionPaperPage(CreateQuestionPaperViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadDataCommand.Execute(null);
        }
    }
} 