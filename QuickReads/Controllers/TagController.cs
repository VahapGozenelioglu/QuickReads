using Microsoft.AspNetCore.Mvc;
using QuickReads.Contexts;
using QuickReads.Services;

namespace QuickReads.Controllers;



[ApiController]
[Route("Tag")]
public class TagController : ControllerBase
{
    private static ApplicationDbContext _context;
    public TagController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("CreateNewTag")]
    public IActionResult CreateNewTag([FromBody] CreateNewTag.Request req)
    {
        var svc = new CreateNewTag(_context);
        var resp = svc.Invoke(new CreateNewTag.Request() { TagName = req.TagName});
        return Ok(resp);
    }
    
    [HttpGet]
    [Route("GetAllTags")]
    public IActionResult GetAllTags()
    {
        var svc = new GetAllTags(_context);
        var resp = svc.Invoke(new GetAllTags.Request() { });
        return Ok(resp);
    }
}