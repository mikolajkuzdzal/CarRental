namespace CarRental.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}