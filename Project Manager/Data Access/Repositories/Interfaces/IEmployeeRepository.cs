using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        // Add new employee
        // Accepts Employee object and cancellation token 
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
    }
}
