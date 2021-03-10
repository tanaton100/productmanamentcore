using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ordersService.GetAllOrder());

        }
        [HttpGet]
        [Route("getOrder")]
        public async Task<IActionResult> GetOrder()
        {
            return Ok(await _ordersService.GetOrder());

        }

        [HttpGet]
        [Route("{id}")]
        public async ValueTask<IActionResult> GetById([FromRoute]int id)
        {
            return Ok(await _ordersService.GetByIdOrders(id));

        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddOrders([FromBody] OrderInput orders)
        {
            var modelInput = new Orders
            {
                ProductId = orders.IdProduct,
                UserId = orders.IdUser
            };
            var result = await _ordersService.AddOrders(modelInput);
            return Created("", result);
        }


        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateOrders([FromBody] Orders orders)
        {
          
            var result = await _ordersService.Update(orders);
            return Accepted(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteOrders([FromRoute] int id)
        {

            var result = await _ordersService.Delete(id);
            return Accepted(result);
        }
    }
}