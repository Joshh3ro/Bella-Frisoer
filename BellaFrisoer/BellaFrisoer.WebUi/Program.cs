using BellaFrisoer.WebUi.Components;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Register a factory so components can inject IDbContextFactory<BellaFrisoerDbContext>
builder.Services.AddDbContextFactory<BellaFrisoerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BellaFrisoerWebUiContext")
            ?? throw new InvalidOperationException("Connection string 'BellaFrisoerWebUiContext' not found."),
        sql => sql.MigrationsAssembly("BellaFrisoer.Infrastructure")
    ));

// optional: keep adapter + map DbContext to the concrete type for existing code
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<BellaFrisoerDbContext>());

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register repositories
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
