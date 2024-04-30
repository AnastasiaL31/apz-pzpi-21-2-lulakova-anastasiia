namespace SmartShelter_WebAPI.Models
{
    public class Aviary
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? AnimalId { get; set; }
        public int? AviaryConditionId { get; set; }


        public Animal? Animal { get; set; }
        public AviaryCondition? AviaryCondition { get; set; }

        
    }
}
