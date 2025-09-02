using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class ExecutorCompanyRepository(AppContextDB context) : IExecutorCompanyRepository
    {
        public async Task AddAsync(ExecutorCompany executorCompany, CancellationToken cancellationToken = default)
        {
            await context.ExecutorCompanies.AddAsync(executorCompany, cancellationToken);     //Add ExecutorCompany object to context 
            await context.SaveChangesAsync(cancellationToken);    //Save changes to BD
        }

        public async Task<List<ExecutorCompany>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.ExecutorCompanies.ToListAsync(cancellationToken);
        }

        public async Task<ExecutorCompany?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.ExecutorCompanies.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
