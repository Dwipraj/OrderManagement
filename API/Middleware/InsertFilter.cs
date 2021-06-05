using API.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Middleware
{
	public class InsertFilter : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.Result != null) return;
			var currentUsername = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

			var insertDto = context.ActionArguments.SingleOrDefault(p => p.Value is IBaseInsert);
			if (insertDto.Value == null)
			{
				throw new ArgumentException("Base class doesn't match.");
			}

			//Approach 1 : Set email of the Request Body from JWT token username claim
			//((IBaseInsert)insertDto.Value).Email = currentUsername;
			//await next();
			//END of Approach 1

			//Approach 2 : Return the request as a Unauthorized Request
			if (((IBaseInsert)insertDto.Value).Email != currentUsername)
			{
				context.Result = await Task.FromResult(new UnauthorizedObjectResult(new ApiResponse(401, "Not a valid email.")));
			}
			else
			{
				await next();
			}
			//END of Approach 2
		}
	}
}
