using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class User
{
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; }  = string.Empty;
    public string Email { get; set; }  = string.Empty;
    public UserRole Role { get; set; } = UserRole.Member;
    
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
    
    
}