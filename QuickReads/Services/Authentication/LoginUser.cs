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

    public class AuthData
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
        public UserLoginDto? User { get; set; }
    }

    public LoginUser(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    
    public AuthData Invoke(UserLoginDto req)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == req.Email);
        
        if (user == null)
        {
            return new AuthData()
            {
                Success = false,
                Token = string.Empty,
                Expiration = string.Empty,
                User = null
            };
        }
        
        bool isValid = BCrypt.Net.BCrypt.Verify(req.Password, user?.Password); 
        
        if (!isValid)
        {
            return new AuthData()
            {
                Success = false,
                Token = string.Empty,
                Expiration = string.Empty,
                User = null
            };
        }
        
        var token = GenerateJwtToken(user.Email);
        
        
        var jwtSettings = _config.GetSection("JwtSettings");
        
        return new AuthData()
        {
            Success = true,
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireTime"] ?? string.Empty)).ToString(),
            User = new UserLoginDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = "Password is not returned for security reasons"
            }
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
