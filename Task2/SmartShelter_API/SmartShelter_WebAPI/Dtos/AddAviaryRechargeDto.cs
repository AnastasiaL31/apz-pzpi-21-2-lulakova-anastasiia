using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShelter_WebAPI.Dtos
{
    public class AddAviaryRechargeDto
    {
        public string Type { get; set; }
        public string? Name { get; set; }
        public float Amount { get; set; }

    }
}
