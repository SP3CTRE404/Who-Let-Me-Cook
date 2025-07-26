namespace WhoLetMeCook.Models;

// Represents a single Areas (Cuisine)
public class Areas
{
    public string strAreas { get; set; }
}

// The API wraps the list in a "meals" property, even for Areass.
public class AreasAPIResponse
{
    public List<Areas> Meals { get; set; }
}
