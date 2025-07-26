using System.ComponentModel.DataAnnotations;

namespace GastosAPI.Models
{
	public class Gasto
	{
		public int Id { get; set; }

		[Required]
		public string Descricao { get; set; }

		[Required]
		public decimal Valor { get; set; }

		[Required]
		public DateTime Data { get; set; }

		public string Categoria { get; set; }
	}
}
