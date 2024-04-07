namespace SmartShelter_WebAPI.Dtos
{
    public class AddMealPlanDto
    {
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public int AnimalId { get; set; }
    }
}
