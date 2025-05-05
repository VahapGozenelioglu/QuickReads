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
        public List<Tag> Tags { get; set; }
    }

    public BulkImportTags(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Response Invoke(Request req)
    {
        var existingTagEntities = _context.Tags
            .Where(x => req.TagNames.Contains(x.Name))
            .ToList();

        var existingTagNames = existingTagEntities.Select(x => x.Name).ToHashSet();

        var newTags = req.TagNames
            .Where(x => !existingTagNames.Contains(x))
            .Select(tagName => new Tag
            {
                Name = tagName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

        _context.Tags.AddRange(newTags);
        _context.SaveChanges();

        var allTags = existingTagEntities.Concat(newTags).ToList();

        return new Response { Tags = allTags };
    }

}
