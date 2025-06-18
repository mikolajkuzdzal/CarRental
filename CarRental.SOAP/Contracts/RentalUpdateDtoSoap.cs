using System;
using System.Runtime.Serialization;

namespace CarRental.SOAP.Contracts
{
    [DataContract]
    public class RentalUpdateDtoSoap
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public DateTime? ReturnDate { get; set; }
    }
}
