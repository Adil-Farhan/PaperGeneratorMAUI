namespace QuestionPaperMaker.Views
{
    public partial class EditQuestionPaperPage : ContentPage
    {
        private EditQuestionPaperViewModel _viewModel;

        public EditQuestionPaperPage(EditQuestionPaperViewModel viewModel)
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