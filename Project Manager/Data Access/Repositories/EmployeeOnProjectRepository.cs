using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class EmployeeOnProjectRepository(AppContextDB context) : IEmployeeOnProjectRepository
    {
        public async Task AddAsync(EmployeeOnProject employeeOnProject, CancellationToken cancellationToken = default)
        {
            await context.EmployeeOnProjects.AddAsync(employeeOnProject);     //Add CustomerCompany object to context 
            await context.SaveChangesAsync();    //Save changes to BD
        }
    }
}
