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
            return Ok(_mapper.Map<IEnumerable<RentalDto>>(ents));
        }

        // GET: api/Rental/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RentalDto>> Get(int id)
        {
            var ent = await _rentalRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();
            return Ok(_mapper.Map<RentalDto>(ent));
        }

        // POST: api/Rental
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<RentalDto>> Create([FromBody] RentalCreateDto dto)
        {
            // sprawdź istnienie Car i Customer
            if (await _carRepo.GetByIdAsync(dto.CarId) == null ||
                await _customerRepo.GetByIdAsync(dto.CustomerId) == null)
            {
                return BadRequest("Invalid CarId or CustomerId");
            }

            var ent = _mapper.Map<Rental>(dto);
            await _rentalRepo.AddAsync(ent);
            var resultDto = _mapper.Map<RentalDto>(ent);
            return CreatedAtAction(nameof(Get), new { id = ent.Id }, resultDto);
        }

        // PUT: api/Rental/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] RentalUpdateDto dto)
        {
            var ent = await _rentalRepo.GetByIdAsync(id);
            if (ent == null) return NotFound();

            _mapper.Map(dto, ent);
            await _rentalRepo.UpdateAsync(ent);
            return NoContent();
        }

        // DELETE: api/Rental/5
        [HttpDelete("{id}")]
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
