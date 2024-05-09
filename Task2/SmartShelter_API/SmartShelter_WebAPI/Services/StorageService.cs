using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartShelter_WebAPI.Dtos;
using System.Linq;

namespace SmartShelter_WebAPI.Services
{
    public class StorageService : IStorageService
    {
        private readonly SmartShelterDBContext _dbContext;
        private readonly IMapper _mapper;

        public StorageService(SmartShelterDBContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public bool CreateOrder(AddOrderDto orderDto, int creatorId)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.StaffId = creatorId;
            order.OrderDate = DateTime.Now;
            _dbContext.Add(order);
            return Save();
        }

        public bool UpdateOrder(UpdateOrderDto orderDto, int staffId)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderDto.Id);
            if (order == null)
            {
                return false;
            }

            _mapper.Map(orderDto, order);
            order.StaffId = staffId;
            if (_dbContext.Entry(order).State == EntityState.Detached)
            {
                _dbContext.Attach(order);
            }

            _dbContext.Entry(order).State = EntityState.Modified;

            return Save();
        }

        public bool DeleteOrder(int orderId, int staffId)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
            {
                return false;
            }

            if (order.StaffId != staffId || staffId != 1)
            {
                return false;
            }
            _dbContext.Remove(order);
            return Save();
        }

        public List<OrderDto>? GetOrderList() //for admin
        {
            var orders = _dbContext.Orders.ToList();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto;
        }

        public List<OrderDto> GetApprovedOrders(int staffId)
        {
            var orders = _dbContext.Orders.Where(x => x.StaffId == staffId && x.IsApproved == true).ToList();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto;
        }

        public bool ApproveOrder(int orderId) //for admin
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
            {
                return false;
            }

            order.IsApproved = true;
            var storage = new Storage()
            {
                Name = order.Name,
                Type = order.Type,
                Amount = order.Amount,
                UnitOfMeasure = order.UnitOfMeasure,
                Price = order.Price,
                Date = DateTime.Now,
                StaffId = (int)order.StaffId
            };
            _dbContext.Update(order);
            _dbContext.Add(storage);
            return Save();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() != 0;
        }

        public List<Storage> GetFullStorage()
        {
            var list = _dbContext.Storage.ToList();
            return list;
        }

        public List<Storage> GetGroupedStorage()
        {
            var list = _dbContext.Storage.ToList();
            List<Storage> storage = new List<Storage>();
            foreach (var item in list)
            {
                if (storage.FirstOrDefault(x => x.Name == item.Name) != null)
                {
                    int ind = storage.FindIndex(x => x.Name == item.Name);
                    if(ind != -1)
                    {
                        storage[ind].Amount += item.Amount;
                        storage[ind].Price += item.Price;
                    }
                }else { storage.Add(item); }
            }
            return storage;
        }
    }
}
