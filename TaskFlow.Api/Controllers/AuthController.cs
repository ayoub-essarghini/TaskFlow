using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ApplicationDbContext _context;
    
    public AuthController(IIdentityService identityService, ApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Username already exists.");
        
        if (!Enum.TryParse<Role>(dto.Role, out var parsedRole)) return BadRequest("Invalid role.");
       
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Role = parsedRole,
            Password = _identityService.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            
        if (user == null || !_identityService.VerifyPassword(dto.Password, user.Password))
            return Unauthorized("Invalid username or password.");

        var token = _identityService.GenerateJwtToken(user.Username, user.Role);

        return Ok(new AuthResponseDto(token, user.Username, user.Role));
    }

}