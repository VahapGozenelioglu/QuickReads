using QuickReads.Contexts;
using QuickReads.Entities.DTOs;

namespace QuickReads.Services;

public class UndoLikeArticle
{
    private static ApplicationDbContext _context;
    
    public class Response
    {
    }

    public UndoLikeArticle(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(ArticleUserDto req)
    {
        var tagIds = _context.ArticleTagAssocs
            .Where(x => x.ArticleId == req.ArticleId)
            .Select(x => x.TagId)
            .ToList();
        
        var userTagAssocs = _context.UserTagAssocs
            .Where(x => x.UserId == req.UserId && tagIds.Contains(x.TagId))
            .ToList();
       
        userTagAssocs.ForEach(x => x.LikeCount--);
        
        _context.UpdateRange(userTagAssocs);
        _context.SaveChanges();

        return new Response() { };
    }
}