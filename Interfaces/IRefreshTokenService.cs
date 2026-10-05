using Blog.DTO;
using Blog.Entity;

namespace Blog.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task<string> CreateTokenAsync(Guid userId, DateTime expireAt);
        public Task<RefreshTokenUpdated?> RotateTokenAsync(string oldToken, DateTime expireAt);
        public Task<bool> RevokeToken(Guid userId, string token);
        
    }
}