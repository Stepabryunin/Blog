using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Interfaces;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeInterface _likeService;
        public LikeController(ILikeInterface likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike([FromQuery] Guid postId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
                return Unauthorized();
            var res = await _likeService.CreateLikeAsync(userId, postId);
            return res.ToAction();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLike([FromQuery] Guid postId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
                return Unauthorized();
            var res = await _likeService.DeleteLikeAsync(userId, postId);
            return res.ToAction();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountLikeBuPoost([FromQuery] Guid postId)
        {
            var res = await _likeService.GetLikeByPostAsync(postId);
            return res.ToAction();
        }

        
    }
}