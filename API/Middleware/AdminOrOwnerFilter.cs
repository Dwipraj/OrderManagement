using Application.Interfaces;
using Core.Enums;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Middleware
{
	public class AdminOrOwnerFilter : IAsyncActionFilter
	{
		private readonly IUnitOfWork _unitOfWork;

		public AdminOrOwnerFilter(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var role = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
			UserRoleType currentUserRole = (UserRoleType)Enum.Parse(typeof(UserRoleType), role);
			if (currentUserRole != UserRoleType.Admin)
			{
				var currentUsername = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
				_unitOfWork.SetUsername(currentUsername);
			}
			await next();
		}
	}
}
