using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Services;

namespace ProductmanagementCore.Controllers
{
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IConfiguration configuration)
        {
            _productService = new ProductService(configuration);
        }
        [HttpGet]
        [Route("")]
        public IActionResult getUser()
        {
            var result = _productService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetId([FromRoute]int id)
        {
            return Ok(_productService.FindById(id));
        }


        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody]ProductInput product)
        {
            var inputUpdate = new Products
            {
                Price = product.Price,
                Name = product.Name
            };
            return Ok(_productService.Add(inputUpdate));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute]int id,[FromBody]ProductInput product)
        {
            var inputUpdate = new Products
            {
                Id = id,
                Price = product.Price,
                Name = product.Name
            };
            return Ok(_productService.Update(inputUpdate));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
           var result = _productService.Delete(id);
            return Ok(result);
        }
    }
}