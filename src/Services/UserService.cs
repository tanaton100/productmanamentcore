using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProductmanagementCore.Models;
using ProductmanagementCore.Repository;
using System.Data;
using System.Threading.Tasks;
using Mapster;
using ProductmanagementCore.Models.Dto;

namespace ProductmanagementCore.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<Users> GetByIdUsers(int id);
        Task<Users> AddUsers(Users users);
        Task<Users> UpdateUser(Users users);
        Task<bool> DeleteUserById(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductRepository _productRepository;

        public UserService(IUserRepository userRepository, IOrdersRepository ordersRepository,
            IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _ordersRepository = ordersRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users =  _userRepository.GetAll();
            var order =  _ordersRepository.GetAll();
            var products =  _productRepository.GetAll();
             await Task.WhenAny(users, order,products);
             var resultUsers = await users;
             var resultOrder = await order;
             var resultProducts = await products;
            foreach (var user in resultUsers)
            {
                var orderUser = resultOrder.Where(o => o.UserId == user.Id);
                //  var oRderUserProductId = orderUser.Select(t => t.ProductId);
                var listProduct = resultProducts.Where(p => orderUser.Any(ou => ou.ProductId == p.Id)).ToList();
                // var list = products.Where(p => oRderUserProductId.Contains(p.Id));
                user.Products = listProduct;
            }

            //TypeAdapterConfig<Users, UserDto>
            //    .NewConfig()
            //    .Map(d => d.Id, s => s.Id)
            //    .Map(d => d.FullName, s => $"{s.Firstname} {s.Lastname}")
            //    .Map(d => d.Products, s => s.Products);//lookup 

            return resultUsers.Adapt<IEnumerable<UserDto>>();

        }

        public async Task<Users> GetByIdUsers(int id)
        {
            return await _userRepository.FindById(id);
        }

        public async Task<Users> AddUsers(Users users)
        {
            var checkDuplicate = await ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DuplicateName");

            var id = await _userRepository.AddAsync(users);
            users.Id = id;
            return id > 0 ? users : new Users();
        }

        public async Task<Users> UpdateUser(Users users)
        {
            var checkDuplicate = await ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DuplicateName");

            var result = await _userRepository.UpdateAsync(users) > 0;
            if (!result)
            {
                throw new Exception("Cannot Update");
            }

            var viewResult = new Users
            {
                Email = users.Email,
                Id = users.Id,
                Lastname = users.Lastname,
                Username = users.Username,
                Firstname = users.Firstname,
                Products = users.Products,
                Tel = users.Tel
            };
            return viewResult;
        }
        public async Task<bool> DeleteUserById(int id)
        {
            return await _userRepository.DeleteAsync(id) > 0;
        }


        private async Task<bool> ValidateUserNameExist(Users users)
        {
            var userList = await _userRepository.GetAll();
            var duplicated = userList.Where(d => d.Username.ToLower().Trim() == users.Username.ToLower().Trim() && d.Id != users.Id);

            return duplicated.Any();
        }

        public async Task<Users> GetProductsWithUser(int id)
        {
            var user = await _userRepository.FindById(id);
            var ordersUserId = await _ordersRepository.FindByUserId(id);
            var products = await _productRepository.GetAll();
            var productModel = ordersUserId.Select(order => products.FirstOrDefault(p => p.Id == order.ProductId)).ToList();
            user.Products = productModel;
            return user;
        }

    }
}
