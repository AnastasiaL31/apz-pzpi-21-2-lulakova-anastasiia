namespace SmartShelter_Web.Models.ViewModel
{
    public class DiseaseTreatmentsVM
    {
        public List<TreatmentWithStaff> Treatments { get; set; }
        public Disease Disease { get; set; }
        public bool isClosed { get; set; }
    }
}
