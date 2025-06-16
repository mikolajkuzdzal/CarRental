using Microsoft.AspNetCore.Mvc;
using CarRental.Domain.Entities;

namespace CarRental.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private static readonly List<Rental> Rentals = new();
    private static readonly List<Customer> Customers = new(); // temp
    private static readonly List<Car> Cars = new(); // temp

    [HttpGet]
    public ActionResult<IEnumerable<Rental>> GetAll()
    {
        return Ok(Rentals);
    }

    [HttpGet("{id}")]
    public ActionResult<Rental> Get(int id)
    {
        var rental = Rentals.FirstOrDefault(r => r.Id == id);
        if (rental is null) return NotFound();
        return Ok(rental);
    }

    [HttpPost]
    public ActionResult<Rental> Create(Rental rental)
    {
        var customerExists = Customers.Any(c => c.Id == rental.CustomerId);
        var carExists = Cars.Any(c => c.Id == rental.CarId);
        if (!customerExists || !carExists)
        {
            return BadRequest("Invalid CustomerId or CarId.");
        }

        rental.Id = Rentals.Count + 1;
        rental.RentalDate = DateTime.UtcNow;
        Rentals.Add(rental);
        return CreatedAtAction(nameof(Get), new { id = rental.Id }, rental);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var rental = Rentals.FirstOrDefault(r => r.Id == id);
        if (rental is null) return NotFound();

        Rentals.Remove(rental);
        return NoContent();
    }
}
