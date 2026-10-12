namespace Blog.DTO
{
    public class FollowDTO
    {
        public Guid FollowerId {get;set;}
        public Guid FollowingId {get;set;}

        public FollowDTO(Guid followerId, Guid followingId)
        {
            FollowerId = followerId;
            FollowingId=followingId;
        }
    }
}