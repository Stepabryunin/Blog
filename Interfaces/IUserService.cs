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
        public Task<Result<PagesObject<UserToFront>>> GetAllUsersByUsernameAsync(string username, int page=1, int pageSize=100);


        
    }
}