using CarRental.Application.Interfaces;
using CarRental.Infrastructure.Repositories;
using CarRental.SOAP.Services;
using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja zależności
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarSoapService, CarSoapService>();

// Rejestracja usług SOAP
builder.Services.AddServiceModelServices();

var app = builder.Build();

((IApplicationBuilder)app).UseServiceModel(builder =>
{
    builder.AddService<CarSoapService>();
    builder.AddServiceEndpoint<CarSoapService, ICarSoapService>(
        new BasicHttpBinding(), "/CarSoapService");
});

app.Run();