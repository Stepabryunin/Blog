using blog.Context;
using Blog.Interfaces;
using Blog.DTO;
using Microsoft.EntityFrameworkCore;
using Blog.Entity;
using Blog.Mapper;

namespace Blog.Services
{
    public class FollowerService:IFollowerService
    {
        private readonly MyDbContext _db;
        private readonly IUserService _userService;
        private readonly UserMapper _userMapper;
        public FollowerService(MyDbContext db, IUserService userService, UserMapper userMapper)
        {
            _db=db;
            _userService= userService;
            _userMapper = userMapper;
        }

        public async Task<Result<FollowDTO>> FolowingToAsync(Guid followerId, string bloggerUserName)
        {
            var follower = await _userService.GetUserByIdAsync(followerId);
            if (follower is null)
                return new Result<FollowDTO>(ResultStatus.Unauthorized, null, null);
            var blogger = await _userService.GetUserByUsernameAsync(bloggerUserName);
            if (blogger is null)
                return new Result<FollowDTO>(ResultStatus.NotFound,null,"Блоггер с таким именем не найден");
            var follow = await _db.Followers.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId==blogger.Id);
            if (follow is not null)
                return new Result<FollowDTO>(ResultStatus.Conflict,null,"Уже подписан");
            var newFollow = new Follow(followerId, blogger.Id);
            await _db.Followers.AddAsync(newFollow);
            return new Result<FollowDTO>(ResultStatus.Ok, new FollowDTO(newFollow.FollowerId, newFollow.FollowingId),null);
        }

        public async Task<Result<FollowDTO>> UnFolowingToAsync(Guid followerId, string bloggerUserName)
        {
            var follower = await _userService.GetUserByIdAsync(followerId);
            if (follower is null)
                return new Result<FollowDTO>(ResultStatus.Unauthorized, null, null);
            var blogger = await _userService.GetUserByUsernameAsync(bloggerUserName);
            if (blogger is null)
                return new Result<FollowDTO>(ResultStatus.NotFound,null,"Блоггер с таким именем не найден");
            var follow = await _db.Followers.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId==blogger.Id);
            if (follow is null)
                return new Result<FollowDTO>(ResultStatus.Conflict,null,"Не подписан");
            _db.Followers.Remove(follow);
            return new Result<FollowDTO>(ResultStatus.NoContent, null,null);
        }

        public async Task<PagesObject<UserToFront>> GetFollowersAsync(string userName,int page=1, int pageSize=20)
        {
            var user = await _userService.GetUserByUsernameAsync(userName);
            if (user is null)
                return new PagesObject<UserToFront>(new List<UserToFront>(),0,0,0);
            if (page<1)
                page=1;
            if (pageSize<1)
                pageSize=20;
            if (pageSize>20)
                pageSize=20;
            List<UserToFront> result = await _db.Followers
                .AsNoTracking()
                .Where(f=>f.FollowingId==user.Id)
                .OrderBy(f=>f.FollowerId)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Select(f=> new UserToFront(f.FollowerId.ToString(),f.Follower.Email))
                .ToListAsync();
            int total = await _db.Followers
                .AsNoTracking()
                .Where(f=>f.FollowingId==user.Id)
                .CountAsync();
            return new PagesObject<UserToFront>(result,total,page,pageSize);
        }

        public async Task<PagesObject<UserToFront>> GetFollowingAsync(string userName,int page=1, int pageSize=20)
        {
            var user = await _userService.GetUserByUsernameAsync(userName);
            if (user is null)
                return new PagesObject<UserToFront>(new List<UserToFront>(),0,0,0);
            if (page<1)
                page=1;
            if (pageSize<1)
                pageSize=20;
            if (pageSize>20)
                pageSize=20;
            List<UserToFront> result = await _db.Followers
                .AsNoTracking()
                .Where(f=>f.FollowerId==user.Id)
                .OrderBy(f=>f.FollowingId)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Select(f=> new UserToFront(f.FollowerId.ToString(),f.Follower.Email))
                .ToListAsync();
            int total = await _db.Followers
                .AsNoTracking()
                .Where(f=>f.FollowerId==user.Id)
                .CountAsync();
            return new PagesObject<UserToFront>(result,total,page,pageSize);
        }
        
    }
}