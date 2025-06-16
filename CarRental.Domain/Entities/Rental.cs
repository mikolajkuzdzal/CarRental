namespace CarRental.Domain.Entities;

public class Rental
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    public DateTime RentalDate { get; set; }

    public Customer? Customer { get; set; }
    public Car? Car { get; set; }
}
