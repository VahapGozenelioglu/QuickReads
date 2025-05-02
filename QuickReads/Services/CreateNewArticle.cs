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
    public Response Invoke(ArticleDto req)
    {
        var article = InitializeFields(req);
        
        CalculateReadTime(req, article);

        var svc = new BulkImportTags(_context);
        svc.Invoke( new BulkImportTags.Request(){TagNames = req.Tags.Select(x => x.Name).ToList()});
        
        _context.Articles.Add(article);
        _context.SaveChanges();
        
        return new Response() { };
    }

    private static void CalculateReadTime(ArticleDto req, Article article)
    {
        article.WordCount = req.Content?.Split(' ').Length ?? 0;
        article.ReadTimeInMinute = (int)Math.Ceiling((double)article.WordCount / 200);
    }

    private static Article InitializeFields(ArticleDto req)
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
            Tags = new(),
            CopyrightDetail = req.CopyrightDetail,
            ImageUrl = req.ImageUrl,
            WordCount = 0,
            ReadTimeInMinute = 0,
            PublishedDate = req.PublishedDate ?? DateTime.UtcNow,
        };
        return tag;
    }
}