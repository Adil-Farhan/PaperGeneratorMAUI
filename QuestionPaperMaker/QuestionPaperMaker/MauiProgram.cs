using Microsoft.Extensions.Logging;
using QuestionPaperMaker.Services;
using QuestionPaperMaker.ViewModels;
using QuestionPaperMaker.Views;

namespace QuestionPaperMaker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<IPreferences>(Preferences.Default);
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            
            builder.Services.AddHttpClient<IQuestionPaperService, QuestionPaperService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<ISettingsService>().GetSettings();
                client.BaseAddress = new Uri(settings.ApiBaseUrl);
            });

            // ViewModels
            builder.Services.AddTransient<QuestionPapersViewModel>();
            builder.Services.AddTransient<CreateQuestionPaperViewModel>();
            builder.Services.AddTransient<EditQuestionPaperViewModel>();
            builder.Services.AddTransient<PreviewQuestionPaperViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            // Views
            builder.Services.AddTransient<QuestionPapersPage>();
            builder.Services.AddTransient<CreateQuestionPaperPage>();
            builder.Services.AddTransient<EditQuestionPaperPage>();
            builder.Services.AddTransient<PreviewQuestionPaperPage>();
            builder.Services.AddTransient<SettingsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
} 