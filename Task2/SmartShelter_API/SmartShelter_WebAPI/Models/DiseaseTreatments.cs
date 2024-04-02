namespace SmartShelter_WebAPI.Models
{
    public class DiseaseTreatments
    {
        public int Id { get; set; }
        public int DiseaseId { get; set; }
        public int TreatmentId { get; set; }

    
        public Disease Disease { get; set; }
        public Treatment Treatment { get; set; }
    }
}
