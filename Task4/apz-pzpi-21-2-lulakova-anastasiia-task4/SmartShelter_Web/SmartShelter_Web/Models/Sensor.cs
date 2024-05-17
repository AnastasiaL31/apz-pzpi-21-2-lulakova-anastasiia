namespace SmartShelter_Web.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string? Notes { get; set; }
        public int Frequency { get; set; }
        public int AviaryId { get; set; }

        public Aviary? Aviary { get; set; }
    }
}
