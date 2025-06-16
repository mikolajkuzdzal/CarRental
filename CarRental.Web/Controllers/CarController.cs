using Microsoft.AspNetCore.Mvc;
using CarRental.Domain.Entities;

namespace CarRental.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private static readonly List<Car> Cars = new();

    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetAll()
    {
        return Ok(Cars);
    }

    [HttpGet("{id}")]
    public ActionResult<Car> Get(int id)
    {
        var car = Cars.FirstOrDefault(c => c.Id == id);
        if (car is null) return NotFound();
        return Ok(car);
    }

    [HttpPost]
    public ActionResult<Car> Create(Car car)
    {
        car.Id = Cars.Count + 1;
        Cars.Add(car);
        return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Car car)
    {
        var existing = Cars.FirstOrDefault(c => c.Id == id);
        if (existing is null) return NotFound();

        existing.Brand = car.Brand;
        existing.Model = car.Model;
        existing.Year = car.Year;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var car = Cars.FirstOrDefault(c => c.Id == id);
        if (car is null) return NotFound();

        Cars.Remove(car);
        return NoContent();
    }
}
