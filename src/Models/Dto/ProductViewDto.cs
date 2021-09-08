using System.Collections.Generic;

namespace ProductmanagementCore.Models.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ResponseProductView : PagingModel
    {
      public IEnumerable<ProductDto> ProductDtos { get; set; }
    }
}
