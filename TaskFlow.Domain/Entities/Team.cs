namespace TaskFlow.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // A team has many users and many projects
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    
}