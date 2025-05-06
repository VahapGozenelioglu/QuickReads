using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuickReads.Contexts;
using QuickReads.Entities;
using QuickReads.Entities.DTOs;

namespace QuickReads.Services;

public class LoginUser
{
    private static ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public class Response
    {
        public bool Succeed { get; set; }
        public string Token { get; set; }
    }

    public LoginUser(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    
    public Response Invoke(UserLoginDto req)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == req.Email);
        
        if (user == null)
        {
            return new Response { Succeed = false, Token = string.Empty };
        }
        
        bool isValid = BCrypt.Net.BCrypt.Verify(req.Password, user?.Password); 
        
        if (!isValid)
        {
            return new Response { Succeed = false, Token = string.Empty };
        }
        
        var token = GenerateJwtToken(user.Email);
        
        return new Response
        {
            Succeed = true,
            Token = token
        };
    }
    
    private string GenerateJwtToken(string email)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireTime"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
