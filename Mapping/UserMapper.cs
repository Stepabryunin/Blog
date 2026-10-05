using Blog.DTO;
using Blog.Entity;
namespace Blog.Mapper
{
    public class UserMapper
    {
        public UserToFront ToFront(User user)
        {
            return new UserToFront(user.Username, user.Email);
            
        }
    }
}