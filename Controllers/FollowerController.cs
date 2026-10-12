using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Interfaces;
using System.Security.Claims;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FollowerController:ControllerBase
    {
        private readonly IFollowerService _followerService;
        public FollowerController(IFollowerService followerService)=>_followerService=followerService;

        [HttpPost()]
        public async Task<IActionResult> FollowingTo([FromQuery] string bloggerName)
        {
            var userIdString  = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
            {
                return Unauthorized();
            }
            var result = await _followerService.FolowingToAsync(userId,bloggerName);
            return result.ToAction();
        }
        [HttpDelete()]
        public async Task<IActionResult> UnFollowing([FromQuery]  string bloggerName)
        {
            var userIdString  = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
            {
                return Unauthorized();
            }
            var result = await _followerService.UnFolowingToAsync(userId,bloggerName);
            return result.ToAction();
        }

        [HttpGet("followers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowers(
            [FromQuery] string userName,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _followerService.GetFollowersAsync(userName, page, pageSize);
            return Ok(result);
        }

        [HttpGet("following")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowing(
            [FromQuery] string userName,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _followerService.GetFollowingAsync(userName, page, pageSize);
            return Ok(result);
        }

        
        
    }


}