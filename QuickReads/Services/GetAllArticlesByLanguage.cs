using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Entities.Enums;
using QuickReads.Extensions;

namespace QuickReads.Services;


public class GetAllArticlesByLanguage
{
    private static ApplicationDbContext _context;

    public class Request
    {
        public LanguageEnum Language { get; set; }
    }

    public class Response
    {
        public List<ArticleDto> Articles { get; set; }
    }

    public GetAllArticlesByLanguage(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(Request req)
    {
        var articles = _context.Articles
            .Include(x => x.Tags)
            .Where(x => x.Language == req.Language)
            .Select(x => x.ToDto())
            .ToList();
        
        return new Response() { Articles = articles };
    }
}