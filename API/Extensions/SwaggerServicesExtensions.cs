﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace API.Extensions
{
	public static class SwaggerServicesExtensions
	{
		public static IServiceCollection SwaggerServices(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", //Name the security scheme
				new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme.",
					Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
					Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme{
							Reference = new OpenApiReference{
								Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
							}
						},new List<string>()
					}
				});
			});

			return services;
		}
	}
}
