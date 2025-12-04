using BelanjaYukAPI.Data;
using BelanjaYukAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BelanjaYukAPI.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.MsProducts
                                .Where(p => p.IsActive == true)
                                .Include(p => p.IdCategoryNavigation)
                                .Include(p => p.IdUserSellerNavigation)
                                .Select(p => new {
                                    p.IdProduct,
                                    p.ProductName,
                                    Price = p.Price ?? 0, 
                                    Discount = p.Discount ?? 0, 
                                    CategoryName = p.IdCategoryNavigation.CategoryName,
                                    StoreName = p.IdUserSellerNavigation.StoreName
                                })
                                .ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound("Tidak ada produk yang ditemukan.");
            }

            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID Produk tidak boleh kosong.");
            }

            var product = await _context.MsProducts
                                .Where(p => p.IdProduct == id && p.IsActive == true)
                                .Include(p => p.IdCategoryNavigation)
                                .Include(p => p.IdUserSellerNavigation)
                                .Select(p => new ProductDetailDto
                                {
                                    IdProduct = p.IdProduct,
                                    ProductName = p.ProductName,
                                    ProductDesc = p.ProductDesc,
                                    Price = p.Price ?? 0, 
                                    CategoryName = p.IdCategoryNavigation.CategoryName,
                                    StoreName = p.IdUserSellerNavigation.StoreName
                                })
                                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { Message = "Produk tidak ditemukan." });
            }

            return Ok(product);
        }
    }
}

