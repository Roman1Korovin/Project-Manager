using Project_Manager.DTOs;

namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface IProjectService
    {
        Task AddAsync(ProjectDTO dto, CancellationToken cancellationToken = default);
        Task<List<ProjectDTO>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ProjectDTO> GetByIdAsync(int id, CancellationToken cancellationToken= default);

        Task UpdateAsync(int id, ProjectDTO dto, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        
    }
}
