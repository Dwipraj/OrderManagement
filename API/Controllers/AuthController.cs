using API.Errors;
using Application.Interfaces;
using Core.Entities.DataTransfer;
using Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ITokenService _tokenService;

		public AuthController(ITokenService tokenService)
		{
			_tokenService = tokenService;
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			string username = loginDto.Email;
			if (username != "admin@mail.com" && username != "jhon.doe@mail.com" && username != "jane.smith@mail.com")
			{
				return Unauthorized(new ApiResponse(401, "No such user found"));
			}

			UserRoleType role = username == "admin@mail.com" ? UserRoleType.Admin : UserRoleType.User;

			return await Task.FromResult(new UserDto
			{
				FirstName = "Something",
				LastName = "Something",
				Email = username,
				UserRole = role,
				Token = _tokenService.CreateToken(username, role.ToString())
			});
		}
	}
}
