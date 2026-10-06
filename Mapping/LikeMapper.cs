using Blog.DTO;
using Blog.Entity;

namespace Blog.Mapper
{
    public class LikeMapper
    {
        public LikeDTO ToDTO(Like like)
        {
            return new LikeDTO(like.UserId,like.PostId);
        }
    }
}