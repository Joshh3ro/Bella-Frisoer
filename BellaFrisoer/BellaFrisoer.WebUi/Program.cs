using BellaFrisoer.WebUi.Components;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Infrastructure.Repositories;
using BellaFrisoer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using BellaFrisoer.Application.Services;


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

// Register repositories and application services by interface
// Use Scoped lifetime for request/component-scoped services in Blazor Server
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingConflictChecker, BookingConflictChecker>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();

// DO NOT register domain entity types as DI services
// e.g. don't do: builder.Services.AddScoped<ICustomer, Customer>();

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