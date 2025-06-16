using Microsoft.AspNetCore.Mvc;
using CarRental.Domain.Entities;

namespace CarRental.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private static readonly List<Customer> Customers = new();

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetAll()
    {
        return Ok(Customers);
    }

    [HttpGet("{id}")]
    public ActionResult<Customer> Get(int id)
    {
        var customer = Customers.FirstOrDefault(c => c.Id == id);
        if (customer is null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public ActionResult<Customer> Create(Customer customer)
    {
        customer.Id = Customers.Count + 1;
        Customers.Add(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Customer customer)
    {
        var existing = Customers.FirstOrDefault(c => c.Id == id);
        if (existing is null) return NotFound();

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var customer = Customers.FirstOrDefault(c => c.Id == id);
        if (customer is null) return NotFound();

        Customers.Remove(customer);
        return NoContent();
    }
}
