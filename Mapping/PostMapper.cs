using Blog.DTO;
using Blog.Entity;

namespace Blog.Mapper
{
    public class PostMapper
    {
        public PostToFront ToFront(Post post)
        {
            return new PostToFront(post.Id,post.Title, post.Description, post.UserId, post.CreatedAt, post.UpdatedAt);
        }
    }
}