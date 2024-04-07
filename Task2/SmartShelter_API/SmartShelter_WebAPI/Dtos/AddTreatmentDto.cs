namespace SmartShelter_WebAPI.Dtos
{
    public class AddTreatmentDto
    {
        public string Type { get; set; }
        public string Notes { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int StaffId { get; set; }
        public int AnimalId { get; set; }

    }
}
