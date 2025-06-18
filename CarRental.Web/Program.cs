using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarRental.Application.Interfaces;
using CarRental.Infrastructure.Repositories;
using CarRental.Infrastructure.Mapping;

var builder = WebApplication.CreateBuilder(args);

// 1. Repozytoria
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// 2. AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 3. JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://your-auth-server/";         // lub inne źródło tokenów
        options.Audience = "CarRentalApi";
        options.RequireHttpsMetadata = true;
        // możesz dodać tu ValidateIssuer, ValidateLifetime, itp.
    });
builder.Services.AddAuthorization();

// 4. Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CarRental API", Version = "v1" });
    // Dodaj obsługę JWT w Swagger UI:
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Wpisz: Bearer {token}"
    });
    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// 5. MVC / API Controllers
builder.Services.AddControllers();

var app = builder.Build();

// 6. Middleware pipeline

// wymuszamy HTTPS
app.UseHttpsRedirection();

// tylko w Development włączamy Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarRental API V1");
        // c.RoutePrefix = string.Empty; // odkomentuj, jeśli chcesz otwierać UI pod /
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
