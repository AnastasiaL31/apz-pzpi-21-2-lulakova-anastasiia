using SmartShelter_Web.Dtos;

namespace SmartShelter_Web.Models.ViewModel
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public AddStaffDto NewStaff { get; set; }
        
    }
}
