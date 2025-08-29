using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IEmployeeOnProjectRepository
    {
        // Add new EmployeeOnProjectRepository
        // Accepts EmployeeOnProjectRepository object and cancellation token 
        Task AddAsync(EmployeeOnProject eployeeOnProject, CancellationToken cancellationToken = default);
    }
}
