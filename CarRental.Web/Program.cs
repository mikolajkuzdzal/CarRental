using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarRental.Application.Interfaces;
using CarRental.Infrastructure.Repositories;
using CarRental.Infrastructure.Mapping;

var builder = WebApplication.CreateBuilder(args);

// repozytoria
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Twoja konfiguracja tokenów
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
