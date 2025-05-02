using QuickReads.Contexts;
using QuickReads.Entities;

namespace QuickReads.Services;

public class BulkImportTags
{
    private static ApplicationDbContext _context;

    public class Request
    {
        public List<string> TagNames { get; set; }
    }

    public class Response
    {
    }

    public BulkImportTags(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Response Invoke(Request req)
    {
        var existingTags = _context.Tags
            .Where(x => req.TagNames.Contains(x.Name))
            .Select(x => x.Name)
            .ToList();
        
        var newTags = req.TagNames
            .Where(x => !existingTags.Contains(x))
            .ToList();
            
        var tags = newTags.Select(tagName => new Tag()
        {
            Name = tagName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();
        
        _context.Tags.AddRange(tags);
        _context.SaveChanges();
        
        return new Response() { };
    }
}
