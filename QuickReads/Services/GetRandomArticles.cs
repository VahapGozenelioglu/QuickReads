using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Extensions;

namespace QuickReads.Services;

public class GetRandomArticles
{
    private static ApplicationDbContext _context;
    private const int DefaultArticleCount = 5;

    public class Request
    {
        public int? ArticleCount { get; set; }
    }

    public class Response
    {
        public List<ArticleDto> Articles { get; set; }
    }

    public GetRandomArticles(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Response Invoke(Request req)
    {
        if(req.ArticleCount == null || req.ArticleCount <= 0)
        {
            req.ArticleCount = DefaultArticleCount;
        }
        
        var articles = _context.Articles
            .Include(x => x.ArticleTagAssocs)
            .ThenInclude(x => x.Tag)
            .Select(x => x.ToDto())
            .OrderBy(x => Guid.NewGuid())
            .Take(req.ArticleCount.Value)
            .ToList();
        
        return new Response() { Articles = articles };
    }
}