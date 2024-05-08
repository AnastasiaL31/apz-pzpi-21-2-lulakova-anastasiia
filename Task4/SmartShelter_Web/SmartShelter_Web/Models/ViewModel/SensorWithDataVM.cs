namespace SmartShelter_Web.Models.ViewModel
{
    public class SensorWithDataVM
    {
        public Sensor Sensor { get; set; }
        public List<SensorData> SensorData { get; set; }

        public int AnimalId { get; set; }
    }
}
