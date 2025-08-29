using Project_Manager.Data;
using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class EmployeeRepository(AppContextDB context) : IEmployeeRepository
    {
        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default) 
        {
           await context.Employees.AddAsync(employee);     //Add employee object to context 
           await context.SaveChangesAsync();    //Save changes to BD
        }
    }
}
