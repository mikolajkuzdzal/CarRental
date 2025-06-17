using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;

namespace CarRental.Web.Controllers
{
    [Authorize] // 🔐 Wymaga uwierzytelnienia JWT
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;

        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cars = _carRepository.GetAll();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var car = _carRepository.Get(id);
            if (car == null)
                return NotFound();
            return Ok(car);
        }

        [Authorize(Roles = "Admin")] // Tylko Admin może tworzyć
        [HttpPost]
        public IActionResult Create([FromBody] Car car)
        {
            _carRepository.Add(car);
            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }

        [Authorize(Roles = "Admin")] // Tylko Admin może edytować
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Car updatedCar)
        {
            var existingCar = _carRepository.Get(id);
            if (existingCar == null)
                return NotFound();

            existingCar.Make = updatedCar.Make;
            existingCar.Model = updatedCar.Model;
            existingCar.Year = updatedCar.Year;
            existingCar.Brand = updatedCar.Brand;

            _carRepository.Update(existingCar);
            return NoContent();
        }

        [Authorize(Roles = "Admin")] // Tylko Admin może usuwać
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCar = _carRepository.Get(id);
            if (existingCar == null)
                return NotFound();

            _carRepository.Delete(id);
            return NoContent();
        }
    }
}
