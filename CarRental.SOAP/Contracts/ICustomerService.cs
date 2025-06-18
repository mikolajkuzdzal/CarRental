using CoreWCF;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.SOAP.Contracts
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract] Task<List<CustomerDtoSoap>> GetAllAsync();
        [OperationContract] Task<CustomerDtoSoap?> GetByIdAsync(int id);
        [OperationContract] Task<CustomerDtoSoap> CreateAsync(CustomerCreateDtoSoap dto);
        [OperationContract] Task UpdateAsync(CustomerUpdateDtoSoap dto);
        [OperationContract] Task DeleteAsync(int id);
    }
}
