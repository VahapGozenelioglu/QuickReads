using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Extensions;

namespace QuickReads.Services;

public class GetArticleById
{
    private static ApplicationDbContext _context;

    public class Request
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public ArticleDto? Article { get; set; }
    }

    public GetArticleById(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Response Invoke(Request req)
    {
        var article = _context.Articles
            .Include(x => x.Tags)
            .FirstOrDefault(article => article.Id == req.Id)
            ?.ToDto();
        
        return new Response() { Article = article };
    }
}