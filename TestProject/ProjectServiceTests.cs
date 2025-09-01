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
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepoMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
        private readonly Mock<ICustomerCompanyRepository> _customerRepoMock = new();
        private readonly Mock<IExecutorCompanyRepository> _executorRepoMock = new();

        private ProjectService CreateService()
        {
            return new ProjectService(
                _projectRepoMock.Object,
                _employeeRepoMock.Object,
                _customerRepoMock.Object,
                _executorRepoMock.Object
            );
        }

        private ProjectDTO CreateValidDto()
        {
            return new ProjectDTO
            {
                Id = 1,
                Name = "Test Project",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10),
                Priority = 3,
                CustomerCompanyID = 100,
                ExecutorCompanyID = 200,
                ManagerID = 300
            };
        }

        [Fact]
        public async Task AddAsync_ShouldAddProject_WhenDataIsValid()
        {
            // Arrange
            var service = CreateService();
            var dto = CreateValidDto();

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.ManagerID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Employee { Id = dto.ManagerID });

            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustomerCompanyID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new CustomerCompany { Id = dto.CustomerCompanyID });

            _executorRepoMock.Setup(r => r.GetByIdAsync(dto.ExecutorCompanyID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ExecutorCompany { Id = dto.ExecutorCompanyID });

            _projectRepoMock.Setup(r => r.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);

            // Act
            await service.AddAsync(dto);

            // Assert
            _projectRepoMock.Verify(r => r.AddAsync(It.Is<Project>(p => p.Name == dto.Name.Trim()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsKeyNotFoundException_WhenManagerNotFound()
        {
            // Arrange
            var service = CreateService();
            var dto = CreateValidDto();

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.ManagerID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Employee)null);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsKeyNotFoundException_WhenCustomerNotFound()
        {
            // Arrange
            var service = CreateService();
            var dto = CreateValidDto();

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.ManagerID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Employee { Id = dto.ManagerID });

            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustomerCompanyID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((CustomerCompany)null);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsKeyNotFoundException_WhenExecutorNotFound()
        {
            // Arrange
            var service = CreateService();
            var dto = CreateValidDto();

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.ManagerID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Employee { Id = dto.ManagerID });

            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustomerCompanyID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new CustomerCompany { Id = dto.CustomerCompanyID });

            _executorRepoMock.Setup(r => r.GetByIdAsync(dto.ExecutorCompanyID, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((ExecutorCompany)null);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowInvalidOperationException_WhenStartDateAfterEndDate()
        {
            // Arrange
            var service = CreateService();
            var dto = CreateValidDto();
            dto.StartDate = DateTime.Today.AddDays(10);
            dto.EndDate = DateTime.Today;

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(dto));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(6)]
        public async Task AddAsync_ThrowArgumentException_WhenPriorityIsInvalid(int invalidPriority)
        {
            // Arrange
            var service = CreateService();
            var dto = CreateValidDto();
            dto.Priority = invalidPriority;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsProjectDTO_WhenAlways()
        {
            // Arrange
            var projects = new List<Project>
    {
        new Project
        {
            Id = 1,
            Name = "Проект A",
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 12, 31),
            Priority = 3,
            CustomerCompanyID = 10,
            ExecutorCompanyID = 20,
            ManagerID = 30
        },
        new Project
        {
            Id = 2,
            Name = "Проект B",
            StartDate = new DateTime(2025, 2, 1),
            EndDate = new DateTime(2025, 11, 30),
            Priority = 5,
            CustomerCompanyID = 11,
            ExecutorCompanyID = 21,
            ManagerID = 31
        }
    };

            _projectRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(projects);

            var service = CreateService();


            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(projects[0].Id, result[0].Id);
            Assert.Equal(projects[0].Name, result[0].Name);
            Assert.Equal(projects[0].Priority, result[0].Priority);

            Assert.Equal(projects[1].Id, result[1].Id);
            Assert.Equal(projects[1].Name, result[1].Name);
            Assert.Equal(projects[1].ManagerID, result[1].ManagerID);

            _projectRepoMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdateAsync_WhenProjectExists()
        {
            // Arrange
            var service = CreateService();

            int id = 1;
            var existingProject = new Project
            {
                Id = id,
                Name = "Old Project",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10),
                Priority = 3,
                CustomerCompanyID = 100,
                ExecutorCompanyID = 200,
                ManagerID = 300
            };

            var dto = new ProjectDTO
            {
                Name = "Updated Project",
                StartDate = existingProject.StartDate,
                EndDate = existingProject.EndDate,
                Priority = 4,
                CustomerCompanyID = existingProject.CustomerCompanyID,
                ExecutorCompanyID = existingProject.ExecutorCompanyID,
                ManagerID = existingProject.ManagerID
            };

           
            _projectRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(existingProject);

             _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.ManagerID, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Employee { Id = dto.ManagerID });

            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustomerCompanyID, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new CustomerCompany { Id = dto.CustomerCompanyID });

            _executorRepoMock.Setup(r => r.GetByIdAsync(dto.ExecutorCompanyID, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ExecutorCompany { Id = dto.ExecutorCompanyID });
            // Act
            await service.UpdateAsync(id, dto);

            // Assert
            _projectRepoMock.Verify(r => r.UpdateAsync(
                It.Is<Project>(p =>
                    p.Id == id &&
                    p.Name == dto.Name &&
                    p.Priority == dto.Priority &&
                    p.ManagerID == dto.ManagerID &&
                    p.CustomerCompanyID == dto.CustomerCompanyID &&
                    p.ExecutorCompanyID == dto.ExecutorCompanyID
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var service = CreateService();

            int id = 1;
            var dto = new ProjectDTO
            {
                Name = "New Project",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5),
                Priority = 3,
                CustomerCompanyID = 100,
                ExecutorCompanyID = 200,
                ManagerID = 300
            };

            _projectRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(id, dto));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDeleteAsync_WhenProjectExists()
        {
            // Arrange
            var service = CreateService();
            int projectId = 1;

            var project = new Project
            {
                Id = projectId,
                Name = "Test Project"
            };

            _projectRepoMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(project);

            // Act
            await service.DeleteAsync(projectId);

            // Assert
            _projectRepoMock.Verify(r => r.DeleteAsync(project, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var service = CreateService();
            int projectId = 1;

            _projectRepoMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Project?)null);

            // Act & Assert
            Task action() => service.DeleteAsync(projectId);

            await Assert.ThrowsAsync<KeyNotFoundException>(action);
        }
    }
}