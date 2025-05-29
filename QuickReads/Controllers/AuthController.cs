using Microsoft.AspNetCore.Mvc;
using QuickReads.Contexts;
using QuickReads.Entities.DTOs;
using QuickReads.Services;

namespace QuickReads.Controllers;

[ApiController]
[Route("Auth")]
public class AuthController : ControllerBase
{
    private static ApplicationDbContext _context;
    private readonly IConfiguration _config;
    
    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    
    [HttpPost("signup")]
    public IActionResult Register([FromBody] UserSignUpDto newUser)
    {
        var resp = new SignUpUser(_context, _config).Invoke(newUser);

        if (resp.Success)
        {
            return Ok(resp);
        }
        
        return Unauthorized("Kullanıcı zaten kayıtlı!");
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto loginUser)
    {
        var resp = new LoginUser(_context, _config).Invoke(loginUser);

        if (resp.Success)
        {
            return Ok(resp);
        }
        
        return Unauthorized("Geçersiz e-posta veya şifre");
    }
}