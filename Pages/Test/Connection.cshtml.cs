using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using THelp_Web.Models.API;
using THelp_Web.Services;

namespace THelp_Web.Pages.Test
{
    public class ConnectionModel : PageModel
    {
        private readonly ApiService _apiService;

        public ConnectionModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public ConnectionResult ConnectionResult { get; set; }
        public ApiStatus ApiStatus { get; set; }

        public async Task OnGetAsync()
        {
            ConnectionResult = await _apiService.TestConnectionAsync();
            ApiStatus = _apiService.GetApiStatus();
            ApiStatus.IsOnline = ConnectionResult.IsSuccess;
        }

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
    }
}