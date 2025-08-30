using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.BusinessLogic.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
    {
        public async Task AddAsync(string fullname, string email, CancellationToken cancellationToken = default)
        {
            //Validate that fullName is not empty and doesn't consists of spaces 
            if(string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentNullException("ФИО сотрудника не может быть пустым!", nameof(fullname));

            //Validate that fullName is less than 500 symbols 
            if (fullname.Length > 500)
                throw new ArgumentException("ФИО сотрудника не может превышать 500 символов!", nameof(fullname));

            //Validate that email is not empty and doesn't consists of spaces 
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email сотрудника не может быть пустым!", nameof(email));

            //Validate that email is less than 500 symbols 
            if (email.Length > 500)
                throw new ArgumentException("Email сотрудника не может превышать 500 символов!", nameof(email));

            //Validate that email contains @ symbol
            if (!email.Contains("@"))
                throw new ArgumentException("Некоректный формат email", nameof(email));

            //Create Employee object and delete spaces at begin and at end
            var employee = new Employee
            {
                FullName = fullname.Trim(),
                Email = email.Trim()
            };
            //Call repository method to add new Employee
            await employeeRepository.AddAsync(employee, cancellationToken);
        }
    }
}
