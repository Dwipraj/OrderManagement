using DataAccess;
using DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
	public static class StorageServicesExtensions
	{
		public static IServiceCollection StorageServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IUnitOfWork>(c => new UnitOfWork(configuration.GetConnectionString("DefaultConnection")));

			return services;
		}
	}
}
