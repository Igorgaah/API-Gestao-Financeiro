using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GastosAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace GastosAPI.Services
{
	public class TokenService
	{
		private readonly IConfiguration _config;
		public TokenService(IConfiguration config) => _config = config;

		public string CreateToken(Usuario usuario)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, usuario.Username),
				new Claim(ClaimTypes.Role, usuario.Role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(15),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
}
