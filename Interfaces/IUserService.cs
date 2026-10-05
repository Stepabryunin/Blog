using Blog.DTO;
using Blog.Entity;

namespace Blog.Interfaces 
{
    public interface IUserService
    {
        public Task<bool> AddUserAsync(User user);
        public Task<User?> GetUserByIdAsync(Guid userId);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetUserByUsernameAsync(string username);

    }
}