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
        IEnumerable<Users> GetAllUserses();
        Users GetByIdUsers(int id);
        Users AddUsers(Users users);
        Users UpdateUser(Users users);
        bool DeleteUserById(int id);
    }

    public class UserService :IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductRepository _productRepository;

        public UserService(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
            _ordersRepository = new OrdersRepository(configuration);
            _productRepository = new ProductRepository(configuration);
        }

        public IEnumerable<Users> GetAllUserses()
        {
            var result = _userRepository.GetAll();
            var userId = result.Select(user => user.Id);
            var order = _ordersRepository.GetAll();
            var products = _productRepository.GetAll();
            foreach (var id in userId)
            {
                var userOrder = order.Where(o => o.IdUser == id);
               
                var productModel = userOrder.Select(orders => products.FirstOrDefault(p => p.Id == orders.IdProduct)).ToList();
                foreach (var prod in result)
                {
                    prod.Products = productModel;
                }
  
            }
         
            return result;
        }

        public Users GetByIdUsers(int id)
        {
            return _userRepository.FindById(id);
        }

        public Users AddUsers(Users users)
        {
            var checkDuplicate = ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DupicateName");

            var id = _userRepository.Add(users);
            users.Id = id;
            return id > 0 ? users : new Users();
        }

        public Users UpdateUser(Users users)
        {
            var checkDuplicate = ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DupicateName");

            var result = _userRepository.Update(users)>0;
            if (!result)
            {
                throw new Exception("Cannot Update");
            }

            var ViemResult = new Users
            {
                Email = users.Email,
                Id = users.Id,
                Lastname = users.Lastname,
                Username = users.Username,
                Fristname = users.Fristname,
                Password = users.Password,
                Products = users.Products,
                Tel = users.Tel
            };
            return ViemResult;
        }
        public bool DeleteUserById(int id)
        {
            return _userRepository.Delete(id)>0;
        }


        private bool ValidateUserNameExist(Users users)
        {
            var userses = _userRepository.GetAll();
            var duplicated = userses.Where(d => d.Username.ToLower().Trim() == users.Username.ToLower().Trim() && d.Id != users.Id);

            return duplicated.Any();
        }

        public Users GetProductsWithUser(int id)
        {
            var user = _userRepository.FindById(id);
            var ordersUserId = _ordersRepository.FindByUserId(id);
            var products = _productRepository.GetAll();
            var productModel = ordersUserId.Select(order => products.FirstOrDefault(p => p.Id == order.IdProduct)).ToList();

            user.Products = productModel;
            return user;
        }

    }
}
