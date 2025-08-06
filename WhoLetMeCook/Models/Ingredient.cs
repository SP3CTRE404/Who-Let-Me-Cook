namespace WhoLetMeCook.Models;

public class Ingredient
{
    public string idIngredient { get; set; }
    public string strIngredient { get; set; }
}

public class IngredientAPIResponse
{
    public List<Ingredient> Meals { get; set; }
}