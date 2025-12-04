using belanjaYuk.WEB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace belanjaYuk.WEB.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginViewModel LoginInput { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("BelanjaYukApiClient");
            var response = await client.PostAsJsonAsync("api/v1/auth/login", LoginInput);

            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(jsonString, options);


                if (loginResponse == null || string.IsNullOrEmpty(loginResponse.UserId))
                {
                    ErrorMessage = "Gagal memproses data login dari API.";
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId), 
                    new Claim(ClaimTypes.Name, loginResponse.UserName), 
                    new Claim(ClaimTypes.Email, loginResponse.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = LoginInput.RememberMe, 
                    ExpiresUtc = System.DateTimeOffset.UtcNow.AddDays(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToPage("/Index");
            }
            else
            {
                try
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<object>();
                    ErrorMessage = errorResponse?.ToString() ?? "Login gagal. Periksa email/password Anda.";
                }
                catch
                {
                    ErrorMessage = "Login gagal. Periksa email/password Anda.";
                }
                return Page();
            }
        }
    }
}

