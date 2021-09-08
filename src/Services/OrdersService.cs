using ProductmanagementCore.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using ProductmanagementCore.Common;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.Dto;

namespace ProductmanagementCore.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<Orders>> GetAllOrder();
        Task<IEnumerable<OrderDto>> GetOrder();
        ValueTask<Orders> GetByIdOrders(int id);
        Task<Orders> AddOrders(Orders orders);
        Task<bool> Update(Orders orders);
        Task<bool> Delete(int id);
        Task<IEnumerable<Orders>> FindbyUserId(int id);
    }

    public class OrdersService : IOrdersService
    {
        private IOrdersRepository _ordersRepository;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;

        public OrdersService(IOrdersRepository ordersRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _ordersRepository = ordersRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Orders>> GetAllOrder()
        {
            return await _ordersRepository.GetAll();
        }

        public async Task<IEnumerable<OrderDto>> GetOrder()
        {
            var users = await _userRepository.GetAll();
            var orders = await _ordersRepository.GetAll();
            var products = await _productRepository.GetAll();
            var result = from order in orders
                join user in users on order.UserId equals user.Id
                join product in products on order.ProductId equals product.Id
                select new  { OrderId = order.Id,UserName =user.Username,ProductName = product.Name};


            return result.Adapt<IEnumerable<OrderDto>>();
        }
        public async ValueTask<Orders> GetByIdOrders(int id)
        {
            return await _ordersRepository.FindById(id);
        }

        public async Task<Orders> AddOrders(Orders orders)
        {
            var id = await _ordersRepository.AddAsync(orders);
            orders.Id = id;
            return id > 0 ? orders : new Orders();
        }

        public async Task<bool> Update(Orders orders)
        {
            return (await _ordersRepository.UpdateAsync(orders)) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            return (await _ordersRepository.DeleteAsync(id)) > 0;
        }
        public async Task <IEnumerable<Orders>> FindbyUserId(int id)
        {
            return await _ordersRepository.FindByUserId(id);
        }
    }
}
