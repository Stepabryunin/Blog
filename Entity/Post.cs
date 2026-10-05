namespace Blog.Entity
{

    public class Post
    {
        public Guid Id {get;set;}
        public string Title {get;set;}
        public string Description {get;set;}
        public Guid UserId {get;set;}
       public DateTime CreatedAt {get;set;}
       public DateTime UpdatedAt {get;set;}
       public DateTime? DeleteddAt {get;set;}

       public User User {get;set;} =null!;
       public List<Like> Likes {get;set;} = new();
       public List<Comment> Comments {get;set;} = new(); 
    
    }

}