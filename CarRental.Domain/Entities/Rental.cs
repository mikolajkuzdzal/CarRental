namespace CarRental.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }

        // najpierw klucze obce
        public int CarId { get; set; }
        public int CustomerId { get; set; }

        // właściwości nawigacyjne
        public Car Car { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // inne pola...
    }
}
