namespace Blog.Entity
{
    public class RefreshToken
    {
        public Guid Id {get;set;}
        public string TokenHash {get;set;}
        public Guid UserId {get;set;}
        public bool IsRevoked {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime ExpireAt {get;set;}

        public User? User {get;set;}

        public RefreshToken(string TokenHash, Guid UserId,DateTime ExpireAt)
        {
            Id = Guid.NewGuid();
            this.TokenHash=TokenHash;
            this.UserId=UserId;
            IsRevoked = false;
            CreatedAt  =DateTime.UtcNow;
            this.ExpireAt = ExpireAt;
        }
    }
}