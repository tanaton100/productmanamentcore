using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Services;

namespace ProductmanagementCore.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration)
        {
            _userService = new UserService(configuration);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetUser()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUser([FromRoute]int id)
        {
            var result = _userService.GetByIdUsers(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddUser([FromBody]UserInputModel users)
        {
            var input = new Users
            {
                Lastname = users.Lastname,
                Email = users.Email,
                Firstname = users.Fristname,
                Password = users.Password,
                Username = users.Username,
                Tel = users.Tel
            };
            var result = _userService.AddUsers(input);
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody]UserInputModel users)
        {
            var input = new Users
            {
                Id =id,
                Lastname = users.Lastname,
                Email = users.Email,
                Firstname = users.Fristname,
                Password = users.Password,
                Username = users.Username,
                Tel = users.Tel
            };
            var result = _userService.UpdateUser(input);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser([FromRoute]int id)
        {
            var result = _userService.DeleteUserById(id);
            return Ok(result);
        }
    }
}