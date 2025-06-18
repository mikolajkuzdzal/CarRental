using System;
using AutoMapper;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Mapping;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using CarRental.SOAP.Contracts;
using CarRental.SOAP.Services;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext — tutaj InMemory, ale możesz wskazać SQL Server
builder.Services.AddDbContext<CarRentalDbContext>(opts =>
    opts.UseInMemoryDatabase("CarRentalDb"));

// 2) Repozytoria i AutoMapper
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 3) CoreWCF – usługi SOAP
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();

// 4) Wstrzykiwanie implementacji serwisów
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRentalService, RentalService>();

var app = builder.Build();

// 5) Routing + SOAP endpoints
app.UseRouting();

app.UseServiceModel(svc =>
{
    svc.AddService<CustomerService>();
    svc.AddServiceEndpoint<CustomerService, ICustomerService>(
        new BasicHttpBinding(), "/CustomerService.svc");

    svc.AddService<RentalService>();
    svc.AddServiceEndpoint<RentalService, IRentalService>(
        new BasicHttpBinding(), "/RentalService.svc");

    // WSDL pod końcówkami ?wsdl
    var smb = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    smb.HttpGetEnabled = true;
});

app.Run();
