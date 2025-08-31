using Project_Manager.DTOs;

namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface ICustomerCompanyService
    {
        Task AddAsync(CustomerCompanyDTO dto, CancellationToken cancellationToken = default);
    }
}
