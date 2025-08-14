using WhoLetMeCook.Services;
using WhoLetMeCook.ViewModels;
using WhoLetMeCook.Views;

namespace WhoLetMeCook;

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

        // Register Services
        builder.Services.AddSingleton<RecipeService>();

        // Register Views and ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();

        builder.Services.AddTransient<RecipesViewModel>();
        builder.Services.AddTransient<RecipePage>();

        builder.Services.AddTransient<RecipeDetailViewModel>();
        builder.Services.AddTransient<RecipeDetailPage>();

        builder.Services.AddTransient<CategoriesViewModel>();
        builder.Services.AddTransient<CategoriesPage>();

        builder.Services.AddSingleton<AIService>();


        return builder.Build();
    }
}
