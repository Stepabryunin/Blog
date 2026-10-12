using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Blog.Entity
{
    public class User
    {
        public Guid Id {get;set;}
        public string Username {get; set;}
        public string Email {get;set;}
        public string PasswordHash {get; set;}
        public RoleEnum Role {get;set;}
        public DateTime CreatedAt {get; set;}

        public List<Post> Posts {get;set;} = new();
        public List<RefreshToken> Tokens {get;set;} = new();
        public List<Comment> Comments {get;set;} = new();
        public List<Like> Likes {get;set;} = new();

        public List<Follow> Followers {get;set;} = new();
        public List<Follow> Followings {get;set;} = new();

        public User(string username, string email, string passwordHash, RoleEnum role)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role=role;
            CreatedAt= DateTime.UtcNow;

        }
    }

}