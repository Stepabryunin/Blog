using blog.Context;
using Blog.Interfaces;
using Blog.DTO;
using Microsoft.EntityFrameworkCore;
using Blog.Entity;
using Blog.Mapper;

namespace Blog.Services
{
    public class LikeService : ILikeInterface
    {
        private readonly MyDbContext _db;
        private readonly LikeMapper _likeMapper;

        public LikeService (MyDbContext db, LikeMapper likeMapper)
        {
            _db=db;
            _likeMapper = likeMapper;
        }
        public async Task<Result<LikeDTO>> CreateLikeAsync(Guid userId, Guid postId)
        {
            var like = await _db.Likes.FirstOrDefaultAsync(l =>l.PostId==postId && l.UserId==userId);
            if (like!=null)
                return new Result<LikeDTO>(ResultStatus.Conflict, null, "Лайк уже стоит");
            var newLike = new Like(userId, postId);
            await _db.Likes.AddAsync(newLike);
            await _db.SaveChangesAsync();
            return new Result<LikeDTO>(ResultStatus.Ok,_likeMapper.ToDTO(newLike),null);
        }

        public async Task<Result<bool>> DeleteLikeAsync(Guid userId, Guid postId)
        {
            var like = await _db.Likes.FirstOrDefaultAsync(l =>l.PostId==postId && l.UserId==userId);
            if (like==null)
                return new Result<bool>(ResultStatus.NotFound, false, "Лайка не стоит");
            _db.Likes.Remove(like);
            await _db.SaveChangesAsync(); 
            return new Result<bool>(ResultStatus.NoContent,false,null);
        }

        public async Task<Result<int>> GetLikeByPostAsync(Guid postId)
        {
            var count_like = await _db.Likes
                .AsNoTracking()
                .Where(l=> l.PostId==postId)
                .CountAsync();
            return new Result<int>(ResultStatus.Ok,count_like,null);
        }

        
    }
}