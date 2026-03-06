using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<User> Users { get; set; }
  
 
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);
       
       // Team -> Projects (One-to-Many)
       modelBuilder.Entity<Team>()
           .HasMany(t => t.Projects)
           .WithOne(p => p.Team)
           .HasForeignKey(p => p.TeamId);

       // Project -> Tasks (One-to-Many)
       modelBuilder.Entity<Project>()
           .HasMany(p => p.Tasks)
           .WithOne(t => t.Project)
           .HasForeignKey(t => t.ProjectId);

       // Team -> Users (One-to-Many)
       modelBuilder.Entity<Team>()
           .HasMany(t => t.Members)
           .WithOne(u => u.Team)
           .HasForeignKey(u => u.TeamId);
       
       // Configure User.Role enum to be stored as string in database
       modelBuilder.Entity<User>()
           .Property(u => u.Role)
           .HasConversion<string>();
      
    }
}