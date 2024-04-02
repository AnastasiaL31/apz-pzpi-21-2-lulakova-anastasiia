namespace SmartShelter_WebAPI.Models
{
    public class MealPlan
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int AnimalId { get; set; }

        public Animal Animal { get; set; }
    }
}
