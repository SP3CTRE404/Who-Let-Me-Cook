using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using WhoLetMeCook.Models;
using WhoLetMeCook.Services;

namespace WhoLetMeCook.ViewModels;

[QueryProperty(nameof(RecipeId), "id")]
public partial class RecipeDetailViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;
    private readonly AIService _aiService;

    [ObservableProperty]
    private MealDetail _recipe;

    [ObservableProperty]
    private string _recipeId;

    [ObservableProperty]
    private ObservableCollection<string> _simplifiedIngredients = new();

    [ObservableProperty]
    private ObservableCollection<string> _simplifiedInstructions = new();

    [ObservableProperty]
    private int _servingSize = 1;

    [ObservableProperty]
    private bool _isBusy;

    private bool _isInitialLoad = true;

    public RecipeDetailViewModel(RecipeService recipeService, AIService aiService)
    {
        _recipeService = recipeService;
        _aiService = aiService;
    }

    async partial void OnRecipeIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _isInitialLoad = true;
            ServingSize = 1;
            await LoadRecipeDetailsAsync();
        }
    }

    async partial void OnServingSizeChanged(int value)
    {
        if (!_isInitialLoad)
        {
            await LoadSimplifiedRecipeAsync();
        }
    }

    private async Task LoadRecipeDetailsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var recipeDetail = await _recipeService.GetMealDetails(RecipeId);
            if (recipeDetail != null)
            {
                Recipe = recipeDetail;
                await LoadSimplifiedRecipeAsync();
            }
        }
        finally
        {
            IsBusy = false;
            _isInitialLoad = false;
        }
    }

    private async Task LoadSimplifiedRecipeAsync()
    {
        if (Recipe == null) return;

        IsBusy = true;
        try
        {
            var ingredientsBuilder = new StringBuilder();
            for (int i = 1; i <= 20; i++)
            {
                var ingredient = (string)typeof(MealDetail).GetProperty($"strIngredient{i}")?.GetValue(Recipe);
                var measure = (string)typeof(MealDetail).GetProperty($"strMeasure{i}")?.GetValue(Recipe);
                if (!string.IsNullOrWhiteSpace(ingredient))
                {
                    ingredientsBuilder.AppendLine($"{measure} {ingredient}".Trim());
                }
            }

            var simplifiedText = await _aiService.SimplifyRecipeAsync(Recipe.strInstructions, ingredientsBuilder.ToString(), ServingSize);
            await ParseCombinedRecipeText(simplifiedText);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading simplified recipe: {ex.Message}");
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                SimplifiedIngredients.Clear();
                SimplifiedInstructions.Clear();
                SimplifiedInstructions.Add("Failed to adjust recipe.");
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ParseCombinedRecipeText(string combinedText)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            SimplifiedIngredients.Clear();
            SimplifiedInstructions.Clear();

            if (string.IsNullOrWhiteSpace(combinedText))
            {
                SimplifiedInstructions.Add("Could not retrieve recipe details.");
                return;
            }

            int ingredientsIndex = combinedText.IndexOf("Ingredients:", StringComparison.OrdinalIgnoreCase);
            int instructionsIndex = combinedText.IndexOf("Instructions:", StringComparison.OrdinalIgnoreCase);

            if (ingredientsIndex != -1 && instructionsIndex != -1)
            {
                string ingredientsSection = combinedText.Substring(ingredientsIndex + "Ingredients:".Length, instructionsIndex - (ingredientsIndex + "Ingredients:".Length)).Trim();
                string instructionsSection = combinedText.Substring(instructionsIndex + "Instructions:".Length).Trim();

                var ingredients = ingredientsSection.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in ingredients)
                {
                    var cleanItem = item.Trim().TrimStart('-', '*').Trim();
                    if (!string.IsNullOrWhiteSpace(cleanItem))
                    {
                        SimplifiedIngredients.Add(cleanItem);
                    }
                }

                var instructions = instructionsSection.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in instructions)
                {
                    var cleanItem = Regex.Replace(item.Trim(), @"^\d+\.\s*", "");
                    if (!string.IsNullOrWhiteSpace(cleanItem))
                    {
                        SimplifiedInstructions.Add(cleanItem);
                    }
                }
            }
            else
            {
                SimplifiedInstructions.Add(combinedText);
            }
        });
    }
}
