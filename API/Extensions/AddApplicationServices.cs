using API.Middleware;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			//Filter Registrations
			services.AddScoped<AdminOrOwnerFilter>();
			services.AddScoped<OwnerFilter>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddScoped<ITokenService>(x => new TokenService(configuration["Token:Key"],configuration["Token:Issuer"]));

			//This will help in case of detailed information exchange between services in a single HTTP call.
			//e.g. Suppose any service is returning boolean value - true if success : false if failed
			//Now we can set a error message just before returnig the false and as this is registered as a scoped DI it will persist the data in that particular HTTP request context
			services.AddScoped<ILogicalErrorMessage, LogicalErrorMessage>();

			services.AddScoped<IOrderService, OrderService>();

			return services;
		}
	}
}
