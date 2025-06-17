using System.Collections.Generic;
using System.ServiceModel;
using CarRental.Domain.Entities;
using CoreWCF;

namespace CarRental.SOAP.Services
{
    [ServiceContract]
    public interface ICarSoapService
    {
        [OperationContract]
        List<Car> GetAllCars();

        [OperationContract]
        Car GetCarById(int id);
    }
}
