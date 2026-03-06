namespace TaskFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // A project belongs to a team
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }

    // A project has many tasks
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}