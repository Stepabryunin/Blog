using Blog.Entity;
using Blog.DTO;
using Blog.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using blog.Context;
using DevOne.Security.Cryptography.BCrypt;
using Blog.Mapper;

namespace Blog.Services
{
    public class UserService: IUserService
    {
        private readonly MyDbContext _db;
        private readonly UserMapper _userMapper;

        public UserService(MyDbContext dbContext)
        {
            _db=dbContext;

        }
        
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
           return await _db.Users.FirstOrDefaultAsync(u =>u.Id==userId);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
           return await _db.Users.FirstOrDefaultAsync(u =>u.Email==email);
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(u=>u.Username==username);
        }
        public async Task<bool> AddUserAsync(User user)
        {
            if (await GetUserByEmailAsync(user.Email) == null && GetUserByUsernameAsync(user.Username)==null)
                {
                    await _db.Users.AddAsync(user);
                    return true;
                }
            return false;
        }
        public async Task<Result<PagesObject<UserToFront>>> GetAllUsersByUsernameAsync(string username, int page=1, int pageSize=20)
        {
            if (page<1)
                page=1;
            if (pageSize<1)
                pageSize=20;
            if (pageSize>20)
                pageSize=20;
            List<UserToFront> res = await _db.Users
                .AsNoTracking()
                .Where(u => u.Username.StartsWith(username))
                .OrderBy(u=>u.Username)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Select(u => new UserToFront(u.Username,u.Email))
                .ToListAsync();
            int total = await _db.Users
                .AsNoTracking()
                .Where(u=>u.Username.StartsWith(username))
                .CountAsync();
            return new Result<PagesObject<UserToFront>>(ResultStatus.Ok, new PagesObject<UserToFront>(res,total,page,pageSize),null);
        }
        
        

    }
}