using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IEmployeeOnProjectRepository
    {
        // Add new EmployeeOnProjectRepository
        // Accepts EmployeeOnProjectRepository object and cancellation token 
        Task AddAsync(EmployeeOnProject eployeeOnProject, CancellationToken cancellationToken = default);

        Task DeleteAsync(EmployeeOnProject eployeeOnProject, CancellationToken cancellationToken = default);

        //Get all project for specific employee 
        Task<List<EmployeeOnProject>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
        //Get all employees for specific project 
        Task<List<EmployeeOnProject>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);

        //Get specific record by Id
        Task<EmployeeOnProject?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
