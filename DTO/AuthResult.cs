using Blog.DTO;
using Blog.Entity;

namespace Blog
{
    public class AuthResult
    {
        public UserToFront User {get; set;}
        public string AccessToken {get;set;}
        public string RefreshToken {get;set;}

        public AuthResult(UserToFront user, string accessToken, string refreshToken)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken =refreshToken;
        }
    }
}