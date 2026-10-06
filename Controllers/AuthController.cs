
using System.Security.Claims;
using Blog.DTO;
using Blog.Entity;
using Blog.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] CreatedUser user)
        {
            var res = await _authService.CreateUser(user);
            return res.ToAction();

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            var res = await _authService.Login(user);
            return res.ToAction();
            
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO token)
        {
            var res = await _authService.Refresh(token);
            return res.ToAction();
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO token)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString is null)
                return Unauthorized();
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
                return Unauthorized();
            var res = await _authService.Logout(userId,token.Token);
            return res.ToAction();
        }        

     
        
    }
}