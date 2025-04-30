using QuickReads.Entities.Enums;

namespace QuickReads.Entities;

public class Article : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public string ImageUrl { get; set; }
    public string CopyrightDetail { get; set; }
    public int WordCount { get; set; }
    public int ReadTimeInMinute { get; set; }
    public DateTime PublishedDate { get; set; }
    public LanguageEnum Language { get; set; }
    public List<Tag> Tags { get; set; }
    public List<CategoryEnum> Categories { get; set; }
}