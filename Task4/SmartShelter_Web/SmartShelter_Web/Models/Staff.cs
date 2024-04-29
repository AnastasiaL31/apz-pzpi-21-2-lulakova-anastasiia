using Microsoft.AspNetCore.Identity;

namespace SmartShelter_Web.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime DOB { get; set; }
        public string Position { get; set; }
        public bool HasRole { get; set; } = false;
        public DateTime AcceptanceDate { get; set; }
        public DateTime? DismissialDate { get; set; }
        public string IdentityUserId { get; set; }

        public IdentityUser IdentityUser { get; set; }
    }
}
