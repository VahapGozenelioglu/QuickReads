using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickReads.Contexts;
using QuickReads.Entities.DTOs;
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
    [Authorize]
    [Route("GetAllArticles")]
    public IActionResult GetAllArticles()
    {
        var svc = new GetAllArticles(_context);
        var resp = svc.Invoke(new GetAllArticles.Request() {});
        return Ok(resp);
    }
    
    [HttpGet]
    [Authorize]
    [Route("GetRandomArticles")]
    public IActionResult GetRandomArticles(int? articleCount)
    {
        var svc = new GetRandomArticles(_context);
        var resp = svc.Invoke(new GetRandomArticles.Request() {ArticleCount = articleCount});
        return Ok(resp);
    }
    
    [HttpGet]
    [Authorize]
    [Route("GetArticleById/{id}")]
    public IActionResult GetArticleById(int id)
    {
        var svc = new GetArticleById(_context);
        var resp = svc.Invoke(new GetArticleById.Request() { Id = id });
        return Ok(resp);
    }
    
    [HttpGet]
    [Authorize]
    [Route("GetAllArticlesByTag")]
    public IActionResult GetAllArticlesByTag(string tagName)
    {
        var svc = new GetAllArticlesByTag(_context);
        var resp = svc.Invoke(new GetAllArticlesByTag.Request() { TagName = tagName });
        return Ok(resp);
    }
    
    [HttpGet]
    [Authorize]
    [Route("GetAllArticlesByLanguage")]
    public IActionResult GetAllArticlesByLanguage(LanguageEnum language)
    {
        var svc = new GetAllArticlesByLanguage(_context);
        var resp = svc.Invoke(new GetAllArticlesByLanguage.Request() { Language = language });
        return Ok(resp);
    }
    
    [HttpPost]
    [Authorize]
    [Route("CreateNewArticle")]
    public IActionResult CreateNewArticle([FromBody] ArticleCreateDto req)
    {
        var svc = new CreateNewArticle(_context);
        var resp = svc.Invoke(req);
        return Ok(resp);
    }
    
    [HttpPost]
    [Authorize]
    [Route("LikeArticle")]
    public IActionResult LikeArticle([FromBody] ArticleUserDto req)
    {
        var svc = new LikeArticle(_context);
        var resp = svc.Invoke(req);
        return Ok(resp);
    }
    
    [HttpPost]
    [Authorize]
    [Route("UndoLikeArticle")]
    public IActionResult UndoLikeArticle([FromBody] ArticleUserDto req)
    {
        var svc = new UndoLikeArticle(_context);
        var resp = svc.Invoke(req);
        return Ok(resp);
    }
    
    [HttpGet]
    [Authorize]
    [Route("SuggestRandomArticlesForUser")]
    public IActionResult SuggestRandomArticlesForUser(int userId)
    {
        var svc = new SuggestRandomArticlesForUser(_context);
        var resp = svc.Invoke(new SuggestRandomArticlesForUser.Request(){UserId = userId});
        return Ok(resp);
    }
    
    [HttpPost]
    [Authorize]
    [Route("AddArticleToBookmarks")]
    public IActionResult AddArticleToBookmarks([FromBody] ArticleUserDto req)
    {
        var svc = new AddArticleToBookmarks(_context);
        var resp = svc.Invoke(req);
        return Ok(resp);
    }
    
    [HttpPost]
    [Authorize]
    [Route("RemoveArticleFromBookmarks")]
    public IActionResult RemoveArticleFromBookmarks([FromBody] ArticleUserDto req)
    {
        var svc = new RemoveArticleFromBookmarks(_context);
        var resp = svc.Invoke(req);
        return Ok(resp);
    }
}