using System.Collections.Generic;
using System.Linq;
using CarRental.Domain.Entities;
using CarRental.Application.Interfaces;
using System.ServiceModel;
using CarRental.Infrastructure.Repositories;

namespace CarRental.SOAP.Services
{
    public class CarSoapService : ICarSoapService
    {
        private readonly ICarRepository _carRepository;

        public CarSoapService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public List<Car> GetAllCars()
        {
            return _carRepository.GetAll().ToList();
        }

        public Car GetCarById(int id)
        {
            return _carRepository.Get(id);
        }
    }
}
