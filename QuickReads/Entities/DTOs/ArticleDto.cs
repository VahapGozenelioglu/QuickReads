using QuickReads.Entities.DTOs;
using QuickReads.Entities.Enums;


public class ArticleDto
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ShortContent { get; set; }
    public string Author { get; set; }
    public string ImageUrl { get; set; }
    public string CopyrightDetail { get; set; }
    public int WordCount { get; set; }
    public int ReadTimeInMinute { get; set; }
    public DateTime? PublishedDate { get; set; }
    public LanguageEnum Language { get; set; }
    public List<TagDto> Tags { get; set; }
    public List<CategoryEnum> Categories { get; set; }
}