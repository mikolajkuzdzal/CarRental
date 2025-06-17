using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Application.DTOs;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Mapping;
using CarRental.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRental.Tests.Controllers
{
    public class RentalControllerTests
    {
        private readonly Mock<IRepository<Rental>> _rentalRepo;
        private readonly Mock<IRepository<Car>> _carRepo;
        private readonly Mock<IRepository<Customer>> _customerRepo;
        private readonly IMapper _mapper;
        private readonly RentalController _controller;

        public RentalControllerTests()
        {
            _rentalRepo = new Mock<IRepository<Rental>>();
            _carRepo = new Mock<IRepository<Car>>();
            _customerRepo = new Mock<IRepository<Customer>>();

            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            _mapper = cfg.CreateMapper();

            _controller = new RentalController(
                _rentalRepo.Object,
                _carRepo.Object,
                _customerRepo.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfRentalDto()
        {
            // Arrange
            var rentals = new List<Rental>
            {
                new Rental { Id = 1, CarId = 10, CustomerId = 20, RentalDate = System.DateTime.Now }
            };
            _rentalRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(rentals);

            // Act
            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            var dtos = Assert.IsAssignableFrom<IEnumerable<RentalDto>>(ok.Value);
            Assert.Single(dtos);
        }

        [Fact]
        public async Task Get_ExistingId_ReturnsOk_WithRentalDto()
        {
            var rental = new Rental { Id = 2, CarId = 11, CustomerId = 21, RentalDate = System.DateTime.Today };
            _rentalRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(rental);

            var result = await _controller.Get(2);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<RentalDto>(ok.Value);
            Assert.Equal(2, dto.Id);
        }

        [Fact]
        public async Task Get_NonExistingId_ReturnsNotFound()
        {
            _rentalRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Rental?)null);

            var result = await _controller.Get(99);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedAtAction()
        {
            var dto = new RentalCreateDto
            {
                CarId = 5,
                CustomerId = 6,
                RentalDate = System.DateTime.UtcNow
            };

            // Repozytoria Car i Customer zwracają istniejące
            _carRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(new Car { Id = 5 });
            _customerRepo.Setup(r => r.GetByIdAsync(6)).ReturnsAsync(new Customer { Id = 6 });

            _rentalRepo
                .Setup(r => r.AddAsync(It.IsAny<Rental>()))
                .Returns(Task.CompletedTask)
                .Callback<Rental>(r => r.Id = 99);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnDto = Assert.IsType<RentalDto>(created.Value);
            Assert.Equal(99, returnDto.Id);
        }

        [Fact]
        public async Task Create_InvalidCarOrCustomer_ReturnsBadRequest()
        {
            _carRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Car?)null);
            _customerRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer?)null);

            var dto = new RentalCreateDto { CarId = 1, CustomerId = 2, RentalDate = System.DateTime.UtcNow };
            var result = await _controller.Create(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Invalid CarId or CustomerId", badRequest.Value.ToString());
        }

        [Fact]
        public async Task Update_Existing_ReturnsNoContent()
        {
            var existing = new Rental { Id = 7, CarId = 1, CustomerId = 2, RentalDate = System.DateTime.Today };
            _rentalRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
            _rentalRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new RentalUpdateDto { Id = 7, ReturnDate = System.DateTime.Today.AddDays(1) };
            var result = await _controller.Update(7, dto);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(dto.ReturnDate, existing.ReturnDate);
        }

        [Fact]
        public async Task Update_NonExisting_ReturnsNotFound()
        {
            _rentalRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Rental?)null);

            var dto = new RentalUpdateDto { Id = 10, ReturnDate = System.DateTime.Today };
            var result = await _controller.Update(10, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Existing_ReturnsNoContent()
        {
            var existing = new Rental { Id = 8 };
            _rentalRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
            _rentalRepo.Setup(r => r.DeleteAsync(existing)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(8);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExisting_ReturnsNotFound()
        {
            _rentalRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Rental?)null);

            var result = await _controller.Delete(99);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
