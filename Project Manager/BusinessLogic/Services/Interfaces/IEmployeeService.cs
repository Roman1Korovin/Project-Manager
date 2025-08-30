using Project_Manager.Models.Domain;
using Project_Manager.DTOs;

namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task AddAsync(EmployeeDTO dto, CancellationToken cancellationToken = default);
        Task<List<EmployeeDTO>> GetAllAsync(CancellationToken cancellationToken = default);

        Task UpdateAsync(int id, EmployeeDTO dto, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
