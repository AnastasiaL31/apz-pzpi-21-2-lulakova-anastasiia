namespace SmartShelter_WebAPI.Dtos
{
    public class StaffDetailsDto
    {
        public StaffDto Staff { get; set; }
        public string Role { get; set; } = "Guest";
    }
}
