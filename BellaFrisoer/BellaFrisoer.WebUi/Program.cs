using BellaFrisoer.WebUi.Components;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Infrastructure.Repositories;
using BellaFrisoer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Application;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<BellaFrisoerWebUiContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BellaFrisoerWebUiContext")
            ?? throw new InvalidOperationException("Connection string not found."),
        sql => sql.MigrationsAssembly("BellaFrisoer.Infrastructure") // important when migrations live in Infrastructure
    ));

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IBookingRepository, BookingRepository>();
builder.Services.AddSingleton<IBookingConflictChecker, BookingConflictChecker>();
builder.Services.AddSingleton<ICustomer>();
builder.Services.AddSingleton<IEmployee>();




var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();