namespace SmartShelter_Web.Models
{
    public class AviaryDescription
    {
        public Aviary Aviary { get; set; }
        public float? FoodPerDay { get; set; }
        public DateTime? LastRecharge { get; set; }
        public float? WaterNow { get; set; }
    }
}
