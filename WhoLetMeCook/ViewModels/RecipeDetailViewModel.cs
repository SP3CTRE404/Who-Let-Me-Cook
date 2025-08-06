using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    [ObservableProperty]
    private string _recipeId;

    public RecipeDetailViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [RelayCommand]
    private async Task LoadRecipeDetailsAsync()
    {
        if (string.IsNullOrEmpty(RecipeId)) return;

        // 1. Fetch the data in the background.
        var recipeDetail = await _recipeService.GetMealDetails(RecipeId);

        if (recipeDetail != null)
        {
            // 2. Prepare the list of ingredients in the background.
            var ingredientsList = new List<string>();
            for (int i = 1; i <= 20; i++)
            {
                var ingredient = (string)typeof(MealDetail).GetProperty($"strIngredient{i}")?.GetValue(recipeDetail);
                var measure = (string)typeof(MealDetail).GetProperty($"strMeasure{i}")?.GetValue(recipeDetail);
                if (!string.IsNullOrWhiteSpace(ingredient))
                {
                    ingredientsList.Add($"{measure} {ingredient}".Trim());
                }
            }

            // 3. Now, hand everything over to the UI thread to update the screen.
            // This is the safest way to update the UI.
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Recipe = recipeDetail;
                Ingredients.Clear();
                foreach (var item in ingredientsList)
                {
                    Ingredients.Add(item);
                }
                System.Diagnostics.Debug.WriteLine($"--- FINAL CHECK: Ingredients collection now has {Ingredients.Count} items.");
            });
        }
    }
}
