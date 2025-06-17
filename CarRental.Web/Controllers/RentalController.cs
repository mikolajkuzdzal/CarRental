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
    public class RentalController : ControllerBase
    {
        private readonly IRepository<Rental> _rentalRepo;
        private readonly IRepository<Car> _carRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public RentalController(
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

        // GET: api/Rental
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetAll()
        {
            var ents = await _rentalRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<RentalDto>>(ents);
            return Ok(dtos);
        }

        // GET: api/Rental/5
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<RentalDto>> Get(int id)
        {
            var ent = await _rentalRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();
            var dto = _mapper.Map<RentalDto>(ent);
            return Ok(dto);
        }

        // POST: api/Rental
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RentalDto>> Create([FromBody] RentalCreateDto dto)
        {
            // ensure foreign keys exist
            var car = await _carRepo.GetByIdAsync(dto.CarId);
            var cust = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (car == null || cust == null)
                return BadRequest("Invalid CarId or CustomerId");

            var ent = _mapper.Map<Rental>(dto);
            await _rentalRepo.AddAsync(ent);

            var resultDto = _mapper.Map<RentalDto>(ent);
            return CreatedAtAction(nameof(Get), new { id = ent.Id }, resultDto);
        }

        // PUT: api/Rental/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] RentalUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var ent = await _rentalRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();

            _mapper.Map(dto, ent);
            await _rentalRepo.UpdateAsync(ent);
            return NoContent();
        }

        // DELETE: api/Rental/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ent = await _rentalRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();

            await _rentalRepo.DeleteAsync(ent);
            return NoContent();
        }
    }
}
