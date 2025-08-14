using System.Net.Http.Json;

namespace WhoLetMeCook.Services;
public class AIService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-preview-05-20:generateContent?key={APIKeys.GeminiApiKey}";

    public AIService()
    {
        _httpClient = new HttpClient();
    }

    // Method signature updated to accept ingredients and serving size.
    public async Task<string> SimplifyRecipeAsync(string originalInstructions, string originalIngredients, int servingSize)
    {
        // This prompt now asks the AI to adjust ingredients for the serving size
        // and to return both ingredients and instructions in a structured format.
        var prompt = $"You are a helpful cooking assistant. Please adjust the following recipe to serve {servingSize} people. " +
                     $"Then, rewrite the instructions in a simple, step-by-step, beginner-friendly format. " +
                     $"The output must have two sections clearly marked with 'Ingredients:' and 'Instructions:'. " +
                     $"Under 'Ingredients:', provide a bulleted list of the adjusted ingredients. " +
                     $"Under 'Instructions:', provide a numbered list of the simplified steps. " +
                     $"Do not use any other Markdown formatting. Provide only the text for the recipe.\n\n" +
                     $"Original Ingredients:\n{originalIngredients}\n\n" +
                     $"Original Instructions:\n{originalInstructions}";

        if (string.IsNullOrWhiteSpace(originalInstructions) || string.IsNullOrWhiteSpace(originalIngredients))
        {
            return "Recipe information is incomplete.";
        }

        try
        {
            var requestPayload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, requestPayload);
            if (response.IsSuccessStatusCode)
            {
                var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                return geminiResponse?.candidates[0]?.content?.parts[0]?.text;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"--- AI API Error: {errorContent}");
                return "Sorry, the AI assistant is currently unavailable.";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"--- AI Service Exception: {ex.Message}");
            return "An error occurred while trying to simplify the recipe.";
        }
    }
}
