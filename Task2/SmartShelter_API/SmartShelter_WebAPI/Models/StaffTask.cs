using AutoMapper;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Models
{
    public class StaffTask
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
        public DateTime EndDate { get; set; }
        public string StaffRole { get; set; }
        public int? AimStaffId { get; set; } //if task for certain person
        public int ByStaffId { get; set; }
        public int? OrderId { get; set; }
        public bool IsAccepted { get; set; } = false;


        public Staff? AimStaff { get; set; }
        public Staff ByStaff { get; set; }
        public Order? Order { get; set; }


       
    }
}
