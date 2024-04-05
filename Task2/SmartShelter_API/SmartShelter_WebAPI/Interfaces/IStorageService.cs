namespace SmartShelter_WebAPI.Interfaces
{
    public interface IStorageService
    {
        public bool CreateOrder(Order order, int creatorId);
        public bool UpdateOrder(Order order, int userId);
        public bool DeleteOrder(Order order, int userId);
        public List<Order> GetOrderList(int userId);
        public List<Order> GetApprovedOrders(int userId);
        public bool ApproveOrder(Order order, int userId);
    }
}