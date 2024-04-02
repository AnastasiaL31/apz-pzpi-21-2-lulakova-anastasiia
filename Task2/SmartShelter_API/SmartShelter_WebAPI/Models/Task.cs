namespace SmartShelter_WebAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public DateTime EndDate { get; set; }
        public int StaffId { get; set; }
        public int? OrderId { get; set; }

        public Staff Staff { get; set; }
        public Order Order { get; set; }
    }
}
