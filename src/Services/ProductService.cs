using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Mapster;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.Dto;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Repository;
using ToPage;

namespace ProductmanagementCore.Services
{
    public interface IProductService
    {
        Task<IQueryable<Products>> QueryBy(Func<Products, bool> predicate);
        Task<IEnumerable<Products>> GetAll();
        Task<Products> FindById(int id);
        Task<Products> Add(Products product);
        Task<Products> Update(Products product);
        Task<ResponseProductView> GetByCretiria(CriteriaRequest request);
        Task<bool> Delete(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<Products>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<ResponseProductView> GetByCretiria(CriteriaRequest request)
        {
            var productlist = await _productRepository.GetAll();
            var resultSorting = productlist.Adapt<IEnumerable<ProductDto>>();
            return FilterAndPagination(resultSorting, request);
        }

        private ResponseProductView FilterAndPagination(IEnumerable<ProductDto> products, CriteriaRequest request)
        {
            var page = request.Page == 0 ? 1 : request.Page;
            var perPage = request.PerPage == 0 ? 10 : request.PerPage;
            var filter = request.Filter?.ToLower();

            var productViewDtos = products.ToList();
            if (!string.IsNullOrEmpty(filter))
            {
                productViewDtos = productViewDtos.Where(cpv =>
                    cpv.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
                   
                ).ToList();
            }
            var result = SortingProductList(productViewDtos, request).ToPageWithCounts(page,perPage);
            
            return new ResponseProductView
            {
                ProductDtos = result,
                Page = result.PageNumber,
                PerPage = perPage,
                TotalPage = result.PageCount,
                TotalRecord = result.ItemCount
            };

        }
        private IEnumerable<ProductDto> SortingProductList(IEnumerable<ProductDto> products,
            CriteriaRequest request)
        {
            var productsList = products.ToList();
            try
            {
                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    var columnName = request.SortColumn;
                    var sortDesc = request.SortDesc ?? false;
                    var isDesc = sortDesc ? "desc" : "asc";

                    var orderString = columnName + " " + isDesc;
                    productsList = productsList.AsQueryable().OrderBy(orderString).ToList();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return productsList;
        }

        public async Task<Products> FindById(int id)
        {
            return await _productRepository.FindById(id);
        }

        public async Task<Products> Add(Products product)
        {
            var result = await _productRepository.AddAsync(product);
            if (result == 0)
            {
                throw new Exception("Cannot Add");
            }
            product.Id = result;
            return result > 0 ? product : new Products();

        }

        public async Task<Products> Update(Products product)
        {
            var result = await _productRepository.UpdateAsync(product) > 0;
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

        public async Task<bool> Delete(int id)
        {
            return await _productRepository.DeleteAsync(id) > 0;
        }

        public async Task<IQueryable<Products>> QueryBy(Func<Products, bool> predicate)
        {
            return await _productRepository.QueryBy(predicate);
        }
    }
}
