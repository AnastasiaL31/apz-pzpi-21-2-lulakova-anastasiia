namespace SmartShelter_WebAPI.Dtos
{
    public class AddStaffTaskDto
    {
        public string Type { get; set; }
        public string? Notes { get; set; }
        public DateTime EndDate { get; set; }
        public string StaffRole { get; set; }
        public int? AimStaffId { get; set; } //if task for certain person
        public int ByStaffId { get; set; }
        public int? OrderId { get; set; }
    }
}
