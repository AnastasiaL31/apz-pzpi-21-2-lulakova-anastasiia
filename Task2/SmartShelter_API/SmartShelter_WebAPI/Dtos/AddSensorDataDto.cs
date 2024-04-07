namespace SmartShelter_WebAPI.Dtos
{
    public class AddSensorDataDto
    {
        public float Water { get; set; }
        public float Food { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float IHS { get; set; }
        public DateTime Date { get; set; }
        public float AvgConsumption { get; set; }
        public float Forecast { get; set; }
        public int SensorId { get; set; }
    }
}
