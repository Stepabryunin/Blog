using Blog.Entity;

namespace Blog.DTO
{
    public class UserToFront
    {
        public string Username {get;set;}
        public string Email {get;set;}

        public UserToFront(string username, string email)
        {
            Username = username;
            Email = email;
        }
    }
}