using Project_Manager.DTOs;
using Project_Manager.Models.Domain;

namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface IEmployeeOnProjectService
    {

        Task AddAsync(EmployeeOnProjectDTO dto, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<List<EmployeeOnProjectDTO>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

        Task<List<EmployeeOnProjectDTO>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);

        Task DeleteByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

    }
}
