using Project_Manager.Data_Access.Repositories.Interfaces;
using Project_Manager.Models.Domain;

namespace Project_Manager.Data_Access.Repositories
{
    public class ProjectRepository(AppContextDB context) : IProjectRepository
    {
        public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            await context.Projects.AddAsync(project);     //Add project object to context 
            await context.SaveChangesAsync();    //Save changes to BD
        }
    }
}
