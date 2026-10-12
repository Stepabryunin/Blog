using Blog.DTO;

namespace Blog.Interfaces
{
    public interface IFollowerService
    {
        public Task<Result<FollowDTO>> FolowingToAsync(Guid followerId, string bloggerUserName);
        public Task<Result<FollowDTO>> UnFolowingToAsync(Guid followerId, string bloggerUserName);
        public Task<PagesObject<UserToFront>> GetFollowersAsync(string userName,int page=1, int pageSize=20);
        public Task<PagesObject<UserToFront>> GetFollowingAsync(string userName,int page=1, int pageSize=20);
        
    }
}