namespace QuestionPaperMaker.Models
{
    public class QuestionPaper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<QuestionConfiguration> Configurations { get; set; } = new();
    }

    public class QuestionConfiguration
    {
        public int CategoryId { get; set; }
        public int? DifficultyId { get; set; }
        public int? QuestionTypeId { get; set; }
        public int NumQuestions { get; set; }
    }
} 