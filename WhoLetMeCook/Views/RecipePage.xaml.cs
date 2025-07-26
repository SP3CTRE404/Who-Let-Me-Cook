using WhoLetMeCook.ViewModels;

namespace WhoLetMeCook.Views;

public partial class RecipePage : ContentPage
{
    private readonly RecipesViewModel _viewModel;

    public RecipePage(RecipesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnRecipeTapped(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Models.Meal selectedMeal)
            return;
        await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}?id={selectedMeal.idMeal}");
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        if (_viewModel.SearchMealsCommand.CanExecute(null))
        {
            await _viewModel.SearchMealsCommand.ExecuteAsync(null);
        }
    }
}
