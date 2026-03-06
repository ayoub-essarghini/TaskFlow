using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<TaskItem> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);
       
       modelBuilder.Entity<TaskItem>().ToTable("tasks");
       modelBuilder.Entity<TaskItem>().HasKey(x => x.Id);
       modelBuilder.Entity<TaskItem>().Property(t => t.Title).IsRequired().HasMaxLength(200);
    }
}