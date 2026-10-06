namespace Blog.DTO
{
    public class PostToFront
    {
        public Guid PostId {get;set;}
        public string Title {get;set;}
        public string Description {get;set;}
        public Guid UserId {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public PostToFront(Guid postId, string title, string description, Guid userId, DateTime createdAt, DateTime updatedAt)
        {
            PostId =postId;
            Title = title;
            Description = description;
            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }    
}