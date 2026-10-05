using Blog;
using Blog.Entity;
using Microsoft.EntityFrameworkCore;

namespace blog.Context
{
    
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users {get;set;} = null!;
        public DbSet<RefreshToken> Tokens {get;set;} = null!;
        public DbSet<Post> Posts {get;set;} = null!;
        public DbSet<Like> Likes {get;set;} = null!;
        public DbSet<Comment> Comments {get;set;} = null!;

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Like>(entity=>
            {
                entity.HasKey(l=> new {l.PostId, l.UserId});
                entity.HasOne(l =>l.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(l=>l.PostId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(l=>l.User)
                    .WithMany(u => u.Likes)
                    .HasForeignKey(l=>l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity=>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Tokens)
                    .HasForeignKey(r =>r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c =>c.User)
                    .WithMany(u=>u.Comments)
                    .HasForeignKey(c=>c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}