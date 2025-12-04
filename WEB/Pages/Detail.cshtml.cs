using belanjaYuk.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace belanjaYuk.WEB.Pages
{
    public class DetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public ProductDetailViewModel Product { get; set; }

        // Parameter 'id' harus cocok dengan 'asp-route-id' dari link
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Index");
            }

            var client = _httpClientFactory.CreateClient("BelanjaYukApiClient");

            try
            {
                // Panggil endpoint API baru: api/v1/products/{id}
                Product = await client.GetFromJsonAsync<ProductDetailViewModel>($"api/v1/products/{id}");

                if (Product == null)
                {
                    return Page(); // Akan menampilkan "Produk Tidak Ditemukan" di UI
                }

                return Page(); // Sukses, tampilkan halaman dengan data produk
            }
            catch (HttpRequestException ex)
            {
                // Tangani jika API mengembalikan 404 (Not Found) atau 500
                // (Anda bisa menambahkan logging di sini)
                Product = null; // Pastikan produk null agar UI menampilkan pesan error
                return Page();
            }
        }
    }
}
