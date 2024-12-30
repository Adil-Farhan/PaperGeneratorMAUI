namespace QuestionPaperMaker.Constants
{
    public static class AppConstants
    {
        public const string DefaultApiBaseUrl = "http://localhost:8000";
        public const string ApiEndpoint = "/api";
        
        public static class Endpoints
        {
            public const string Questions = "/questions";
            public const string Categories = "/categories";
            public const string Difficulties = "/difficulties";
            public const string QuestionTypes = "/question-types";
            public const string QuestionPapers = "/question-papers";
            public const string GeneratePdf = "/generate-pdf";
        }
    }
} 