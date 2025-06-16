namespace CarRental.Domain.Entities;

public class Car
{
    public int Id { get; set; }
    public string Model { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public int Year { get; set; }
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
