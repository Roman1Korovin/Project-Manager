using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface IExecutorCompanyRepository
    {
        // Add new ExecutorCompany
        // Accepts ExecutorCompany object and cancellation token 
        Task AddAsync(ExecutorCompany executorCompany, CancellationToken cancellationToken = default);
    }
}
