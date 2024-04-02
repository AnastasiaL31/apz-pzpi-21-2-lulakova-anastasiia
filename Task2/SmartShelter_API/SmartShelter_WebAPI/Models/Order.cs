namespace SmartShelter_WebAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
        public float Price { get; set; }
        public bool IsApproved { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StaffId { get; set; }

        public Staff Staff { get; set; }
    }
}
