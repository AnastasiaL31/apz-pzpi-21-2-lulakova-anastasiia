namespace SmartShelter_WebAPI.Models
{
    public class Diet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public int MealId { get; set; }

        public MealPlan Meal { get; set; }
    }
}
