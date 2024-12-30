namespace QuestionPaperMaker.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string CodeSnippet { get; set; }
        public string Language { get; set; }
    }
} 