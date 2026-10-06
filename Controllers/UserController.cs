using blog.Context;
using Blog.DTO;
using Blog.Entity;
using Blog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        public UserController (IUserService userService)
        {
            _userService=userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByName([FromQuery] string username, [FromQuery] int page,[FromQuery] int pageSize )
        {
            var res = await _userService.GetAllUsersByUsernameAsync(username,page,pageSize);
            return res.ToAction();
        }
    }
}