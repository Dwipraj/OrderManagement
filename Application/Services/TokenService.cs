using Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
	public class TokenService : ITokenService
	{
		private readonly SymmetricSecurityKey _key;
		private readonly string _issuer;

		public TokenService(string key, string issuer)
		{
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			_issuer = issuer;

		}

		public string CreateToken(string username, string role = null)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, username)
			};

			if (!string.IsNullOrEmpty(role))
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddMinutes(45),
				SigningCredentials = cred,
				Issuer = _issuer
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = _key,
				ValidateLifetime = false
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}
