using belanjaYuk.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace belanjaYuk.WEB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        // This property will hold the list of products to display on the page
        // The @foreach loop in your Index.cshtml file will use this.
        public List<ProductViewModel>? Products { get; private set; }

        // This property will be true if we can't connect to the API
        // The @if (Model.IsApiOffline) check in your Index.cshtml uses this.
        public bool IsApiOffline { get; private set; } = false;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // This method runs automatically when the page is loaded
        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Get the pre-configured HttpClient we set up in Program.cs
            var httpClient = _httpClientFactory.CreateClient("BelanjaYukApiClient");

            try
            {
                // 2. Call the API endpoint (this path matches the API project's ProductsController)
                // The BaseAddress (e.g., https://localhost:7123) is already in the client.
                var response = await httpClient.GetAsync("/api/v1/products");

                if (response.IsSuccessStatusCode)
                {
                    // 3. Read the JSON string from the response
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // 4. Deserialize the JSON into our C# ProductViewModel list
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Matches "idProduct" to "IdProduct"
                    };
                    Products = JsonSerializer.Deserialize<List<ProductViewModel>>(jsonString, options);
                }
                else
                {
                    // API is running but gave an error (e.g., 404 Not Found, 500 Server Error)
                    _logger.LogWarning("API call failed with status code: {StatusCode}", response.StatusCode);
                    Products = new List<ProductViewModel>(); // Set to an empty list
                }
            }
            catch (Exception ex)
            {
                // This 'catch' block runs if the API project isn't running
                // or if the BaseAddress URL in Program.cs is wrong.
                _logger.LogError(ex, "Failed to connect to API backend.");
                IsApiOffline = true; // Set this flag so the page can show a helpful error
                Products = new List<ProductViewModel>();
            }

            return Page();
        }
    }
}

