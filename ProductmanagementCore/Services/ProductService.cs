using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductmanagementCore.Models;
using ProductmanagementCore.Repository;

namespace ProductmanagementCore.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAll();
        Task<Products> FindById(int id);
        Task<Products> Add(Products product);
        Task<Products> Update(Products product);
        Task<bool> Delete(int id);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<Products>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Products> FindById(int id)
        {
            return await _productRepository.FindById(id);
        }

        public async Task<Products> Add(Products product)
        {
            var result = await _productRepository.Add(product);
            if (result == 0)
            {
                throw new Exception("Cannot Add");
            }
            product.Id = result;
            return result > 0 ? product : new Products();

        }

        public async Task<Products> Update(Products product)
        {
            var result = await _productRepository.Update(product) > 0;
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

        public async Task <bool> Delete(int id)
        {
            return await _productRepository.Delete(id) > 0;
        }
    }
}
