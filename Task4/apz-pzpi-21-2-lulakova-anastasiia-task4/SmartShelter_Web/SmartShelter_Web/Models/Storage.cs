namespace SmartShelter_Web.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public int StaffId { get; set; }

        public Staff Staff { get; set; }
    }
}
