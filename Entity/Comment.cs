
using Blog.Entity;

namespace Blog
{
    public class Comment
    {
        public Guid Id {get;set;}
        public string Text {get;set;}
        public Guid UserId {get;set;}
        public Guid PostId {get;set;}
        public Guid? ParrentCommentId {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        public User? User {get;set;}
    }
}