using Blog.DTO;
using Blog.Entity;

namespace Blog.Interfaces
{
    public interface IPostService
    {
        public Task<Result<PostToFront>> CreatePostAsync(Guid userId,PostToBack post);
        public Task<Result<PostToFront>> GetPostByIdAsync(Guid PostId);
        public Task<Result<PagesObject<PostToFront>>> GetAllUserPostByIdPagesAsync(Guid UserId, int page, int pageSize=100);
        public Task<Result<bool>> DeletePostAsync(Guid userId, Guid postId);
        public Task<Result<PostToFront>> UpdatePostAsync(Guid userId,Guid postId, UpdatedPost post);
    }
}