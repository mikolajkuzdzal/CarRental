using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.SOAP.Contracts;
using CoreWCF;

namespace CarRental.SOAP.Services
{
    [ServiceBehavior]
    public class RentalService : IRentalService
    {
        private readonly IRepository<Rental> _rentalRepo;
        private readonly IRepository<Car> _carRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public RentalService(
            IRepository<Rental> rentalRepo,
            IRepository<Car> carRepo,
            IRepository<Customer> customerRepo,
            IMapper mapper)
        {
            _rentalRepo = rentalRepo;
            _carRepo = carRepo;
            _customerRepo = customerRepo;
            _mapper = mapper;
        }

        public async Task<List<RentalDtoSoap>> GetAllAsync()
        {
            var ents = await _rentalRepo.GetAllAsync();
            return _mapper.Map<List<RentalDtoSoap>>(ents);
        }

        public async Task<RentalDtoSoap?> GetByIdAsync(int id)
        {
            var ent = await _rentalRepo.GetByIdAsync(id);
            return ent == null ? null : _mapper.Map<RentalDtoSoap>(ent);
        }

        public async Task<RentalDtoSoap> CreateAsync(RentalCreateDtoSoap dto)
        {
            // walidacja istnienia Car i Customer
            var car = await _carRepo.GetByIdAsync(dto.CarId);
            var cust = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (car == null || cust == null)
                throw new FaultException("Invalid CarId or CustomerId");

            var ent = _mapper.Map<Rental>(dto);
            await _rentalRepo.AddAsync(ent);
            return _mapper.Map<RentalDtoSoap>(ent);
        }

        public async Task UpdateAsync(RentalUpdateDtoSoap dto)
        {
            var ent = await _rentalRepo.GetByIdAsync(dto.Id);
            if (ent == null) throw new FaultException("Rental not found");
            _mapper.Map(dto, ent);
            await _rentalRepo.UpdateAsync(ent);
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _rentalRepo.GetByIdAsync(id);
            if (ent == null) throw new FaultException("Rental not found");
            await _rentalRepo.DeleteAsync(ent);
        }
    }
}
