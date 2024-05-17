using SmartShelter_Web.Dtos;

namespace SmartShelter_Web.Models.ViewModel
{
    public class StaffDetailsVM
    {
        public StaffDto Staff { get; set; }
        public string Role { get; set; } = "Guest";
    }
}
