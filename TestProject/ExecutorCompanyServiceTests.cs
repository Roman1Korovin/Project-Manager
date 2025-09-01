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
    public class ExecutorCompanyServiceTests
    {
        private readonly Mock<IExecutorCompanyRepository> _executorRepoMock;
        private readonly ExecutorCompanyService _executorCompanyService;

        public ExecutorCompanyServiceTests()
        {
            _executorRepoMock = new Mock<IExecutorCompanyRepository>();
            _executorCompanyService = new ExecutorCompanyService(_executorRepoMock.Object);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAddAsync_WhenDataCorrect()
        {
            // Arrange
            var dto = new ExecutorCompanyDTO
            {
                Name = "Компания Рога и Копыта"
            };

            // Act
            await _executorCompanyService.AddAsync(dto);

            // Assert
            _executorRepoMock.Verify(r => r.AddAsync(
                It.Is<ExecutorCompany>(c => c.Name == dto.Name.Trim()),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new ExecutorCompanyDTO { Name = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _executorCompanyService.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenNameIsWhiteSpaces()
        {
            // Arrange
            var dto = new ExecutorCompanyDTO { Name = "   " };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _executorCompanyService.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentException_WhenNameIsTooLong()
        {
            // Arrange
            var dto = new ExecutorCompanyDTO
            {
                Name = new string('A', 501)
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _executorCompanyService.AddAsync(dto));
        }
    }
}