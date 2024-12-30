namespace QuestionPaperMaker.Services
{
    public interface IPrintService
    {
        Task PrintAsync(QuestionPaper paper, IEnumerable<Question> questions);
    }
} 