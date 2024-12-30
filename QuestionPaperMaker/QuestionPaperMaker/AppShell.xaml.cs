namespace QuestionPaperMaker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute("CreateQuestionPaper", typeof(CreateQuestionPaperPage));
            Routing.RegisterRoute("EditQuestionPaper", typeof(EditQuestionPaperPage));
            Routing.RegisterRoute("PreviewQuestionPaper", typeof(PreviewQuestionPaperPage));
        }
    }
} 