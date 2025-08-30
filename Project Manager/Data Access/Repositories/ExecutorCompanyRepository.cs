using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class ExecutorCompanyRepository(AppContextDB context) : IExecutorCompanyRepository
    {
        public async Task AddAsync(ExecutorCompany executorCompany, CancellationToken cancellationToken = default)
        {
            await context.ExecutorCompanies.AddAsync(executorCompany);     //Add ExecutorCompany object to context 
            await context.SaveChangesAsync();    //Save changes to BD
        }
    }
}
