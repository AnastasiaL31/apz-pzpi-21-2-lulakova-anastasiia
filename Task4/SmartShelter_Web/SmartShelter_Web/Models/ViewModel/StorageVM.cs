using SmartShelter_Web.Dtos;

namespace SmartShelter_Web.Models.ViewModel
{
    public class StorageVM
    {
        public List<Storage> FullList { get; set; }
        public List<Storage> GroupedList { get; set; }

        public AddOrderDto NewOrder { get; set; }

    }
}
