namespace QuickReads.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public List<ArticleTagAssoc> ArticleTagAssocs { get; set; }
}