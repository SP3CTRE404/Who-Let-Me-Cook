using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WhoLetMeCook.Models;
using WhoLetMeCook.Services;

namespace WhoLetMeCook.ViewModels;

[QueryProperty(nameof(Areas), "Areas")]
[QueryProperty(nameof(Category), "category")]
public partial class RecipesViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private ObservableCollection<Meal> _meals = new();
    [ObservableProperty]
    private bool _isBusy;
    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private string _category;
    [ObservableProperty]
    private string _Areas;

    public RecipesViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    partial void OnCategoryChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Task.Run(async () => await LoadMealsByCategoryAsync());
        }
    }

    partial void OnAreasChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Task.Run(async () => await LoadMealsByAreasAsync());
        }
    }

    private async Task LoadMealsByCategoryAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var mealList = await _recipeService.GetMealsByCategory(Category);
            Meals.Clear();
            foreach (var meal in mealList) { Meals.Add(meal); }
        }
        finally { IsBusy = false; }
    }

    private async Task LoadMealsByAreasAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var mealList = await _recipeService.GetMealsByAreasAsync(Areas);
            Meals.Clear();
            foreach (var meal in mealList) { Meals.Add(meal); }
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SearchMealsAsync()
    {
        if (IsBusy || string.IsNullOrEmpty(SearchText)) return;
        try
        {
            IsBusy = true;
            var mealList = await _recipeService.SearchMealsByName(SearchText);
            Meals.Clear();
            foreach (var meal in mealList) { Meals.Add(meal); }
        }
        finally { IsBusy = false; }
    }
}
