using WhoLetMeCook.ViewModels;

namespace WhoLetMeCook.Views;

public partial class RecipeDetailPage : ContentPage
{
    private readonly RecipeDetailViewModel _viewModel;

    public RecipeDetailPage(RecipeDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // This method runs on the UI thread when the page is about to appear.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // We safely call the command to load the recipe details.
        if (_viewModel.LoadRecipeDetailsCommand.CanExecute(null))
        {
            await _viewModel.LoadRecipeDetailsCommand.ExecuteAsync(null);
        }
    }
}
