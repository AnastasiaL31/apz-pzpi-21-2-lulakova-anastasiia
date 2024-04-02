namespace SmartShelter_WebAPI.Models
{
    public class Disease
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Symptoms { get; set; }
        public int? AnimalId { get; set; }


        [DeleteBehavior(DeleteBehavior.SetNull)]
        public Animal Animal { get; set; }
    }
}
