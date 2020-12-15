using Microsoft.AspNetCore.Mvc;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Services;
using System.Threading.Tasks;

namespace ProductmanagementCore.Controllers
{
    [Route("api/Product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProduct()
        {
            var result = await _productService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task <IActionResult> GetId([FromRoute]int id)
        {
            return Ok(await _productService.FindById(id));
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody]ProductInput product)
        {
            var inputUpdate = new Products
            {
                Price = product.Price,
                Name = product.Name
            };
            return Ok(await _productService.Add(inputUpdate));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id,[FromBody]ProductInput product)
        {
            var inputUpdate = new Products
            {
                Id = id,
                Price = product.Price,
                Name = product.Name
            };
            return Ok(await _productService.Update(inputUpdate));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           var result = await _productService.Delete(id);
            return Ok(result);
        }
    }
}