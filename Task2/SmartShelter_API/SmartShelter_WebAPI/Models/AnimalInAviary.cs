namespace SmartShelter_WebAPI.Models
{
    public class AnimalInAviary
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public int  AviaryId { get; set; }


        public Animal Animal { get; set; }
        public Aviary Aviary { get; set; }
    }
}
