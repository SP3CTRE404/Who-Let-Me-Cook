using System.Text.Json;
using WhoLetMeCook.Models;

namespace WhoLetMeCook.Services;

public class RecipeService
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "https://www.themealdb.com/api/json/v1/1/";
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RecipeService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("categories.php");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<CategoryAPIResponse>(jsonString, _jsonSerializerOptions);
                return apiResponse?.Categories ?? new List<Category>();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching categories: {ex.Message}");
        }
        return new List<Category>();
    }

    public async Task<List<Meal>> GetMealsByCategory(string category)
    {
        if (string.IsNullOrEmpty(category)) return new List<Meal>();
        try
        {
            var response = await _httpClient.GetAsync($"filter.php?c={category}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<MealAPIResponse<Meal>>(jsonString, _jsonSerializerOptions);
                return apiResponse?.Meals ?? new List<Meal>();
            }
        }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        return new List<Meal>();
    }

    public async Task<MealDetail> GetMealDetails(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        try
        {
            var response = await _httpClient.GetAsync($"lookup.php?i={id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<MealAPIResponse<MealDetail>>(jsonString, _jsonSerializerOptions);
                return apiResponse?.Meals?.FirstOrDefault();
            }
        }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        return null;
    }

    public async Task<List<Meal>> SearchMealsByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return new List<Meal>();
        try
        {
            var response = await _httpClient.GetAsync($"search.php?s={name}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<MealAPIResponse<Meal>>(jsonString, _jsonSerializerOptions);
                return apiResponse?.Meals ?? new List<Meal>();
            }
        }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        return new List<Meal>();
    }

    public async Task<List<Meal>> GetMealsByAreasAsync(string Areas)
    {
        if (string.IsNullOrEmpty(Areas)) return new List<Meal>();
        try
        {
            var response = await _httpClient.GetAsync($"filter.php?a={Areas}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<MealAPIResponse<Meal>>(jsonString, _jsonSerializerOptions);
                return apiResponse?.Meals ?? new List<Meal>();
            }
        }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        return new List<Meal>();
    }

}
