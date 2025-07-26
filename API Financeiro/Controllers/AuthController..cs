using GastosAPI.Data;
using GastosAPI.Models;
using GastosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GastosAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly TokenService _tokenService;

		public AuthController(AppDbContext context, TokenService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] Usuario novoUsuario)
		{
			if (await _context.Usuarios.AnyAsync(u => u.Username == novoUsuario.Username))
				return BadRequest("Usuário já existe.");

			novoUsuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(novoUsuario.PasswordHash);
			novoUsuario.Role = novoUsuario.Role ?? "user";

			await _context.Usuarios.AddAsync(novoUsuario);
			await _context.SaveChangesAsync();

			return Ok("Usuário registrado com sucesso.");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserModel login)
		{
			var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == login.Username);
			if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.PasswordHash))
				return Unauthorized("Credenciais inválidas.");

			var accessToken = _tokenService.CreateToken(usuario);
			var refreshToken = _tokenService.GenerateRefreshToken();

			usuario.RefreshToken = refreshToken;
			usuario.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				token = accessToken,
				refreshToken = refreshToken
			});
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] string refreshToken)
		{
			var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
			if (usuario == null || usuario.RefreshTokenExpiryTime < DateTime.Now)
				return Unauthorized("Token inválido ou expirado.");

			var newAccessToken = _tokenService.CreateToken(usuario);
			var newRefreshToken = _tokenService.GenerateRefreshToken();

			usuario.RefreshToken = newRefreshToken;
			usuario.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				token = newAccessToken,
				refreshToken = newRefreshToken
			});
		}
	}
}
