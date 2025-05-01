using QuickReads.Contexts;
using QuickReads.Entities;

namespace QuickReads.Services;

public class CreateNewTag
{
    private static ApplicationDbContext _context;

    public class Request
    {
        public string TagName { get; set; }
    }

    public class Response
    {
    }

    public CreateNewTag(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(Request req)
    {
        var tag = new Tag()
        {
            Name = req.TagName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Tags.Add(tag);
        _context.SaveChanges();
        
        return new Response() { };
    }
}