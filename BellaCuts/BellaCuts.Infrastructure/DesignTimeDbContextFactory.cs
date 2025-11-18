using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using BellaCuts.Infrastructure.Data;

namespace BellaCuts.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BellaCutsDbContext>
{
    public BellaCutsDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection")
                               ?? config["ConnectionStrings:DefaultConnection"]
                               ?? "Server=localhost,1433;Database=BellaCutsDb;User=sa;Password=Your_strong_P@ssw0rd!;TrustServerCertificate=True";

        var builder = new DbContextOptionsBuilder<BellaCutsDbContext>();
        builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(BellaCutsDbContext).Assembly.FullName));

        return new BellaCutsDbContext(builder.Options);
    }
}