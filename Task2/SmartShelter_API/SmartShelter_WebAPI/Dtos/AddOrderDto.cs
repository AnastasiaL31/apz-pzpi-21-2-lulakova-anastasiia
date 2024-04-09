namespace SmartShelter_WebAPI.Dtos
{
    public class AddOrderDto
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
        public float Price { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
