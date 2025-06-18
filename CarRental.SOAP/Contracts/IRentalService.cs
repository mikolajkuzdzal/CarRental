using CoreWCF;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.SOAP.Contracts
{
    [ServiceContract]
    public interface IRentalService
    {
        [OperationContract] Task<List<RentalDtoSoap>> GetAllAsync();
        [OperationContract] Task<RentalDtoSoap?> GetByIdAsync(int id);
        [OperationContract] Task<RentalDtoSoap> CreateAsync(RentalCreateDtoSoap dto);
        [OperationContract] Task UpdateAsync(RentalUpdateDtoSoap dto);
        [OperationContract] Task DeleteAsync(int id);
    }
}
