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
    private static readonly ObservableCollection<Category> _categories = new();

    [ObservableProperty]
    private static readonly ObservableCollection<Areas> _Areas = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isCategoryFilterVisible = true;
    [ObservableProperty]
    private bool _isCuisineFilterVisible;

    public CategoriesViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
        CreatePredefinedCuisineList();
    }


    private void CreatePredefinedCuisineList()
    {
        Areas.Add(new Areas { strAreas = "American" });
        Areas.Add(new Areas { strAreas = "British" });
        Areas.Add(new Areas { strAreas = "Canadian" });
        Areas.Add(new Areas { strAreas = "Chinese" });
        Areas.Add(new Areas { strAreas = "French" });
        Areas.Add(new Areas { strAreas = "Indian" });
        Areas.Add(new Areas { strAreas = "Italian" });
        Areas.Add(new Areas { strAreas = "Japanese" });
        Areas.Add(new Areas { strAreas = "Mexican" });
        Areas.Add(new Areas { strAreas = "Spanish" });
        Areas.Add(new Areas { strAreas = "Thai" });
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy || Categories.Count > 0) return;
        try
        {
            IsBusy = true;
            var categoryList = await _recipeService.GetAllCategoriesAsync();

            Categories.Clear();
            foreach (var item in categoryList) { Categories.Add(item); }
        }
        finally { IsBusy = false; }
    }

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
