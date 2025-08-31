using Project_Manager.BusinessLogic.Services;
using Project_Manager.BusinessLogic.Services.Interfaces;

namespace Project_Manager.BusinessLogic.Extensions
{
    public static class BusinessLogicExtension
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection) 
        {

            serviceCollection.AddScoped<IEmployeeService, EmployeeService>();
            serviceCollection.AddScoped<IProjectService, ProjectService>();
            return serviceCollection;
        }
    }
}
