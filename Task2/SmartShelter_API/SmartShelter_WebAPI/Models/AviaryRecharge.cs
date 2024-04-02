using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShelter_WebAPI.Models
{
    public class AviaryRecharge
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Name { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey(nameof(Staff))]
        public  int StaffId { get; set; }
        [ForeignKey(nameof(Aviary))]
        public int AviaryId { get;}


        public Staff Staff { get; set; }
        public Aviary Aviary { get; set; }
    }
}
