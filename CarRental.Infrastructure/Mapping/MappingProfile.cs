using AutoMapper;
using CarRental.Application.DTOs;
using CarRental.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRental.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();

            // Rental
            CreateMap<Rental, RentalDto>().ReverseMap();
            CreateMap<RentalCreateDto, Rental>();
            CreateMap<RentalUpdateDto, Rental>();

            // Car (jeśli potrzebujesz DTO dla Car)
            // CreateMap<Car, CarDto>().ReverseMap();
        }
    }
}
