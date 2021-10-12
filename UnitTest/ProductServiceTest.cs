﻿using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using ProductmanagementCore.Models;
using ProductmanagementCore.Repository;
using ProductmanagementCore.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ProductServiceTest
    {
        private AutoMock _autoMock;
        private IProductService _productService;
        private IProductRepository _productRepository;


        [SetUp]
        public void Setup()
        {
            _autoMock = AutoMock.GetLoose();
            _productService = _autoMock.Create<ProductService>();
            _productRepository = _autoMock.Create<ProductRepository>();
        }
        [TearDown]
        public void TearDown()
        {
            _autoMock.Dispose();
        }

        [Test]
        public async Task Given_WhenGetAllProducts_ThenReturenResultCount2()
        {
            _autoMock.Mock<IProductRepository>().Setup(method => method.GetAll()).ReturnsAsync(ProductsMock());
            var result = (await _productService.GetAll());
            Assert.AreEqual(result.Count(), ProductsMock().Count());

        }

        [Test]
        public async Task GivenId1_WhenGetById_ThenReturenResult1()
        {
            var productId = 1;
            var dataMockReturn = ProductsMock().FirstOrDefault(p => p.Id == productId);
            _autoMock.Mock<IProductRepository>().Setup(method => method.FindById(It.IsAny<int>()))
                .Returns(() => new ValueTask<Products>(dataMockReturn));
            var result = (await _productService.FindById(productId));
            Assert.AreEqual(result, dataMockReturn);
        }



        public IEnumerable<Products> ProductsMock()
        {

            yield return new Products { Id = 1, Name = "Iphone" };
            yield return new Products { Id = 2, Name = "Ipad" };
        }

    }
}
