namespace SmartShelter_WebAPI.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int DiseaseId { get; set; }
        public int StaffId { get; set; }
        public int AnimalId { get; set; }

        
        public Disease Disease { get; set; }
        public Staff Staff { get; set; }
        public Animal Animal { get; set; }
        
    }
}
