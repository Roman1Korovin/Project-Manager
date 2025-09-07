using Project_Manager.DTOs;

namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface ICustomerCompanyService
    {
        Task<CustomerCompanyDTO> AddAsync(CustomerCompanyDTO dto, CancellationToken cancellationToken = default);
        Task<List<CustomerCompanyDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
