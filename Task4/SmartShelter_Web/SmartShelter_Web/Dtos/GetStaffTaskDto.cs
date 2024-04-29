using SmartShelter_Web.Models;

namespace SmartShelter_Web.Dtos
{
    public class GetStaffTaskDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
        public DateTime EndDate { get; set; }
        public string? StaffRole { get; set; }
        public int? AimStaffId { get; set; } //if task for certain person
        public int ByStaffId { get; set; }
        public int? OrderId { get; set; }
        public bool IsAccepted { get; set; } = false;


        public AddStaffDto? AimStaff { get; set; }
        public AddStaffDto ByStaff { get; set; }
        public OrderDto? Order { get; set; }


        public static GetStaffTaskDto MapStaffTaskToGetStaffTaskDto(StaffTask staffTask)
        {
            return new GetStaffTaskDto
            {
                Id = staffTask.Id,
                Type = staffTask.Type,
                Notes = staffTask.Notes,
                EndDate = staffTask.EndDate,
                StaffRole = staffTask.StaffRole,
                AimStaffId = staffTask.AimStaffId,
                ByStaffId = staffTask.ByStaffId,
                OrderId = staffTask.OrderId,
                IsAccepted = staffTask.IsAccepted,

                
                AimStaff = staffTask.AimStaff != null ? new AddStaffDto { Name = staffTask.AimStaff.Name } : null,
                ByStaff =  new AddStaffDto { Name = staffTask.ByStaff.Name }
            };
        }
    }
}
