using Microsoft.AspNetCore.Mvc;
using QuickReads.Contexts;
using QuickReads.Entities.Enums;
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
    
    [HttpGet]
    [Route("GetAllArticlesByTag")]
    public IActionResult GetAllArticlesByTag(string tagName)
    {
        var svc = new GetAllArticlesByTag(_context);
        var resp = svc.Invoke(new GetAllArticlesByTag.Request() { TagName = tagName });
        return Ok(resp);
    }
    
    [HttpGet]
    [Route("GetAllArticlesByLanguage")]
    public IActionResult GetAllArticlesByLanguage(LanguageEnum language)
    {
        var svc = new GetAllArticlesByLanguage(_context);
        var resp = svc.Invoke(new GetAllArticlesByLanguage.Request() { Language = language });
        return Ok(resp);
    }
    
    [HttpPost]
    [Route("CreateNewArticle")]
    public IActionResult CreateNewArticle([FromBody] ArticleDto req)
    {
        var svc = new CreateNewArticle(_context);
        var resp = svc.Invoke(req);
        return Ok(resp);
    }
}