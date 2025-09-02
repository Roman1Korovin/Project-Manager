using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IExecutorCompanyRepository
    {
        // Add new ExecutorCompany
        // Accepts ExecutorCompany object and cancellation token 
        Task AddAsync(ExecutorCompany executorCompany, CancellationToken cancellationToken = default);

        Task<List<ExecutorCompany>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ExecutorCompany?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
