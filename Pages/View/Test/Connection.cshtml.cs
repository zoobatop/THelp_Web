using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using THelp_Web.Models.API;
using THelp_Web.Services;

namespace THelp_Web.Pages.View.Test
{
    public class ConnectionModel : PageModel
    {
        private readonly ApiService _apiService;

        public ConnectionModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Propriedades para a view
        public ConnectionResult ConnectionResult { get; set; }
        public ApiStatus ApiStatus { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Testa a conexão quando a página é carregada
            ConnectionResult = await _apiService.TestConnectionAsync();
            ApiStatus = _apiService.GetApiStatus();
            ApiStatus.IsOnline = ConnectionResult.IsSuccess;

            return Page();
        }

        // Handler para requisições AJAX
        public async Task<JsonResult> OnPostCheckStatusAsync()
        {
            var isOnline = await _apiService.PingAsync();
            var status = _apiService.GetApiStatus();
            status.IsOnline = isOnline;

            return new JsonResult(new
            {
                success = isOnline,
                status = status,
                message = isOnline ? "API online!" : "API offline!"
            });
        }

        // Handler para testar URL específica
        public async Task<IActionResult> OnPostTestUrlAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new JsonResult(new { success = false, message = "URL não informada" });
            }

            try
            {
                var tempHttpClient = new HttpClient();
                var response = await tempHttpClient.GetAsync(url);

                return new JsonResult(new
                {
                    success = response.IsSuccessStatusCode,
                    statusCode = (int)response.StatusCode,
                    message = $"Status: {response.StatusCode}"
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = $"Erro: {ex.Message}"
                });
            }
        }
    }
}
