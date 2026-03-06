using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public record RegisterDto(string Username, string Email, string Password, string Role =  "Member");
public record LoginDto(string Username, string Password);
public record AuthResponseDto(string Token, string Username, Role Role);