using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Application.DTOs;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var dtos = _mapper.Map<IEnumerable<CustomerDto>>(ents);
            return Ok(dtos);
        }

        // GET: api/Customer/5
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            var ent = await _customerRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = _mapper.Map<CustomerDto>(ent);
            return Ok(dto);
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
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var ent = await _customerRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();

            _mapper.Map(dto, ent);
            await _customerRepo.UpdateAsync(ent);
            return NoContent();
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id:int}")]
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
