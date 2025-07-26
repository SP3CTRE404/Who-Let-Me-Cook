using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WhoLetMeCook.Models;
using WhoLetMeCook.Services;
using WhoLetMeCook.Views;

namespace WhoLetMeCook.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();

    [ObservableProperty]
    private ObservableCollection<Meal> _suggestedMeals = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isCategoryListVisible;

    public HomeViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [RelayCommand]
    private async Task LoadHomepageDataAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var categoriesTask = _recipeService.GetAllCategoriesAsync();
            var suggestionsTask = _recipeService.GetMealsByCategory("Dessert");
            await Task.WhenAll(categoriesTask, suggestionsTask);

            Categories.Clear();
            var categoryList = await categoriesTask;
            foreach (var category in categoryList)
            {
                Categories.Add(category);
            }

            // NEW: Add our special "More" category at the end of the list.
            // We give it a unique ID so we can identify it later.
            Categories.Add(new Category { strCategory = "More", idCategory = "more" });


            SuggestedMeals.Clear();
            foreach (var meal in await suggestionsTask)
            {
                SuggestedMeals.Add(meal);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading homepage data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ToggleCategoryList()
    {
        IsCategoryListVisible = !IsCategoryListVisible;
    }

    [RelayCommand]
    private async Task GoToRecipesAsync(Category category)
    {
        if (category == null) return;
        IsCategoryListVisible = false;

        // UPDATED LOGIC: Check if the user tapped our special "More" button.
        if (category.idCategory == "more")
        {
            // If so, navigate to our new CategoriesPage.
            await Shell.Current.GoToAsync(nameof(CategoriesPage));
        }
        else
        {
            // Otherwise, navigate to the RecipePage with the selected category.
            await Shell.Current.GoToAsync($"{nameof(RecipePage)}?category={category.strCategory}");
        }
    }

    [RelayCommand]
    private async Task GoToRecipeDetailAsync(Meal meal)
    {
        if (meal == null) return;
        await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}?id={meal.idMeal}");
    }
}
