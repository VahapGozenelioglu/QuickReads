using Microsoft.AspNetCore.Mvc;
using QuickReads.Contexts;
using QuickReads.Services;

namespace QuickReads.Controllers;

[ApiController]
[Route("Article")]
public class ArticleController : ControllerBase
{
    private static ApplicationDbContext _context;
    public ArticleController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("GetAllArticles")]
    public IActionResult GetAllArticles()
    {
        var svc = new GetAllArticles(_context);
        var resp = svc.Invoke(new GetAllArticles.Request() {});
        return Ok(resp);
    }
    
    [HttpGet]
    [Route("GetArticleById/{id}")]
    public IActionResult GetArticleById(int id)
    {
        var svc = new GetArticleById(_context);
        var resp = svc.Invoke(new GetArticleById.Request() { Id = id });
        return Ok(resp);
    }
}