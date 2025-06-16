using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CarRental.Infrastructure.Persistence;
using CarRental.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Dodaj DbContext EF Core (InMemory)
builder.Services.AddDbContext<CarRentalDbContext>(options =>
    options.UseInMemoryDatabase("CarRentalDb"));

// Rejestracja generycznego repozytorium
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// Dodaj kontrolery
builder.Services.AddControllers();

// Dodaj Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarRental API", Version = "v1" });
});

var app = builder.Build();

// Middleware developerski i Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapowanie kontrolerów
app.MapControllers();

app.Run();
