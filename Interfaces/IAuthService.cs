using Blog.DTO;
using Blog.Entity;
namespace Blog.Interfaces
{
    public interface IAuthService
    {
            public Task<Result<AuthResult>> CreateUser(CreatedUser User);
            public Task<Result<AuthResult>> Login(LoginUser User);
            public Task<Result<AuthResult>> Refresh(RefreshTokenDTO Token);
           public Task<Result<bool>> Logout(Guid userId,string token);

    }
}