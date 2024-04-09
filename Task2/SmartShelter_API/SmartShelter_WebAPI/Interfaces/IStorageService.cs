using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Interfaces
{
    public interface IStorageService
    {
        public bool CreateOrder(AddOrderDto orderDto, int creatorId);
        public bool UpdateOrder(UpdateOrderDto orderDto, int staffId);
        public bool DeleteOrder(int orderId, int staffId);
        public List<OrderDto>? GetOrderList(); //for admin
        public List<OrderDto>? GetApprovedOrders(int staffId);
        public bool ApproveOrder(int orderId); //for admin
    }
}