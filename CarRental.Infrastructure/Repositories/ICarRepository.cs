using CarRental.Domain.Entities;
using System.Collections.Generic;

namespace CarRental.Application.Interfaces
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        Car? Get(int id); 
        void Add(Car car);
        void Update(Car car);
        void Delete(int id);
    }
}
