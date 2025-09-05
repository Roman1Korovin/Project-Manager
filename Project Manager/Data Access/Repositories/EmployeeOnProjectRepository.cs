using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class EmployeeOnProjectRepository(AppContextDB context) : IEmployeeOnProjectRepository
    {
        public async Task AddAsync(EmployeeOnProject employeeOnProject, CancellationToken cancellationToken = default)
        {
            await context.EmployeeOnProjects.AddAsync(employeeOnProject,cancellationToken);     //Add EmployeeOnProject object to context 
            await context.SaveChangesAsync(cancellationToken);    //Save changes to BD
        }

        public async Task DeleteAsync(EmployeeOnProject employeeOnProject, CancellationToken cancellationToken = default)
        {
            context.EmployeeOnProjects.Remove(employeeOnProject);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async  Task<List<EmployeeOnProject>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            return await context.EmployeeOnProjects
                .Where(e => e.EmployeeId == employeeId)
                .Include(e => e.Project)
                    .ThenInclude(p => p.CustomerCompany)
                .Include(e => e.Project)
                    .ThenInclude(p => p.ExecutorCompany)
                .Include(e => e.Project)
                    .ThenInclude(p => p.Manager)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeeOnProject>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            return await context.EmployeeOnProjects
                .Where(ep => ep.ProjectId == projectId)
                .Include(ep => ep.Employee)
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            await context.EmployeeOnProjects
                .Where(ep => ep.ProjectId == projectId)
                .ExecuteDeleteAsync(cancellationToken);

            context.ChangeTracker.Clear();
        }

        public async Task<EmployeeOnProject?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.EmployeeOnProjects
                 .Include(ep => ep.Project)
                 .Include(ep => ep.Employee)
                 .FirstOrDefaultAsync(ep => ep.Id == id, cancellationToken);
        }

        
    }
}
