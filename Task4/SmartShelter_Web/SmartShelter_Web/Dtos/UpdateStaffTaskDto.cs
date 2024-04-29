namespace SmartShelter_Web.Dtos
{
    public class UpdateStaffTaskDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
        public DateTime EndDate { get; set; }
        public string StaffRole { get; set; }
        public int? AimStaffId { get; set; } //if task for certain person
        public int? OrderId { get; set; }
    }
}
