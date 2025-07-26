namespace WhoLetMeCook.Models
{
    public class MealAPIResponse<T>
    {
        public List<T> Meals { get; set; }
    }
}
