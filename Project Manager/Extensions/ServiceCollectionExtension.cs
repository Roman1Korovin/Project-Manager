using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project_Manager.Data;
using Project_Manager.Data_Access.Repositories;
using Project_Manager.Data_Access.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace Project_Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //Configurating DbContext for SQL Server with specified connection string
            serviceCollection.AddDbContext<AppContextDB>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            //Regisering repositories 
            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IProjectRepository, ProjectRepository>();
            serviceCollection.AddScoped<IEmployeeOnProjectRepository, EmployeeOnProjectRepository>();
            serviceCollection.AddScoped<ICustomerCompanyRepository, CustomerCompanyRepository>();
            serviceCollection.AddScoped<IExecutorCompanyRepository, ExecutorCompanyRepository>();

            return serviceCollection;
        }



    }
}
