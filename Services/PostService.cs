using blog.Context;
using Blog.DTO;
using Blog.Entity;
using Blog.Interfaces;
using Blog.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class PostService:IPostService
    {
        private readonly MyDbContext _db;
        private readonly PostMapper _postMapper;
        public PostService(MyDbContext myDbContext, PostMapper postMapper)
        {
            _db=myDbContext;
            _postMapper = postMapper;
        }

        public async Task<Result<PostToFront>> CreatePostAsync(Guid userId,PostToBack post)
        {
            var newPost = new Post(post.Title, post.Description, userId);
            await _db.Posts.AddAsync(newPost);
            await _db.SaveChangesAsync();
            return new Result<PostToFront>(ResultStatus.Ok, _postMapper.ToFront(newPost), null);
        }
        public async Task<Result<PostToFront>> GetPostByIdAsync(Guid PostId)
        {
            var result = await _db.Posts.FirstOrDefaultAsync(p => p.Id==PostId && p.DeletedAt==null);
            if (result == null)
                return new Result<PostToFront>(ResultStatus.NotFound, null, "Пост не найден");
            return new Result<PostToFront>(ResultStatus.Ok,_postMapper.ToFront(result),null);
        }
        private async Task<Post?> GetPostByIdAsyncInternal(Guid PostId)
        {
            return  await _db.Posts.FirstOrDefaultAsync(p => p.Id==PostId && p.DeletedAt==null);
        }

        public async Task<Result<PagesObject<PostToFront>>> GetAllUserPostByIdPagesAsync(Guid userId, int page, int pageSize=100)
        {
            if (page<1)
                page=1;
            if (pageSize<1)
                pageSize=1;
            if (pageSize>100)
                pageSize=100;
            List<PostToFront> posts= await _db.Posts
                    .AsNoTracking()
                    .Where(p =>p.UserId==userId && p.DeletedAt==null)
                    .OrderByDescending(i =>i.CreatedAt)
                    .ThenByDescending(i => i.Id)
                    .Skip((page -1)*pageSize)
                    .Take(pageSize)
                    .Select(p=> new PostToFront(p.Id, p.Title, p.Description,p.UserId,p.CreatedAt, p.UpdatedAt))
                    .ToListAsync();
            int total = await _db.Posts
                .AsNoTracking()
                .Where(p =>p.UserId==userId && p.DeletedAt==null)
                .CountAsync();
            return new Result<PagesObject<PostToFront>>(ResultStatus.Ok, new PagesObject<PostToFront>(posts,total, page, pageSize), null);
                
        }

        public async Task<Result<bool>> DeletePostAsync(Guid userId, Guid postId)
        {
            var post = await GetPostByIdAsyncInternal(postId);
            if (post is null || post.DeletedAt is not null )
                return new Result<bool>(ResultStatus.NotFound,false,"Ресурс не найден");
            if (post.UserId != userId)
                return new Result<bool>(ResultStatus.Forbidden, false,"Нет доступа к данному ресурсу");
            post.DeletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return new Result<bool>(ResultStatus.NoContent, true, null);
        }
        public async Task<Result<PostToFront>> UpdatePostAsync(Guid userId,Guid postId, UpdatedPost post)
        {
            var oldPost = await GetPostByIdAsyncInternal(postId);
            if (oldPost is null || oldPost.DeletedAt is not null)
                return new Result<PostToFront>(ResultStatus.NotFound,null,"Ресурс не наден");
            if (oldPost.UserId != userId)
                return new Result<PostToFront>(ResultStatus.Forbidden, null, "Нет доступа к ресурсу");
            bool isUpdated=false;
            if (post.Title is not null)
            {
                oldPost.Title = post.Title;
                isUpdated=true;
            }
            if (post.Description is not null)
            {
                oldPost.Description=post.Description;
                isUpdated = true;
            }
            if (isUpdated)
            {
                oldPost.UpdatedAt=DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return new Result<PostToFront>(ResultStatus.Ok, _postMapper.ToFront(oldPost),null);
            }
            return new Result<PostToFront>(ResultStatus.BadRequest, null,"Нечего обновлять");
        }
    }
}
