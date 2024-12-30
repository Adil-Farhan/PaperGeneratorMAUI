namespace QuestionPaperMaker.Views
{
    public partial class QuestionPapersPage : ContentPage
    {
        private readonly QuestionPapersViewModel _viewModel;

        public QuestionPapersPage(QuestionPapersViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadCommand.Execute(null);
        }
    }
} 