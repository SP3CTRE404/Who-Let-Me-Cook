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
    private bool _isBusy;

    public CategoriesViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var categoryList = await _recipeService.GetAllCategoriesAsync();
            Categories.Clear();
            foreach (var category in categoryList)
            {
                Categories.Add(category);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRecipesAsync(Category category)
    {
        if (category == null) return;
        await Shell.Current.GoToAsync($"{nameof(RecipePage)}?category={category.strCategory}");
    }
}
