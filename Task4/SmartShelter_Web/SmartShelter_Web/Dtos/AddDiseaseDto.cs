namespace SmartShelter_Web.Dtos
{
    public class AddDiseaseDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Symptoms { get; set; }
        public int AnimalId { get; set; }

    }
}
