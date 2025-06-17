using AutoMapper;                                  // dla IMapper
using CarRental.Application.DTOs;                  // Twoje DTO
using CarRental.Application.Interfaces;            // IRepository<T>
using CarRental.Domain.Entities;                   // Customer, Rental
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarRental.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public CustomerController(IRepository<Customer> customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }

        // GET: api/Customer
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var ents = await _customerRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(ents));
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            var ent = await _customerRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();
            return Ok(_mapper.Map<CustomerDto>(ent));
        }

        // POST: api/Customer
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CustomerCreateDto dto)
        {
            var ent = _mapper.Map<Customer>(dto);
            await _customerRepo.AddAsync(ent);
            var resultDto = _mapper.Map<CustomerDto>(ent);
            return CreatedAtAction(nameof(Get), new { id = ent.Id }, resultDto);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDto dto)
        {
            var ent = await _customerRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();

            _mapper.Map(dto, ent);
            await _customerRepo.UpdateAsync(ent);
            return NoContent();
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ent = await _customerRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();

            await _customerRepo.DeleteAsync(ent);
            return NoContent();
        }
    }
}
