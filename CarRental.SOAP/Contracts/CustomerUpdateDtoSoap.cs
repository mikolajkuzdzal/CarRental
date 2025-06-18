using System.Runtime.Serialization;

namespace CarRental.SOAP.Contracts
{
    [DataContract]
    public class CustomerUpdateDtoSoap
    {
        [DataMember]
        public int Id { get; set; }              // <— to musi być

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;
    }
}
