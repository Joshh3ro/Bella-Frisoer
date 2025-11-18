using BellaCuts.WebUi.Components;
using BellaCuts.Infrastructure;
using BellaCuts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register infrastructure (DbContext & infra services). Reads ConnectionStrings:DefaultConnection.
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Apply pending EF migrations at startup (safe in many scenarios; for production consider migration pipeline)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<BellaCutsDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log or handle the migration error as appropriate for your app
        var logger = services.GetService<ILoggerFactory>()?.CreateLogger("Migration");
        logger?.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
