using System.Net.Http.Json;
using QuestionPaperMaker.Constants;
using QuestionPaperMaker.Models;

namespace QuestionPaperMaker.Services
{
    public class QuestionPaperService : IQuestionPaperService
    {
        private readonly HttpClient _httpClient;
        private readonly ISettingsService _settingsService;
        private readonly string _baseUrl;

        public QuestionPaperService(HttpClient httpClient, ISettingsService settingsService)
        {
            _httpClient = httpClient;
            _settingsService = settingsService;
            _baseUrl = _settingsService.GetSettings().ApiBaseUrl + AppConstants.ApiEndpoint;
        }

        public async Task<List<QuestionPaper>> GetQuestionPapersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<QuestionPaper>>($"{_baseUrl}{AppConstants.Endpoints.QuestionPapers}");
        }

        public async Task<QuestionPaper> GetQuestionPaperAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<QuestionPaper>($"{_baseUrl}{AppConstants.Endpoints.QuestionPapers}/{id}");
        }

        public async Task<QuestionPaper> CreateQuestionPaperAsync(QuestionPaper paper)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}{AppConstants.Endpoints.QuestionPapers}", paper);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<QuestionPaper>();
        }

        public async Task<QuestionPaper> UpdateQuestionPaperAsync(int id, QuestionPaper paper)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}{AppConstants.Endpoints.QuestionPapers}/{id}", paper);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<QuestionPaper>();
        }

        public async Task DeleteQuestionPaperAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{AppConstants.Endpoints.QuestionPapers}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<byte[]> GeneratePdfAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{AppConstants.Endpoints.QuestionPapers}/{id}{AppConstants.Endpoints.GeneratePdf}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>($"{_baseUrl}{AppConstants.Endpoints.Categories}");
        }

        public async Task<List<Difficulty>> GetDifficultiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Difficulty>>($"{_baseUrl}{AppConstants.Endpoints.Difficulties}");
        }

        public async Task<List<QuestionType>> GetQuestionTypesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<QuestionType>>($"{_baseUrl}{AppConstants.Endpoints.QuestionTypes}");
        }
    }
} 