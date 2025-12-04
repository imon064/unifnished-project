using System.Text.Json.Serialization;

namespace belanjaYuk.WEB.Models
{
    public class ProductViewModel
    {
        public string IdProduct { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string? CategoryName { get; set; } 
        public string? StoreName { get; set; }
    }
}

