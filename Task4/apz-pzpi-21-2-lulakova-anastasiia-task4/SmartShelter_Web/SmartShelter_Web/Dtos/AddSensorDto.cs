namespace SmartShelter_Web.Dtos
{
    public class AddSensorDto
    {
        public string? Notes { get; set; }
        public int Frequency { get; set; } = 3600000;
        public int AviaryId { get; set; }

    }
}
