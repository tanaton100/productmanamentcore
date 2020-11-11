using ProductmanagementCore.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<Orders>> GetAllOrder();
        ValueTask<Orders> GetByIdOrders(int id);
        Task<Orders> AddOrders(Orders orders);
        Task<bool> Update(Orders orders);
        Task<bool> Delete(int id);
        Task<IEnumerable<Orders>> FindbyUserId(int id);
    }

    public class OrdersService : IOrdersService
    {
        private IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;

        }

        public async Task<IEnumerable<Orders>> GetAllOrder()
        {
            return await _ordersRepository.GetAll();
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
