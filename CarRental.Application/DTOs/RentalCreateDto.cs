using System;

namespace CarRental.Application.DTOs
{
    public class RentalCreateDto
    {
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        // ReturnDate pomijamy, bo klient nie podaje go przy tworzeniu
    }
}
