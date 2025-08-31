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


namespace TestsProject
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly EmployeeService _employeeService;
        
        public EmployeeServiceTests()
        {
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepoMock.Object);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAddAsync_WhenDataCorrect()
        {
            //arrange 
            var dto = new EmployeeDTO
            {
                FullName = "Смирнов Илья Иванович",
                Email = "smirnov2222@mail.ru"
            };

            //act
            await _employeeService.AddAsync(dto);

            //assert
            _employeeRepoMock.Verify(repo => repo.AddAsync(
                It.Is<Employee>(e => e.FullName == dto.FullName && e.Email == dto.Email),
                It.IsAny<CancellationToken>()
                ), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenFullNameIsEmpty()
        {
            //arrange 
            var dto = new EmployeeDTO 
            { 
                FullName = "",
                Email = "smirnov2222@mail.ru" 
            };


            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenFullNameConsistsOnlySpaces()
        {
            //arrange 
            var dto = new EmployeeDTO 
            { 
                FullName = "     ", 
                Email = "smirnov2222@mail.ru" 
            };


            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentException_WhenFullNameIsMore500Symbols()
        {
            //arrange 
            var dto = new EmployeeDTO 
            { 
                FullName = new string('a', 501), 
                Email = "smirnov2222@mail.ru" 
            };

            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenEmailIsEmpty()
        {
            //arrange 
            var dto = new EmployeeDTO 
            { 
                FullName = "Смирнов Илья Иванович", 
                Email = "" 
            };

            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenEmailConsistsOnlySpaces()
        {
            //arrange 
            var dto = new EmployeeDTO { 
                FullName = "Смирнов Илья Иванович", 
                Email = "     " 
            };


            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentException_WhenEmailIsMore500Symbols()
        {
            //arrange 
            var dto = new EmployeeDTO { 
                FullName = "Смирнов Илья Иванович", 
                Email = new string('a', 501) 
            };

            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task AddпAsync_ThrowsArgumentException_WhenEmailDoesntConsistSpecialSymbol()
        {
            //arrange 
            var dto = new EmployeeDTO 
            { 
                FullName = "Смирнов Илья Иванович", 
                Email = "smirnov2222mail.ru" 
            };

            //act & assert
            Task action() => _employeeService.AddAsync(dto);
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfEmployeeDTO_WhenEmployeesExist()
        {
            //arrange 
            var employees = new List<Employee>
            {
                new Employee { Id = 1, FullName = "Иван Иванов", Email = "ivan@mail.ru" },
                new Employee { Id = 2, FullName = "Пётр Петров", Email = "petr@mail.ru" }
                };

            _employeeRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            //act
            var result = await _employeeService.GetAllAsync();

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Иван Иванов", result[0].FullName);
            Assert.Equal("ivan@mail.ru", result[0].Email);
            Assert.Equal("Пётр Петров", result[1].FullName);
            Assert.Equal("petr@mail.ru", result[1].Email);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdateAsync_WhenEmployeeExists()
        {
            //arrange
            int id = 1;
            var existingEmployee = new Employee 
            {   Id = id, 
                FullName = "Иван Иванов", 
                Email = "ivan@mail.ru" 
            };

            var dto = new EmployeeDTO 
            { 
                FullName = "Иван Updated", 
                Email = "ivan.updated@mail.ru" 
            };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingEmployee);
            //act
            await _employeeService.UpdateAsync(id, dto);
            //assert
            _employeeRepoMock.Verify(r => r.UpdateAsync(
                It.Is<Employee>(e => e.Id == id && e.FullName == dto.FullName && e.Email == dto.Email),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenEmployeeDoesNotExist()
        {
            //arrange
            int id = 1;
            var dto = new EmployeeDTO 
            { 
                FullName = "Иван", 
                Email = "ivan@mail.ru" 
            };
            //act & assert
            _employeeRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Employee?)null);

            Task action() => _employeeService.UpdateAsync(id, dto);

            await Assert.ThrowsAsync<KeyNotFoundException>(action);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDeleteAsync_WhenEmployeeExists()
        {
            //arrange
            int id = 1;
            var employee = new Employee 
            { 
                Id = id, 
                FullName = "Иван Иванов", 
                Email = "ivan@mail.ru" 
            };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(employee);
            //act
            await _employeeService.DeleteAsync(id);
            //assert
            _employeeRepoMock.Verify(r => r.DeleteAsync(employee, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenEmployeeDoesNotExist()
        {
            //arrange
            int id = 1;
            _employeeRepoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Employee?)null);
            //act & assert
            Task action() => _employeeService.DeleteAsync(id);

            await Assert.ThrowsAsync<KeyNotFoundException>(action);
        }

    }
}
