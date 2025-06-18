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
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repo;
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CustomerDtoSoap>> GetAllAsync()
        {
            var ents = await _repo.GetAllAsync();
            return _mapper.Map<List<CustomerDtoSoap>>(ents);
        }

        public async Task<CustomerDtoSoap?> GetByIdAsync(int id)
        {
            var ent = await _repo.GetByIdAsync(id);
            return ent == null ? null : _mapper.Map<CustomerDtoSoap>(ent);
        }

        public async Task<CustomerDtoSoap> CreateAsync(CustomerCreateDtoSoap dto)
        {
            var ent = _mapper.Map<Customer>(dto);
            await _repo.AddAsync(ent);
            return _mapper.Map<CustomerDtoSoap>(ent);
        }

        public async Task UpdateAsync(CustomerUpdateDtoSoap dto)
        {
            var ent = await _repo.GetByIdAsync(dto.Id);
            if (ent == null) throw new FaultException("Customer not found");
            _mapper.Map(dto, ent);
            await _repo.UpdateAsync(ent);
        }

        public async Task DeleteAsync(int id)
        {
            var ent = await _repo.GetByIdAsync(id);
            if (ent == null) throw new FaultException("Customer not found");
            await _repo.DeleteAsync(ent);
        }
    }
}
