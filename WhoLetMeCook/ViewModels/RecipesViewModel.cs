using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WhoLetMeCook.Models;
using WhoLetMeCook.Services;

namespace WhoLetMeCook.ViewModels;

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

    private string _category;
    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            // NEW: Check if a category was actually passed before loading.
            if (!string.IsNullOrEmpty(value))
            {
                Task.Run(async () => await LoadMealsByCategoryAsync());
            }
            else
            {
                // If no category is passed, clear the list.
                // The page will appear empty, ready for a search.
                Meals.Clear();
            }
        }
    }


    public RecipesViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
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
