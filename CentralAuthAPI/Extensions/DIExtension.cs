using CentralAuthAPI.Repository;
using CentralAuthAPI.Repository.IRepository;
using CentralAuthAPI.Service;

namespace CentralAuthAPI.Extensions
{
	public static class DIExtension
	{
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }

    }
}
