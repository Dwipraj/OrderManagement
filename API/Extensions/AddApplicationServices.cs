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

			services.AddScoped<ILogicalErrorMessage, LogicalErrorMessage>();

			services.AddScoped<IOrderService, OrderService>();

			return services;
		}
	}
}
