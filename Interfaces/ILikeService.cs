using Blog.DTO;
using Blog.Entity;

namespace Blog.Interfaces
{
    public interface ILikeInterface
    {
        public Task<Result<LikeDTO>> CreateLikeAsync(Guid userId, Guid postId);
        public Task<Result<bool>> DeleteLikeAsync(Guid userId, Guid postId);
    }
}