using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.Models.Domain;
using Project_Manager.DTOs;


namespace TestProject
{
    public class CustomerCompanyServiceTests
    {
        private readonly Mock<ICustomerCompanyRepository> _customerRepoMock;
        private readonly CustomerCompanyService _customerCompanyService;

        public CustomerCompanyServiceTests()
        {
            _customerRepoMock = new Mock<ICustomerCompanyRepository>();
            _customerCompanyService = new CustomerCompanyService(_customerRepoMock.Object);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAddAsync_WhenDataCorrect()
        {
            // Arrange
            var dto = new CustomerCompanyDTO
            {
                Name = "Компания Рога и Копыта"
            };

            // Act
            await _customerCompanyService.AddAsync(dto);

            // Assert
            _customerRepoMock.Verify(r => r.AddAsync(
                It.Is<CustomerCompany>(c => c.Name == dto.Name.Trim()),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new CustomerCompanyDTO { Name = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerCompanyService.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenNameIsWhiteSpaces()
        {
            // Arrange
            var dto = new CustomerCompanyDTO { Name = "   " };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerCompanyService.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentException_WhenNameIsTooLong()
        {
            // Arrange
            var dto = new CustomerCompanyDTO
            {
                Name = new string('A', 501) 
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _customerCompanyService.AddAsync(dto));
        }
    }
}
