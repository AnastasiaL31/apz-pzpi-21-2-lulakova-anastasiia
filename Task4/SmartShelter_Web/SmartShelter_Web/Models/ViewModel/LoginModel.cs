using SmartShelter_Web.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SmartShelter_Web.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required") ]
        [MinLength(10, ErrorMessage = "Length must be more then 10")]
        public string Username { get; set; } = "";

        [MinLength(8, ErrorMessage = "Length must be more then 8")]
        public string Password { get; set; } = "";

        public AddStaffDto NewStaff { get; set; }
        
    }
}
