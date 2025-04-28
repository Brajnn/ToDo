using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Domain.Interafaces;
using ToDo.Infrastructure.Presistence;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure the DbContext to use MySQL with Pomelo provider
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

        // Register the application's DbContext
        services.AddDbContext<ToDoDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));

        // Register the Todo repository
        services.AddScoped<ITodoRepository, TodoRepository>();
    }
}
