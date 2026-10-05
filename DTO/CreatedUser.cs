using System.ComponentModel.DataAnnotations;

namespace Blog.DTO
{
    public class CreatedUser
    {
        public string Username {get;set;}
        [EmailAddress]
        public string Email {get;set;}
        public string Password {get;set;}

    }
}