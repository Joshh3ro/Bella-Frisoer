using BellaFrisoer.WebUi.Components;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using BellaFrisoer.Application.Services;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Infrastructure.Repositories;



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
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();


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
app.MapGet("/invoice/{id}", async (int id, IBookingService bookingService) =>
{
    var booking = await bookingService.GetByIdAsync(id);
    if (booking is null)
        return Results.NotFound();

    string txt = $"""
        Faktura for booking #{booking.Id}

        Kunde: {booking.Customer?.FirstName}
        Ansatte: {booking.Employee?.FirstName}
        Dato: {booking.BookingDateTime}
        Varighed: {booking.BookingDuration} minutter
        Behandling: {booking.Treatment?.Name}
        Pris: {booking.TotalPrice}
        {DateTime.Now}
        """;

    return Results.File(
        System.Text.Encoding.UTF8.GetBytes(txt),
        "text/plain",
        $"invoice_{booking.Id}.txt"
    );
});


app.Run();