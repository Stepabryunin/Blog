namespace Blog.DTO
{
    public class LikeDTO
    {
        public Guid UserId {get;set;}
        public Guid PostId {get;set;}

        public LikeDTO(Guid userId, Guid postId)
        {
            UserId = userId;
            PostId=postId;
        }
    }
}