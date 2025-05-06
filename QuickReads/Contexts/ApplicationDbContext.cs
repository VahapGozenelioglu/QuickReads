using Microsoft.EntityFrameworkCore;
using QuickReads.Entities;

namespace QuickReads.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Article> Articles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ArticleTagAssoc> ArticleTagAssocs { get; set; }
    public DbSet<UserTagAssoc> UserTagAssocs { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }

    public DbSet<UserEntity> Users { get; set; }
}