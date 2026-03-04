using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public TaksStatus Status { get; set; } = TaksStatus.Todo;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}