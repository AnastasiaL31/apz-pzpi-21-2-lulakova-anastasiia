namespace SmartShelter_Web.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
        public int TreatmentId { get; set; }

        public Treatment Treatment { get; set; }
    }
}
