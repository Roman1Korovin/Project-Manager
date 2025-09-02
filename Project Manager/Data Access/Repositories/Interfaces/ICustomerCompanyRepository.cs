using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories.Interfaces
{
    public interface ICustomerCompanyRepository
    {
        // Add new CustomerCompany
        // Accepts CustomerCompany object and cancellation token 
        Task AddAsync(CustomerCompany customerCompany, CancellationToken cancellationToken = default);
        Task<List<CustomerCompany>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CustomerCompany?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
