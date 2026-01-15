using BellaFrisoer.WebUi.Components;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Application.Services;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using BellaFrisoer.Application.Queries;
using BellaFrisoer.Infrastructure.Queries;
using BellaFrisoer.Domain.Services;
using BellaFrisoer.Domain.Queries;
using BellaFrisoer.Domain.Models.Discounts;
using System;

var builder = WebApplication.CreateBuilder(args);

// AddDbContext er scoped som default
builder.Services.AddDbContext<BellaFrisoerWebUiContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BellaFrisoerWebUiContext")
            ?? throw new InvalidOperationException("Connection string not found."),
        sql => sql.MigrationsAssembly("BellaFrisoer.Infrastructure")
    ));

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingQuery, BookingQuery>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingPriceService, BookingPriceService>();

builder.Services.AddScoped<IBookingConflictChecker, BookingConflictChecker>();
builder.Services.AddScoped<IBookingConflictQuery, BookingConflictQuery>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IDiscountCalculator, LoyaltyDiscountStrategy>();


builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGet("/invoice/{id}", async (int id, IInvoiceService invoiceService) =>
{
    try
    {
        string invoiceText = await invoiceService.GenerateInvoiceAsync(id);
        return Results.File(
            System.Text.Encoding.UTF8.GetBytes(invoiceText),
            "text/plain",
            $"invoice_{id}.txt"
        );
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});


app.Run();
