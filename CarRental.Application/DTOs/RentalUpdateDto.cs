using System;

namespace CarRental.Application.DTOs
{
    public class RentalUpdateDto
    {
        public int Id { get; set; }               // wskazuje, które wypożyczenie aktualizujesz
        public DateTime? ReturnDate { get; set; } // np. data zwrotu
        // dodaj tu inne pola, które w Twojej domenie mogą być edytowane
    }
}
