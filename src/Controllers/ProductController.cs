using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Services;
using System;
using System.Threading.Tasks;
using OfficeOpenXml.Style;
using System.IO;

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
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            return Ok(await _productService.QueryBy(p => p.Id == id));
        }


        [HttpGet]
        [Route("export")]
        public async Task<IActionResult> ExportExcel()
        {
            var products = await _productService.GetAll();
            var excelName = $"{DateTime.Now:dd/MM/yyyy}-product";
            using var excelPackage = new ExcelPackage();
            var workbook = excelPackage.Workbook;
            var worksheet = workbook.Worksheets.Add("product");

            RenderHeader(worksheet);

            var dataRow = 2;
            var column = 1;
            foreach (var item in products)
            {
                worksheet.SetValue(dataRow, column++, item.Id);
                worksheet.SetValue(dataRow, column, item.Name ?? "");
                dataRow++;
                column = 1;
            }

            return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet]
        [Route("export1")]
        public async Task<IActionResult> ExportExcel1()
        {
            var products = await _productService.GetAll();
            var excelName = $"{DateTime.Now:dd/MM/yyyy}-product";
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("product");
                workSheet.Cells.LoadFromCollection(products, true);
                package.Save();
            }
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] ProductInput product)
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProductInput product)
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



        private void RenderHeader(ExcelWorksheet worksheet)
        {

            string[] headers =
           {
                     "Id",
                     "Name"
                 };
            var headerdatarow = 1;
            foreach (var header in headers)
            {

                worksheet.Cells[1, headerdatarow++].Value = header;
            }
        }
    }
}