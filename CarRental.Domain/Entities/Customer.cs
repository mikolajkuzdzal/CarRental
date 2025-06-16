namespace CarRental.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
