
using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Entities;
using QuickReads.Entities.DTOs;

namespace QuickReads.Services;

public class LikeArticle
{
    private static ApplicationDbContext _context;

    
    public class Response
    {
    }

    public LikeArticle(ApplicationDbContext context)
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
            .Where(x => x.UserId == req.UserId)
            .Select(x => x.TagId)
            .ToList();
        
        var commonTagIds = tagIds.Intersect(userTagAssocs).ToList();

        var commonTags = _context.UserTagAssocs
            .Where(x => commonTagIds.Contains(x.TagId))
            .ToList();
        commonTags.ForEach(x =>
        {
            x.LikeCount++;
            x.UpdatedAt = DateTime.Now;
        });
        
        _context.UpdateRange(commonTags);
        _context.SaveChanges();
        
        var newTagAssocs = tagIds.Except(userTagAssocs).ToList();
        foreach (var tagId in newTagAssocs)
        {
            var newUserTagAssoc = new UserTagAssoc
            {
                UserId = req.UserId,
                TagId = tagId,
                LikeCount = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.UserTagAssocs.Add(newUserTagAssoc);
        }
        
        _context.SaveChanges();

        return new Response() { };
    }
}