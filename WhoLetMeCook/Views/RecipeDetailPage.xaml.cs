using WhoLetMeCook.ViewModels;

namespace WhoLetMeCook.Views;

public partial class RecipeDetailPage : ContentPage
{
    // The corresponding ViewModel is automatically injected by the MAUI dependency
    // injection container when this page is navigated to.
    public RecipeDetailPage(RecipeDetailViewModel viewModel)
    {
        InitializeComponent();

        // Set the BindingContext of the page to the injected ViewModel instance.
        // This allows the XAML to bind to the properties and commands in the ViewModel.
        BindingContext = viewModel;
    }
}
