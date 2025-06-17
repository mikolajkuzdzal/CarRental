using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Infrastructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly List<Car> _cars = new();

        public IEnumerable<Car> GetAll() => _cars;

        public Car? Get(int id) => _cars.FirstOrDefault(c => c.Id == id); // 👈 DODAJ TO

        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Update(Car updatedCar)
        {
            var existing = Get(updatedCar.Id);
            if (existing is not null)
            {
                existing.Make = updatedCar.Make;
                existing.Model = updatedCar.Model;
                existing.Year = updatedCar.Year;
                existing.Brand = updatedCar.Brand;
            }
        }

        public void Delete(int id)
        {
            var car = Get(id);
            if (car is not null)
            {
                _cars.Remove(car);
            }
        }
    }
}
