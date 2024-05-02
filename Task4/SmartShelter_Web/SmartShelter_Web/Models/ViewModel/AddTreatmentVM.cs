using SmartShelter_Web.Dtos;

namespace SmartShelter_Web.Models.ViewModel
{
    public class AddTreatmentVM
    {
        public Treatment NewTreatment { get; set; }
        public Disease? Disease { get; set; }
        public List<AddSupplyDto> SuppliesToAdd { get; set; }
        public AddSupplyDto NewSupply { get; set; }
        public int indexToDelete { get; set; }
    }
}
