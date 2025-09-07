using Project_Manager.BusinessLogic.Services.Interfaces;
using Project_Manager.Data_Access.Repositories;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.DTOs;
using Project_Manager.Models.Domain;

namespace Project_Manager.BusinessLogic.Services
{
    public class CustomerCompanyService(ICustomerCompanyRepository customerCompanyRepository) : ICustomerCompanyService
    {
        private void ValidateCustomerCompanyData(CustomerCompanyDTO dto)
        {
            //Validate that customerCompany is not empty and doesn't consists of spaces 
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentNullException("Название компании заказчика не может быть пустым!", nameof(dto.Name));

            //Validate that customerCompany is not more than 500 symbols 
            if (dto.Name.Length > 500)
                throw new ArgumentException("Названиекомпании заказчика не может превышать 500 символов!", nameof(dto.Name));
        }

        public async Task<CustomerCompanyDTO> AddAsync(CustomerCompanyDTO dto, CancellationToken cancellationToken = default)
        {
            ValidateCustomerCompanyData(dto);

            //Create CustomerCompany object and delete spaces at begin and at end
            var customerCompany = new CustomerCompany
            {
                Name = dto.Name.Trim(),
            };
            //Call repository method to add new CustomerCompany
            await customerCompanyRepository.AddAsync(customerCompany, cancellationToken);

            return new CustomerCompanyDTO
            {
                Id = customerCompany.Id,
                Name = customerCompany.Name
            };
        }

        //Returns list of DTO contining only necessary field
        public async Task<List<CustomerCompanyDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var customerCompanies = await customerCompanyRepository.GetAllAsync(cancellationToken);
            return customerCompanies.Select(c => new CustomerCompanyDTO { Id = c.Id, Name = c.Name }).ToList();
        }
    }
}

