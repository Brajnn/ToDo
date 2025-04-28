using ToDo.Infrastructure.Extensions;
using ToDo.Application.Extensions;
using ToDo.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;
namespace ToDo.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // Register infrastructure services
        builder.Services.AddInfrastructure(builder.Configuration);
        // Register application layer services
        builder.Services.AddApplicationServices();
        var app = builder.Build();

        // Automatically apply database migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
