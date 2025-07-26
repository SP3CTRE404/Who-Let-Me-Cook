using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WhoLetMeCook.Models;
using WhoLetMeCook.Services;

namespace WhoLetMeCook.ViewModels;

[QueryProperty(nameof(RecipeId), "id")]
public partial class RecipeDetailViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private MealDetail _recipe;

    public ObservableCollection<string> Ingredients { get; } = new();

    private string _recipeId;
    public string RecipeId
    {
        get => _recipeId;
        set
        {
            _recipeId = value;
            Task.Run(async () => await LoadRecipeDetailsAsync());
        }
    }

    public RecipeDetailViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    private async Task LoadRecipeDetailsAsync()
    {
        if (string.IsNullOrEmpty(RecipeId)) return;
        var recipeDetail = await _recipeService.GetMealDetails(RecipeId);
        if (recipeDetail != null)
        {
            Recipe = recipeDetail;
            PopulateIngredients();
        }
    }

    private void PopulateIngredients()
    {
        Ingredients.Clear();
        if (Recipe == null) return;
        for (int i = 1; i <= 20; i++)
        {
            var ingredient = (string)typeof(MealDetail).GetProperty($"strIngredient{i}")?.GetValue(Recipe);
            var measure = (string)typeof(MealDetail).GetProperty($"strMeasure{i}")?.GetValue(Recipe);
            if (!string.IsNullOrWhiteSpace(ingredient))
            {
                Ingredients.Add($"{measure} {ingredient}".Trim());
            }
        }
    }
}
