using System.Collections.Generic;

namespace CarRental.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Brand { get; set; } = string.Empty;
        public ICollection<Rental>? Rentals { get; set; }
    }
}
