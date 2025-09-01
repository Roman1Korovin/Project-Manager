using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class CustomerCompanyRepository(AppContextDB context) : ICustomerCompanyRepository
    {
        public async Task AddAsync(CustomerCompany customerCompany, CancellationToken cancellationToken = default)
        {
            await context.CustomerCompanies.AddAsync(customerCompany, cancellationToken);     //Add CustomerCompany object to context 
            await context.SaveChangesAsync(cancellationToken);    //Save changes to BD
        }

        public async Task<CustomerCompany?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.CustomerCompanies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
