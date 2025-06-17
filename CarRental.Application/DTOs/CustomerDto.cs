namespace CarRental.Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;  // dopasuj do pola w encji Customer
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        // inne pola, które chcesz zwracać w odpowiedzi
    }
}
