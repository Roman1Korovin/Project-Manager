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
    public class EmployeeOnProjectServiceTests
    {
        private readonly Mock<IEmployeeOnProjectRepository> _eopRepoMock;
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly Mock<IProjectRepository> _projectRepoMock;
        private readonly EmployeeOnProjectService _service;


        public EmployeeOnProjectServiceTests()
        {
            _eopRepoMock = new Mock<IEmployeeOnProjectRepository>();
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _projectRepoMock = new Mock<IProjectRepository>();

            _service = new EmployeeOnProjectService(
                _eopRepoMock.Object,
                _employeeRepoMock.Object,
                _projectRepoMock.Object
            );
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAddAsync_WhenDataIsValid()
        {
            // Arrange
            var dto = new EmployeeOnProjectCreateDTO
            {
                EmployeeId = 1,
                ProjectId = 10
            };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.EmployeeId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Employee { Id = dto.EmployeeId });

            _projectRepoMock.Setup(r => r.GetByIdAsync(dto.ProjectId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new Project { Id = dto.ProjectId });

            // Act
            await _service.AddAsync(dto);

            // Assert
            _eopRepoMock.Verify(r => r.AddAsync(
                It.Is<EmployeeOnProject>(eop => eop.EmployeeId == dto.EmployeeId && eop.ProjectId == dto.ProjectId),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsKeyNotFoundException_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var dto = new EmployeeOnProjectCreateDTO { EmployeeId = 1, ProjectId = 10 };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.EmployeeId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddAsync(dto));
        }
        [Fact]
        public async Task AddAsync_ThrowsKeyNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var dto = new EmployeeOnProjectCreateDTO { EmployeeId = 1, ProjectId = 10 };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.EmployeeId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Employee { Id = dto.EmployeeId });

            _projectRepoMock.Setup(r => r.GetByIdAsync(dto.ProjectId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDeleteAsync_WhenRecordExists()
        {
            // Arrange
            int id = 1;
            var eop = new EmployeeOnProject
            {
                Id = id,
                EmployeeId = 10,
                ProjectId = 20
            };

            _eopRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(eop);

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _eopRepoMock.Verify(r => r.DeleteAsync(eop, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenRecordDoesNotExist()
        {
            // Arrange
            int id = 1;

            _eopRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((EmployeeOnProject?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(id));
        }

        [Fact]
        public async Task GetByEmployeeIdAsync_ReturnsEmployeeOnProjectDTO_WhenEmployeeExists()
        {
            // Arrange
            int employeeId = 10;

            _employeeRepoMock.Setup(r => r.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new Employee { Id = employeeId });

            var eops = new List<EmployeeOnProject>
        {
            new EmployeeOnProject { EmployeeId = employeeId, ProjectId = 100 },
            new EmployeeOnProject { EmployeeId = employeeId, ProjectId = 200 }
        };

            _eopRepoMock.Setup(r => r.GetByEmployeeIdAsync(employeeId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(eops);

            // Act
            var result = await _service.GetByEmployeeIdAsync(employeeId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(employeeId, result[0].EmployeeId);
            Assert.Equal(employeeId, result[1].EmployeeId);
            Assert.Equal(100, result[0].ProjectId);
            Assert.Equal(200, result[1].ProjectId);

            _employeeRepoMock.Verify(r => r.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()), Times.Once);
            _eopRepoMock.Verify(r => r.GetByEmployeeIdAsync(employeeId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByEmployeeIdAsync_ThrowsKeyNotFoundException_WhenEmployeeNotFound()
        {
            // Arrange
            int employeeId = 10;

            _employeeRepoMock.Setup(r => r.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByEmployeeIdAsync(employeeId));
        }

        [Fact]
        public async Task GetByProjectIdAsync_ReturnsEmployeeOnProjectDTO_WhenProjectExists()
        {
            // Arrange
            int projectId = 100;

            _projectRepoMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new Project { Id = projectId });

            var eops = new List<EmployeeOnProject>
            {
                 new EmployeeOnProject { EmployeeId = 10, ProjectId = projectId },
                new EmployeeOnProject { EmployeeId = 20, ProjectId = projectId }
            };

            _eopRepoMock.Setup(r => r.GetByProjectIdAsync(projectId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(eops);

            // Act
            var result = await _service.GetByProjectIdAsync(projectId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(10, result[0].EmployeeId);
            Assert.Equal(20, result[1].EmployeeId);
            Assert.Equal(projectId, result[0].ProjectId);
            Assert.Equal(projectId, result[1].ProjectId);

            _projectRepoMock.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
            _eopRepoMock.Verify(r => r.GetByProjectIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByProjectIdAsync_ThrowsKeyNotFoundException_WhenProjectNotFound()
        {
            // Arrange
            int projectId = 100;

            _projectRepoMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByProjectIdAsync(projectId));
        }


    }
}
