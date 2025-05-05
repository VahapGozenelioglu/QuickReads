using QuickReads.Contexts;
using QuickReads.Entities;

namespace QuickReads.Services;

public class CreateNewArticle
{
    private static ApplicationDbContext _context;

    
    public class Response
    {
    }

    public CreateNewArticle(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(ArticleCreateDto req)
    {
        var article = InitializeFields(req);
        
        CalculateReadTime(req, article);

        var svc = new BulkImportTags(_context);
        var tags = svc.Invoke( new BulkImportTags.Request(){TagNames = req.TagNames});
        article.ArticleTagAssocs = new ();
        
        _context.Articles.Add(article);
        _context.SaveChanges();
        
        article.ArticleTagAssocs.AddRange(tags.Tags.Select(tag => new ArticleTagAssoc()
        {
            TagId = tag.Id,
            ArticleId = article.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }));
        _context.SaveChanges();
        
        return new Response() { };
    }

    private static void CalculateReadTime(ArticleCreateDto req, Article article)
    {
        article.WordCount = req.Content?.Split(' ').Length ?? 0;
        article.ReadTimeInMinute = (int)Math.Ceiling((double)article.WordCount / 200);
    }

    private static Article InitializeFields(ArticleCreateDto req)
    {
        var tag = new Article()
        {
            Title = req.Title,
            Content = req.Content,
            Language = req.Language,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false,
            Author = req.Author,
            Categories = req.Categories,
            ArticleTagAssocs = new(),
            CopyrightDetail = req.CopyrightDetail,
            ImageUrl = req.ImageUrl,
            WordCount = 0,
            ReadTimeInMinute = 0,
            PublishedDate = req.PublishedDate ?? DateTime.UtcNow,
            ShortContent = req.Content.Length > 10 ? req.Content.Substring(0, 10) : req.Content
        };
        return tag;
    }
}