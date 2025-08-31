using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class ProjectRepository(AppContextDB context) : IProjectRepository
    {
        public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            await context.Projects.AddAsync(project);     //Add project object to context 
            await context.SaveChangesAsync(cancellationToken);    //Save changes to BD
        }

        public async Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Projects
                .Include(p=>p.CustomerCompany)
                .Include(p => p.ExecutorCompany)
                .Include(p => p.Manager)
                .Include(p => p.EmployeesOnProject)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Projects
                .Include(p => p.CustomerCompany)
                .Include(p => p.ExecutorCompany)
                .Include(p => p.Manager)
                .Include(p => p.EmployeesOnProject)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }


        public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
        {
            context.Projects.Update(project);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
        {
            context.Projects.Remove(project);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
