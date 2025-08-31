using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        // Add new Project
        // Accepts Project object and cancellation token 
        Task AddAsync(Project project, CancellationToken cancellationToken = default);

        Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
        Task DeleteAsync(Project project, CancellationToken cancellationToken = default);
    }
}
