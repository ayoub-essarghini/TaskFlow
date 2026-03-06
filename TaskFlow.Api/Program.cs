
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Repositories;
using TaskFlow.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// postgreSql Connection
var connectionString = builder.Configuration.GetConnectionString("TasksDb");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

//Register Dependency Injection
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();


var app = builder.Build();


// This creates a temporary scope to grab our database context and run migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // This command checks for pending migrations and applies them automatically!
        context.Database.Migrate(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   // Add this!
    app.UseSwaggerUI(); // Add this!
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();



app.Run();

