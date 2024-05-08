namespace PlantCare_Web.Models.ViewModels
{
    public class SensorDataViewModel
    {
        public List<float> Temperatures { get; set; }
        public List<float> Humidities { get; set; }
        public List<float> DewPoints { get; set; }
        public List<float> AbsHumidities { get; set; }
        public List<DateTime> Dates { get; set; }

    }
}
