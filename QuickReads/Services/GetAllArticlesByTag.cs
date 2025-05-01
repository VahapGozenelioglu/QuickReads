using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Entities.Enums;
using QuickReads.Extensions;

namespace QuickReads.Services;


public class GetAllArticlesByTag
{
    private static ApplicationDbContext _context;

    public class Request
    {
        public String TagName { get; set; }
    }

    public class Response
    {
        public List<ArticleDto> Articles { get; set; }
    }

    public GetAllArticlesByTag(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(Request req)
    {
        var allArticles = _context.Articles
            .Include(x => x.Tags)
            .Select(x => x.ToDto())
            .ToList();

        var resp = allArticles.Where(x => x.Tags.Any( y => y.Name == req.TagName)).ToList();
        
        return new Response() { Articles = resp };
    }
}