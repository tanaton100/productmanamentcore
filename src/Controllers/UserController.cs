using Microsoft.AspNetCore.Mvc;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.ModelInput;
using ProductmanagementCore.Services;
using System.Threading.Tasks;

namespace ProductmanagementCore.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUser()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser([FromRoute]int id)
        {
            var result = await _userService.GetByIdUsers(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddUser([FromBody]UserInputModel users)
        {
            var input = new Users
            {
                Lastname = users.Lastname,
                Email = users.Email,
                Firstname = users.Fristname,
                Username = users.Username,
                Tel = users.Tel
            };
            var result = await _userService.AddUsers(input);
            return Accepted(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]int id, [FromBody]UserInputModel users)
        {
            var input = new Users
            {
                Id =id,
                Lastname = users.Lastname,
                Email = users.Email,
                Firstname = users.Fristname,
                Username = users.Username,
                Tel = users.Tel
            };
            var result = await  _userService.UpdateUser(input);
            return Accepted(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {
            var result = await _userService.DeleteUserById(id);
            return Accepted(result);
        }
        [HttpGet("hhh")]

        public IActionResult Test()
        {
            return Ok(new CrysTal());
        }


    }

    public class CrysTal
    {
        public bool Test { get; set; }
    }
}