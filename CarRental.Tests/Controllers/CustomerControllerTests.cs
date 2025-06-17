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
    public class CustomerControllerTests
    {
        private readonly Mock<IRepository<Customer>> _repoMock;
        private readonly IMapper _mapper;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _repoMock = new Mock<IRepository<Customer>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _controller = new CustomerController(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfCustomerDto()
        {
            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<Customer>
                     {
                         new Customer { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.c" }
                     });

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(ok.Value);
            Assert.Single(dtos);
        }

        [Fact]
        public async Task Get_ExistingId_ReturnsOk_WithCustomerDto()
        {
            _repoMock.Setup(r => r.GetByIdAsync(5))
                     .ReturnsAsync(new Customer { Id = 5, FirstName = "X", LastName = "Y", Email = "x@y.z" });

            var result = await _controller.Get(5);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<CustomerDto>(ok.Value);
            Assert.Equal(5, dto.Id);
        }

        [Fact]
        public async Task Get_NonExistingId_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Customer?)null);

            var result = await _controller.Get(99);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedAtAction()
        {
            var dto = new CustomerCreateDto { FirstName = "F", LastName = "L", Email = "f@l.com" };
            // repo.AddAsync nie musi zwracać nic, tylko zapisujemy
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                     .Returns(Task.CompletedTask)
                     .Callback<Customer>(c => c.Id = 42);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnDto = Assert.IsType<CustomerDto>(created.Value);
            Assert.Equal(42, returnDto.Id);
        }

        [Fact]
        public async Task Update_Existing_ReturnsNoContent()
        {
            var existing = new Customer { Id = 7, FirstName = "A", LastName = "B", Email = "a@b.c" };
            _repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new CustomerUpdateDto { Id = 7, FirstName = "AA", LastName = "BB", Email = "aa@bb.com" };
            var result = await _controller.Update(7, dto);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("AA", existing.FirstName);
        }

        [Fact]
        public async Task Update_NonExisting_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Customer?)null);

            var dto = new CustomerUpdateDto { Id = 10, FirstName = "X", LastName = "Y", Email = "x@y.z" };
            var result = await _controller.Update(10, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Existing_ReturnsNoContent()
        {
            var existing = new Customer { Id = 8 };
            _repoMock.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DeleteAsync(existing)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(8);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExisting_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Customer?)null);

            var result = await _controller.Delete(99);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
