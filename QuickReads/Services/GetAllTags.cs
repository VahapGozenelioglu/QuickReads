using Microsoft.EntityFrameworkCore;
using QuickReads.Contexts;
using QuickReads.Entities.DTOs;
using QuickReads.Extensions;

namespace QuickReads.Services;

public class GetAllTags
{
    private static ApplicationDbContext _context;
    public class Request { }

    public class Response
    {
        public List<TagDto> Tags { get; set; }
    }

    public GetAllTags(ApplicationDbContext context)
    {
        _context = context;
    }
    public Response Invoke(Request req)
    {
        var tags = _context.Tags
            .Select(x => x.ToDto())
            .ToList();
        
        return new Response() { Tags = tags };
    }
}