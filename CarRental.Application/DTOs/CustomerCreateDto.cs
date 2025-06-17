namespace CarRental.Application.DTOs
{
    public class CustomerCreateDto
    {
        // brak Id — przy tworzeniu jeszcze go nie znasz
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        // inne pola niezbędne przy tworzeniu
    }
}
