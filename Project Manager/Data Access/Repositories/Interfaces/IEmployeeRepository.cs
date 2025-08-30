using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        // Add new employee
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);
        Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default);
    }
}
