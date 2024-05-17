namespace SmartShelter_Web.Models
{
    public class SensorData
    {
        public int Id { get; set; }
        public float Water { get; set; }
        public float Food { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float IHS { get; set; }
        public DateTime Date { get; set; }
        public float Forecast { get; set; }
        public int SensorId { get; set; }

        public Sensor? Sensor { get; set; }
    }
}
