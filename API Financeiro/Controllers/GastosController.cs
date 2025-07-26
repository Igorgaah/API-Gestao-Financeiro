using GastosAPI.Data;
using GastosAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GastosAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class GastosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public GastosController(AppDbContext context)
		{
			_context = context;
		}

		// GET com filtros opcionais
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Gasto>>> Get(
			[FromQuery] string? categoria,
			[FromQuery] DateTime? dataInicial,
			[FromQuery] DateTime? dataFinal)
		{
			var query = _context.Gastos.AsQueryable();

			if (!string.IsNullOrEmpty(categoria))
				query = query.Where(g => g.Categoria == categoria);

			if (dataInicial.HasValue)
				query = query.Where(g => g.Data >= dataInicial.Value);

			if (dataFinal.HasValue)
				query = query.Where(g => g.Data <= dataFinal.Value);

			return await query.ToListAsync();
		}

		// GET por ID
		[HttpGet("{id}")]
		public async Task<ActionResult<Gasto>> Get(int id)
		{
			var gasto = await _context.Gastos.FindAsync(id);
			return gasto == null ? NotFound() : gasto;
		}

		// POST
		[HttpPost]
		public async Task<ActionResult<Gasto>> Post(Gasto gasto)
		{
			_context.Gastos.Add(gasto);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(Get), new { id = gasto.Id }, gasto);
		}

		// PUT
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, Gasto gasto)
		{
			if (id != gasto.Id) return BadRequest();
			_context.Entry(gasto).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var gasto = await _context.Gastos.FindAsync(id);
			if (gasto == null) return NotFound();
			_context.Gastos.Remove(gasto);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// Exportar CSV
		[HttpGet("exportar")]
		public async Task<IActionResult> ExportarCsv(
			[FromQuery] string? categoria,
			[FromQuery] DateTime? dataInicial,
			[FromQuery] DateTime? dataFinal)
		{
			var query = _context.Gastos.AsQueryable();

			if (!string.IsNullOrEmpty(categoria))
				query = query.Where(g => g.Categoria == categoria);

			if (dataInicial.HasValue)
				query = query.Where(g => g.Data >= dataInicial.Value);

			if (dataFinal.HasValue)
				query = query.Where(g => g.Data <= dataFinal.Value);

			var gastos = await query.ToListAsync();

			var linhas = new List<string>
			{
				"Id,Descricao,Valor,Data,Categoria"
			};

			linhas.AddRange(gastos.Select(g =>
				$"{g.Id},\"{g.Descricao}\",{g.Valor},{g.Data:yyyy-MM-dd},{g.Categoria}"));

			var csv = string.Join("\n", linhas);
			var bytes = System.Text.Encoding.UTF8.GetBytes(csv);

			return File(bytes, "text/csv", "gastos_exportados.csv");
		}
	}
}
