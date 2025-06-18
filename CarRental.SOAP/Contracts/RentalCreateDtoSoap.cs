using System;
using System.Runtime.Serialization;

namespace CarRental.SOAP.Contracts
{
    [DataContract]
    public class RentalCreateDtoSoap
    {
        [DataMember] public int CarId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public DateTime RentalDate { get; set; }
    }
}
