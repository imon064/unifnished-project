using belanjaYuk.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace belanjaYuk.WEB.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public RegisterViewModel RegisterInput { get; set; }

        public void OnGet()
        {
            // Hanya menampilkan halaman
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Kembalikan halaman dengan error validasi
            }

            var client = _httpClientFactory.CreateClient("BelanjaYukApiClient");

            // Kirim data registrasi ke API
            var response = await client.PostAsJsonAsync("api/v1/auth/register", RegisterInput);

            if (response.IsSuccessStatusCode)
            {
                // Jika sukses, arahkan ke halaman Login dengan pesan sukses
                TempData["SuccessMessage"] = "Registrasi berhasil! Silakan masuk.";
                return RedirectToPage("/Login");
            }
            else
            {
                // Jika API mengembalikan error (misal: email sudah ada)
                string apiError = await response.Content.ReadAsStringAsync();

                // Coba deserialisasi error dari API
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<JsonElement>(apiError);
                    if (errorResponse.TryGetProperty("message", out var message))
                    {
                        ModelState.AddModelError(string.Empty, message.GetString());
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Terjadi kesalahan saat registrasi.");
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Terjadi kesalahan: " + response.ReasonPhrase);
                }

                return Page();
            }
        }
    }
}
