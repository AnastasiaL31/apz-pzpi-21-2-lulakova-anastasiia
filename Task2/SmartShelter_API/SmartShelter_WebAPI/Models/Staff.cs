namespace SmartShelter_WebAPI.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime DOB { get; set; }
        public string Position { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public DateTime? DismissialDate { get; set; }


    }
}
