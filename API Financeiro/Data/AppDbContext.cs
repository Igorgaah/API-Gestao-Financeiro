using GastosAPI.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Gasto> Gastos { get; set; }
	public DbSet<Usuario> Usuarios { get; set; }
}
