using System;
using System.Runtime.Serialization;

namespace CarRental.SOAP.Contracts
{
    [DataContract]
    public class RentalDtoSoap
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CarId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public DateTime RentalDate { get; set; }
        [DataMember] public DateTime? ReturnDate { get; set; }
    }
}
