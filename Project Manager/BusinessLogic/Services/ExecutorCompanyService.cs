using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.Data_Access.Repositories;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.DTOs;
using Project_Manager.Models.Domain;

namespace Project_Manager.BusinessLogic.Services
{
    public class ExecutorCompanyService(IExecutorCompanyRepository executorCompanyRepository) : IExecutorCompanyService
    {
        private void ValidateExecutorCompanyData(ExecutorCompanyDTO dto)
        {
            //Validate that executorCompany is not empty and doesn't consists of spaces 
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentNullException("Название компании заказчика не может быть пустым!", nameof(dto.Name));

            //Validate that executorCompany is not more than 500 symbols 
            if (dto.Name.Length > 500)
                throw new ArgumentException("Названиекомпании заказчика не может превышать 500 символов!", nameof(dto.Name));
        }

        public async Task<CustomerCompanyDTO> AddAsync(ExecutorCompanyDTO dto, CancellationToken cancellationToken = default)
        {
            ValidateExecutorCompanyData(dto);

            //Create ExecutorCompany object and delete spaces at begin and at end
            var executorCompany = new ExecutorCompany
            {
                Name = dto.Name.Trim(),
            };
            //Call repository method to add new ExecutorCompany
            await executorCompanyRepository.AddAsync(executorCompany, cancellationToken);

            return new CustomerCompanyDTO
            {
                Id = executorCompany.Id,
                Name = executorCompany.Name
            };
        }

        //Returns list of DTO contining only necessary field
        public async Task<List<ExecutorCompanyDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var executorCompanies = await executorCompanyRepository.GetAllAsync(cancellationToken);
            return executorCompanies.Select(c => new ExecutorCompanyDTO { Id = c.Id, Name = c.Name }).ToList();
        }
    }
}
