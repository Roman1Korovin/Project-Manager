using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class EmployeeRepository(AppContextDB context) : IEmployeeRepository
    {
        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default) 
        {
           await context.Employees.AddAsync(employee, cancellationToken);     //Add employee object to context 
           await context.SaveChangesAsync(cancellationToken);    //Save changes to BD
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Employees.ToListAsync(cancellationToken);
        }
        public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }


        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            context.Employees.Update(employee);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            // Check that employee is manager of at least one project
            var projectsWhereManager = await context.Projects
                .Where(p => p.ManagerID == employee.Id)
                .ToListAsync(cancellationToken);

            if (projectsWhereManager.Any())
            {
                var projectNames = string.Join(", ", projectsWhereManager.Select(p => p.Name));
                throw new InvalidOperationException(
                    $"Невозможно удалить сотрудника {employee.FullName}, так как он является менеджером проектов: {projectNames}."
                );
            }

            context.Employees.Remove(employee);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
