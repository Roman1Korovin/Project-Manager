using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project_Manager.Data;
using System.Runtime.CompilerServices;

namespace Project_Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<AppContextDB>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return serviceCollection;
        }
    }
}
