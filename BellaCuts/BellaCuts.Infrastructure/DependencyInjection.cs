using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BellaCuts.Infrastructure.Data;
using BellaCuts.Application.Services;
using BellaCuts.Infrastructure.Services;

namespace BellaCuts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? configuration["ConnectionStrings:DefaultConnection"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured. Set it in appsettings.json, environment variables, or Docker Compose.");
        }

        services.AddDbContext<BellaCutsDbContext>(options =>
            options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(BellaCutsDbContext).Assembly.FullName)));

        // Register application-facing implementations:
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}