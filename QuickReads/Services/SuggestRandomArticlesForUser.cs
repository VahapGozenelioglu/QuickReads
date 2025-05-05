using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Extensions;

namespace QuickReads.Services;

public class SuggestRandomArticlesForUser
{
    private static ApplicationDbContext _context;
    private const int DefaultTagCount = 3;
    private const int DefaultArticleCount = 5;

    public class Request
    {
        public int UserId { get; set; }
    }

    public class Response
    {
        public List<ArticleDto> Articles { get; set; }
    }

    public SuggestRandomArticlesForUser(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Response Invoke(Request req)
    {
        var userTagIds = _context
            .UserTagAssocs
            .Where(x => x.UserId == req.UserId && x.LikeCount > 0)
            .OrderByDescending(x => x.LikeCount)
            .Select(x => x.TagId)
            .Take(DefaultTagCount)
            .ToList();
        
        var articles = _context.ArticleTagAssocs
            .Include(x => x.Article)
            .ThenInclude(x => x.ArticleTagAssocs)
            .ThenInclude(x => x.Tag)
            .Where(x => userTagIds.Contains(x.TagId))
            .Select(x => x.Article.ToDto())
            .OrderBy(x => Guid.NewGuid())
            .Take(DefaultArticleCount)
            .ToList();
        
        return new Response() { Articles = articles };
    }
}