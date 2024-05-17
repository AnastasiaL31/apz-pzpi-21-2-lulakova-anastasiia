namespace SmartShelter_Web.Models
{
    public class Disease
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Symptoms { get; set; }
        public int? AnimalId { get; set; }

        public Animal? Animal { get; set; }
    }
}
