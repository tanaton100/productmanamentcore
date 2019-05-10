using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Services;

namespace ProductmanagementCore.Controllers
{
    [Route("api/Order")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        [Route("getall")]
        public IEnumerable<Orders> GetAll()
        {
            var result = _ordersService.GetAllOrder();
            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public Orders GetById([FromRoute]int id)
        {
            var result = _ordersService.GetByIdOrders(id);
            return result;
        }

        [HttpPost]
        [Route("")]
        public Orders AddOrders([FromBody] OrderInput orders)
        {
            var modelInput = new Orders
            {
                IdProduct = orders.IdProduct,
                IdUser = orders.IdUser
            };
            var result = _ordersService.AddOrders(modelInput);
            return result;
        }


        [HttpPut]
        [Route("")]
        public bool UpdateOrders([FromBody] Orders orders)
        {
          
            var result = _ordersService.Update(orders);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public bool DeleteOrders([FromRoute] int id)
        {

            var result = _ordersService.Delete(id);
            return result;
        }
    }
}