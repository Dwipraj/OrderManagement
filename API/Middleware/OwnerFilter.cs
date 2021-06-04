using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Middleware
{
	public class OwnerFilter : IAsyncActionFilter
	{
		private readonly IUnitOfWork _unitOfWork;

		public OwnerFilter(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var currentUsername = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
			_unitOfWork.SetUsername(currentUsername);
			await next();
		}
	}
}
