namespace belanjaYuk.WEB.Models
{
    public class ProductDetailViewModel
    {
        public string IdProduct { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? StoreName { get; set; }
    }
}

