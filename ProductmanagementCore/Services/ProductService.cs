using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using ProductmanagementCore.Repository;

namespace ProductmanagementCore.Services
{
    public interface IProductService
    {
        IEnumerable<Products> GetAll();
        Products FindById(int id);
        Products Add(Products product);
        Products Update(Products product);
        bool Delete(int id);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IConfiguration configuration)
        {
            _productRepository = new ProductRepository(configuration);
        }
        public IEnumerable<Products> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Products FindById(int id)
        {
            return _productRepository.FindBy(id);
        }

        public Products Add(Products product)
        {
            var result = _productRepository.Add(product);
            if (result==0)
            {
                throw new  Exception("Cannot Add");
            }
            product.Id = result;
            return result > 0 ? product : new Products();

        }

        public Products Update(Products product)
        {
            var result = _productRepository.Update(product)>0;
            if (!result)
            {
                throw new Exception("cannot Update");
            }

            var responesProduct = new Products
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            return responesProduct;
        }

        public bool Delete(int id)
        {
            return _productRepository.Delete(id) > 0;
        }
    }
}
