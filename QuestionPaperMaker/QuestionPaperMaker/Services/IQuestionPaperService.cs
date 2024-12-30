using QuestionPaperMaker.Models;

namespace QuestionPaperMaker.Services
{
    public interface IQuestionPaperService
    {
        Task<List<QuestionPaper>> GetQuestionPapersAsync();
        Task<QuestionPaper> GetQuestionPaperAsync(int id);
        Task<QuestionPaper> CreateQuestionPaperAsync(QuestionPaper paper);
        Task<QuestionPaper> UpdateQuestionPaperAsync(int id, QuestionPaper paper);
        Task DeleteQuestionPaperAsync(int id);
        Task<byte[]> GeneratePdfAsync(int id);
        
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Difficulty>> GetDifficultiesAsync();
        Task<List<QuestionType>> GetQuestionTypesAsync();
    }
} 