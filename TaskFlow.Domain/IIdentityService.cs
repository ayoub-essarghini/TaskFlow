using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain;

public interface IIdentityService
{
    string GenerateJwtToken(string username, Role role);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}