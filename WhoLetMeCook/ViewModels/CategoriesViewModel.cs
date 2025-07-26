using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WhoLetMeCook.Models;
using WhoLetMeCook.Services;
using WhoLetMeCook.Views;

namespace WhoLetMeCook.ViewModels;

public partial class CategoriesViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();
    [ObservableProperty]
    private ObservableCollection<Areas> _Areass = new();

    [ObservableProperty]
    private bool _isBusy;

    // These properties will control which list is visible in the UI
    [ObservableProperty]
    private bool _isCategoryFilterVisible = true; // Start by showing categories
    [ObservableProperty]
    private bool _isCuisineFilterVisible;

    public CategoriesViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var categoriesTask = _recipeService.GetAllCategoriesAsync();
            var AreassTask = _recipeService.GetAllAreassAsync();

            await Task.WhenAll(categoriesTask, AreassTask);

            Categories.Clear();
            foreach (var item in await categoriesTask) { Categories.Add(item); }

            Areass.Clear();
            foreach (var item in await AreassTask) { Areass.Add(item); }
        }
        finally { IsBusy = false; }
    }

    // This command runs when a filter button is clicked
    [RelayCommand]
    private void SetCurrentFilter(string filter)
    {
        IsCategoryFilterVisible = filter == "Category";
        IsCuisineFilterVisible = filter == "Cuisine";
    }

    [RelayCommand]
    private async Task GoToRecipesAsync(object selectedItem)
    {
        if (selectedItem == null) return;

        if (selectedItem is Category category)
        {
            await Shell.Current.GoToAsync($"{nameof(RecipePage)}?category={category.strCategory}");
        }
        else if (selectedItem is Areas Areas)
        {
            await Shell.Current.GoToAsync($"{nameof(RecipePage)}?Areas={Areas.strAreas}");
        }
    }
}
