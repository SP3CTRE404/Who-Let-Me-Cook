namespace WhoLetMeCook.Models;

// This class represents a single category from the API
public class Category
{
    public string idCategory { get; set; }
    public string strCategory { get; set; }
    public string strCategoryThumb { get; set; }
    public string strCategoryDescription { get; set; }
}

// The API sends {"categories": [...]}, so our property is named "Categories".
public class CategoryAPIResponse
{
    public List<Category> Categories { get; set; }
}
