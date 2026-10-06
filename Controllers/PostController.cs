using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.DTO;
using Blog.Interfaces;
using System.Security.Claims;
namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController:ControllerBase
    {
        private readonly IPostService _postServie;
        public PostController(IPostService postService)
        {
            _postServie =postService;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostToBack post)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
                return Unauthorized();
            var result  = await _postServie.CreatePostAsync(userId,post);
            return result.ToAction();
        }
        [HttpGet("{postId:guid}")]
        public async Task<IActionResult> GetPostById(Guid postId)
        {
            var post = await _postServie.GetPostByIdAsync(postId);
            return post.ToAction();

        }  
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetPostsByUserIdPagesPagessize(Guid userId, [FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var res = await _postServie.GetAllUserPostByIdPagesAsync(userId,page,pageSize);
            return res.ToAction();
        }      

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString,out userId))
                return Unauthorized();
            var res = await _postServie.DeletePostAsync(userId, postId);
            return res.ToAction();
        }
        [HttpPatch("{postId}")]
        public async Task<IActionResult> UpatePost(Guid postId,[FromBody] UpdatedPost post)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
                return Unauthorized();
            var res = await _postServie.UpdatePostAsync(userId,postId, post);
            return res.ToAction();
        }

    }
}