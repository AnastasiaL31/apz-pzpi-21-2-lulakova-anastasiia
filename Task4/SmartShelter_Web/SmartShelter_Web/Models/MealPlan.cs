namespace SmartShelter_Web.Models
{
    public class MealPlan
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public int AnimalId { get; set; }

        public Animal? Animal { get; set; }
    }
}
