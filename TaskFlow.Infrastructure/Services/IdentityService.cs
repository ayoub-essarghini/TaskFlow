using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Domain;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Infrastructure.Services;

using BCrypt.Net;
using Microsoft.Extensions.Configuration;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _config;

    public IdentityService(IConfiguration config) => _config = config;

    public string HashPassword(string password) => BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hashedPassword) =>
        BCrypt.Verify(password, hashedPassword);

    public string GenerateJwtToken(string username, Role role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
      
            new Claim(ClaimTypes.Role, role.ToString())
          
           
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}