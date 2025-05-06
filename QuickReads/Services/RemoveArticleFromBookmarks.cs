using QuickReads.Contexts;
using QuickReads.Entities.DTOs;

namespace QuickReads.Services;

public class RemoveArticleFromBookmarks
{
    private static ApplicationDbContext _context;
    
    public class Response
    {
    }

    public RemoveArticleFromBookmarks(ApplicationDbContext context)
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
            _context.Bookmarks.RemoveRange(bookmarks);
            _context.SaveChanges();
        }
        
        return new Response();
    }

}