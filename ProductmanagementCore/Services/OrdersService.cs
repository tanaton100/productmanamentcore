﻿using ProductmanagementCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Services
{
    public interface IOrdersService
    {
        IEnumerable<Orders> GetAllOrder();
        Orders GetByIdOrders(int id);
        Orders AddOrders(Orders orders);
        bool Update(Orders orders);
        bool Delete(int id);
    }

    public class OrdersService: IOrdersService
    {
        private IOrdersRepository _ordersRepository;

        public OrdersService( IConfiguration configuration)
        {
            _ordersRepository = new OrdersRepository(configuration);

        }

        public IEnumerable<Orders> GetAllOrder()
        {
            return _ordersRepository.GetAll().ToList();
        }

        public Orders GetByIdOrders(int id)
        {
            return _ordersRepository.FindById(id);
        }

        public Orders AddOrders(Orders orders)
        {   
            var id = _ordersRepository.Add(orders);
            orders.Id = id;
            return id > 0 ? orders : new Orders();
        }

        public bool Update(Orders orders)
        {
            return _ordersRepository.Update(orders)>0;
        }

        public bool Delete(int id)
        {
            return _ordersRepository.Delete(id) > 0;
        }
    }
}
