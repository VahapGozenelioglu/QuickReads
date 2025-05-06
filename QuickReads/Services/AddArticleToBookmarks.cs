using QuickReads.Contexts;
using QuickReads.Entities;
using QuickReads.Entities.DTOs;

namespace QuickReads.Services;

public class AddArticleToBookmarks
{
    private static ApplicationDbContext _context;
    
    public class Response
    {
    }

    public AddArticleToBookmarks(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(ArticleUserDto req)
    {
        var bookmarks = _context.Bookmarks
            .Where(x => x.UserId == req.UserId && x.ArticleId == req.ArticleId)
            .ToList();

        if (bookmarks.Count > 0)
        {
            return new Response();
        }
        
        var bookmark = new Bookmark()
        {
            UserId = req.UserId,
            ArticleId = req.ArticleId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Bookmarks.Add(bookmark);
        _context.SaveChanges();
        
        return new Response();
    }

}