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
    private ObservableCollection<Areas> _Areas = new();

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
        Areas.Add(new Areas { strAreas = "American", FlagUrl = "https://flagcdn.com/w40/us.png" });
        Areas.Add(new Areas { strAreas = "British", FlagUrl = "https://flagcdn.com/w40/gb.png" });
        Areas.Add(new Areas { strAreas = "Canadian", FlagUrl = "https://flagcdn.com/w40/ca.png" });
        Areas.Add(new Areas { strAreas = "Chinese", FlagUrl = "https://flagcdn.com/w40/cn.png" });
        Areas.Add(new Areas { strAreas = "French", FlagUrl = "https://flagcdn.com/w40/fr.png" });
        Areas.Add(new Areas { strAreas = "Indian", FlagUrl = "https://flagcdn.com/w40/in.png" });
        Areas.Add(new Areas { strAreas = "Italian", FlagUrl = "https://flagcdn.com/w40/it.png" });
        Areas.Add(new Areas { strAreas = "Japanese", FlagUrl = "https://flagcdn.com/w40/jp.png" });
        Areas.Add(new Areas { strAreas = "Mexican", FlagUrl = "https://flagcdn.com/w40/mx.png" });
        Areas.Add(new Areas { strAreas = "Spanish", FlagUrl = "https://flagcdn.com/w40/es.png" });
        Areas.Add(new Areas { strAreas = "Thai", FlagUrl = "https://flagcdn.com/w40/th.png" });
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
