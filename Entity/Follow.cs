namespace Blog.Entity
{
    public class Follow
    {
        public Guid FollowerId {get;set;}
        public Guid FollowingId {get;set;}

        public User Follower {get;set;}
        public User Following {get;set;}

        public Follow(Guid followerId, Guid followingId)
        {
            FollowerId=followerId;
            FollowingId = followingId;
        }

    }
}