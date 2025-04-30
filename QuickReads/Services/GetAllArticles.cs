using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Extensions;

namespace QuickReads.Services;

public class GetAllArticles
{
    private static ApplicationDbContext _context;
    public class Request { }

    public class Response
    {
        public List<ArticleDto> Articles { get; set; }
    }

    public GetAllArticles(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(Request req)
    {
        var articles = _context.Articles
            .Include(x => x.Tags)
            .Select(x => x.ToDto())
            .ToList();
        
        return new Response() { Articles = articles };
    }
}