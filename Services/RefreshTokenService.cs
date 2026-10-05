using System.Text;
using blog.Context;
using Blog.DTO;
using Blog.Entity;
using Blog.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class RefreshTokenService :IRefreshTokenService
    {
        private readonly MyDbContext _db;

        public RefreshTokenService(MyDbContext db)
        {
            _db=db;
        } 
        public async Task<string> CreateTokenAsync(Guid userId, DateTime expireAt)
        {

            var token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(128));
            var tokenEntity  = new RefreshToken(ToHashToken(token),userId, expireAt);
            await _db.Tokens.AddAsync(tokenEntity);
            await _db.SaveChangesAsync();
            return token;
        }
        public async Task<RefreshTokenUpdated?> RotateTokenAsync(string oldToken, DateTime expireAt)
        {
            var oldTokenEntity = await GetTokenByHashAsync(ToHashToken(oldToken)); 
            if (oldTokenEntity==null)
                return null;
            if  (oldTokenEntity.IsRevoked || DateTime.UtcNow>=oldTokenEntity.ExpireAt)
                return null;
            await using var tr = await _db.Database.BeginTransactionAsync();
            int str = await _db.Tokens.
                Where(t =>t.Id==oldTokenEntity.Id && !t.IsRevoked)
                .ExecuteUpdateAsync(s => s.SetProperty(t=>t.IsRevoked,true));
            if (str==0)
            {
                await tr.RollbackAsync();
                return null;
            }
            var newToken= await CreateTokenAsync(oldTokenEntity.UserId,expireAt);
            await tr.CommitAsync();
            return new RefreshTokenUpdated(oldTokenEntity.UserId, newToken);
        }
        public async Task<bool> RevokeToken(Guid userId, string token)
        {
            var tokenEntity = await GetTokenByHashAsync(ToHashToken(token));
            if (tokenEntity is null)
                return false;
            if (tokenEntity.UserId != userId)
                return false;
            tokenEntity.IsRevoked = true;
            await _db.SaveChangesAsync();
            return true;
        }
        private string ToHashToken(string token)
        {
            using(var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(hash);
            }
        }
        private async Task<RefreshToken?> GetTokenByHashAsync(string hashToken)
        {
            return await _db.Tokens.FirstOrDefaultAsync(t => t.TokenHash==hashToken);
        }


    }
}