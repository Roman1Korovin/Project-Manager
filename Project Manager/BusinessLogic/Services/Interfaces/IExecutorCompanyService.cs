using Project_Manager.DTOs;

namespace Project_Manager.BusinessLogic.Services.Interfaces
{
    public interface IExecutorCompanyService
    {
        Task<CustomerCompanyDTO> AddAsync(ExecutorCompanyDTO dto, CancellationToken cancellationToken = default);

        Task<List<ExecutorCompanyDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
