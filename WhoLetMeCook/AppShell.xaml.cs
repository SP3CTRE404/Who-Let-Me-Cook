using WhoLetMeCook.Views;

namespace WhoLetMeCook;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
        Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
        Routing.RegisterRoute(nameof(CategoriesPage), typeof(CategoriesPage));
    }
}
