using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.BusinessLogic.Services;
using Project_Manager.Models.Domain;


namespace Tests
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
        public async Task TaskAsync_CallsRepositoryAddAsync_WhenDataCorrect()
        {
            //arrange 
            string fullName = "Смирнов Илья Иванович";
            string email = "smirnov2222@mail.ru";

            //act
            await _employeeService.AddAsync(fullName, email);

            //assert
            _employeeRepoMock.Verify(repo => repo.AddAsync(
                It.Is<Employee>(e => e.FullName == fullName && e.Email == email),
                It.IsAny<CancellationToken>()
                ), Times.Once);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentNullException_WhenFullNameIsEmpty()
        {
            //arrange 
            string fullName = "";
            string email = "smirnov2222@mail.ru";


            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentNullException_WhenFullNameConsistsOnlySpaces()
        {
            //arrange 
            string fullName = "         ";
            string email = "smirnov2222@mail.ru";


            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentException_WhenFullNameIsMore500Symbols()
        {
            //arrange 
            string fullName = new string('a', 501);
            string email = "smirnov2222@mail.ru";

            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentNullException_WhenEmailIsEmpty()
        {
            //arrange 
            string fullName = "Смирнов Илья Иванович";
            string email = "";

            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentNullException_WhenEmailConsistsOnlySpaces()
        {
            //arrange 
            string fullName = "Смирнов Илья Иванович";
            string email = "         ";


            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentException_WhenEmailIsMore500Symbols()
        {
            //arrange 
            string fullName = "Смирнов Илья Иванович";
            string email = "smirnov2222";

            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task TaskAsync_ThrowsArgumentException_WhenEmailDoesntConsistSpecialSymbol()
        {
            //arrange 
            string fullName = "Смирнов Илья Иванович";
            string email = new string('a', 501) + "@mail.ru";

            //act & assert
            Task action() => _employeeService.AddAsync(fullName, email);
            await Assert.ThrowsAsync<ArgumentException>(action);
        }
    }


}
