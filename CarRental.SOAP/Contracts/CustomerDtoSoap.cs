using System.Runtime.Serialization;

namespace CarRental.SOAP.Contracts
{
    [DataContract]
    public class CustomerDtoSoap
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string FirstName { get; set; } = string.Empty;
        [DataMember] public string LastName { get; set; } = string.Empty;
        [DataMember] public string Email { get; set; } = string.Empty;
    }
}
