using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class CustomerCompanyRepository(AppContextDB context) : ICustomerCompanyRepository
    {
        public async Task AddAsync(CustomerCompany customerCompany, CancellationToken cancellationToken = default)
        {
            await context.CustomerCompanies.AddAsync(customerCompany);     //Add CustomerCompany object to context 
            await context.SaveChangesAsync();    //Save changes to BD
        }
    }
}
