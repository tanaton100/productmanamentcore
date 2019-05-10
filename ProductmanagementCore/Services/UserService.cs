using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using ProductmanagementCore.Repository;
using ServiceStack;
using System.Data;

namespace ProductmanagementCore.Services
{
    public interface IUserService
    {
        IEnumerable<Users> GetAllUsers();
        Users GetByIdUsers(int id);
        Users AddUsers(Users users);
        Users UpdateUser(Users users);
        bool DeleteUserById(int id);
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

        public IEnumerable<Users> GetAllUsers()
        {
            var users = _userRepository.GetAll().ToList();
        
            var order = _ordersRepository.GetAll().ToList();
            var products = _productRepository.GetAll().ToList();

            foreach (var user in users)
            {
                var orderUser = order.Where(o => o.IdUser == user.Id);

                var listProduct = products.Where(p => orderUser.Any(ou => ou.IdProduct == p.Id)).ToList();
                user.Products = listProduct;
            }

            return users;
        }


        public Users GetByIdUsers(int id)
        {
            return _userRepository.FindById(id);
        }

        public Users AddUsers(Users users)
        {
            var checkDuplicate = ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DuplicateName");

            var id = _userRepository.Add(users);
            users.Id = id;
            return id > 0 ? users : new Users();
        }

        public Users UpdateUser(Users users)
        {
            var checkDuplicate = ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DuplicateName");

            var result = _userRepository.Update(users) > 0;
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
                Password = users.Password,
                Products = users.Products,
                Tel = users.Tel
            };
            return viewResult;
        }
        public bool DeleteUserById(int id)
        {
            return _userRepository.Delete(id) > 0;
        }


        private bool ValidateUserNameExist(Users users)
        {
            var userList = _userRepository.GetAll();
            var duplicated = userList.Where(d => d.Username.ToLower().Trim() == users.Username.ToLower().Trim() && d.Id != users.Id);

            return duplicated.Any();
        }

        public Users GetProductsWithUser(int id)
        {
            var user = _userRepository.FindById(id);
            var ordersUserId = _ordersRepository.FindByUserId(id);
            var products = _productRepository.GetAll();
            var productModel = ordersUserId.Select(order =>products.FirstOrDefault(p=>p.Id==order.IdProduct)).ToList();
            user.Products = productModel;
            return user;
        }

    }
}
