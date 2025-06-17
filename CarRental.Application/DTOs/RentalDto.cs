using System;

namespace CarRental.Application.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        // jeśli chcesz, możesz tu dodać dodatkowe pola,
        // np. CarModel, CustomerName, TotalPrice itp.
    }
}
