namespace SmartShelter_WebAPI.Dtos
{
    public class AddSupplyDto
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
