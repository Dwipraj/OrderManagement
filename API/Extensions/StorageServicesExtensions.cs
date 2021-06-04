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
			//This DI should be used in HTTP calls rather than in BackgroundServices
			//In case of BackgroundServices this will act as a Singleton DI, as BackgroundServices will get Disposed when application will be stopped.
			//So a connection will left open always in-spite of not being used always.
			services.AddScoped<IUnitOfWork>(c => new UnitOfWork(configuration.GetConnectionString("DefaultConnection")));

			//IF THERE IS A PLAN FOR EVENT BASED ARCHITECTURE THEN IT'S RECOMANDED TO USE TRANSIENT DI

			return services;
		}
	}
}
