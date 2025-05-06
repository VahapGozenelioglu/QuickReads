namespace QuickReads.Entities;

public class Bookmark : BaseEntity
{
    public int UserId { get; set; }
    public int ArticleId { get; set; }
    public UserEntity UserEntity { get; set; }
    public Article Article { get; set; }
}