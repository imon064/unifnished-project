namespace BelanjaYukAPI.Models
{
	public class ProductDetailDto
	{
		public string IdProduct { get; set; }
		public string ProductName { get; set; }
		public string ProductDesc { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; } 
		public string? CategoryName { get; set; }
		public string? StoreName { get; set; }
	}
}

