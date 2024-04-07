namespace SmartShelter_WebAPI.Dtos
{
    public class AddAnimalDto
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public float Weight { get; set; }
    }
}
