namespace CarRental.Domain.Entities;

public class Rental
{
    public Guid Id { get; set; }

    public Guid CarId { get; set; }
    public Car Car { get; set; } = null!;

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalCost { get; set; }
}
