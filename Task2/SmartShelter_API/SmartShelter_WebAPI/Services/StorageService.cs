namespace SmartShelter_WebAPI.Services
{
    public class StorageService : IStorageService
    {
        public bool CreateOrder(Order order, int creatorId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOrder(Order order, int userId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(Order order, int userId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrderList(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetApprovedOrders(int userId)
        {
            throw new NotImplementedException();
        }

        public bool ApproveOrder(Order order, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
