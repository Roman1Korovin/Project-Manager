using Microsoft.AspNetCore.Http.HttpResults;
using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.Data_Access.Repositories;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;
using Project_Manager.DTOs;
namespace Project_Manager.BusinessLogic.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
    {
        
        private void ValidateEmployeeData(string fullname, string email)
        {
            //Validate that fullName is not empty and doesn't consists of spaces 
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentNullException("ФИО сотрудника не может быть пустым!", nameof(fullname));

            //Validate that fullName is not more than 500 symbols 
            if (fullname.Length > 500)
                throw new ArgumentException("ФИО сотрудника не может превышать 500 символов!", nameof(fullname));

            //Validate that email is not empty and doesn't consists of spaces 
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email сотрудника не может быть пустым!", nameof(email));

            //Validate that email is not more than 500 symbols 
            if (email.Length > 500)
                throw new ArgumentException("Email сотрудника не может превышать 500 символов!", nameof(email));

            //Validate that email contains @ symbol
            if (!email.Contains("@"))
                throw new ArgumentException("Некоректный формат email", nameof(email));
        }

        //Returns list of DTO contining only necessary field
        public async Task<List<EmployeeDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var employees = await employeeRepository.GetAllAsync(cancellationToken);
            return employees.Select(e => new EmployeeDTO {Id = e.Id, FullName = e.FullName, Email = e.Email }).ToList();
        }


        public async Task AddAsync(EmployeeDTO dto, CancellationToken cancellationToken = default)
        {
            ValidateEmployeeData(dto.FullName, dto.Email);

            //Create Employee object and delete spaces at begin and at end
            var employee = new Employee
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim()
            };
            //Call repository method to add new Employee
            await employeeRepository.AddAsync(employee, cancellationToken);
        }

        public async Task UpdateAsync(int id, EmployeeDTO dto, CancellationToken cancellationToken = default)
        {
            ValidateEmployeeData(dto.FullName, dto.Email);

            //Check that employee with current Id is exists
            var employee = await employeeRepository.GetByIdAsync(id, cancellationToken);
            if (employee == null)
                throw new KeyNotFoundException($"Сотрудник с Id {id} не найден.");

            //Update Employee object and delete spaces at begin and at end
            employee.FullName = dto.FullName.Trim();
            employee.Email = dto.Email.Trim();

            await employeeRepository.UpdateAsync(employee, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            //Check that employee with current Id is exists
            var employee = await employeeRepository.GetByIdAsync(id, cancellationToken);
            if (employee == null)
                throw new KeyNotFoundException($"Сотрудник с Id {id} не найден.");

            await employeeRepository.DeleteAsync(employee, cancellationToken);
        }
    }
}
