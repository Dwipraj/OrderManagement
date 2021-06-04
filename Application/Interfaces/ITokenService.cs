using System.Security.Claims;

namespace Application.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(string username, string role = null);
		string GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}
