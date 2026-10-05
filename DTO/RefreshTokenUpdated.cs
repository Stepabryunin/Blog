namespace Blog.DTO
{
    public class RefreshTokenUpdated
    {
        public Guid UserId {get;}
        public string RefreshToken {get;}
        public RefreshTokenUpdated(Guid id, string token)
        {
            UserId = id;
            RefreshToken = token;
        }
    }
}