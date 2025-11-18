using Microsoft.AspNetCore.Mvc;
using THelp_Web.Services;

namespace THelp_Web.Controllers
{
    public class TestController : Controller
    {
        private readonly ApiService _apiService;

        public TestController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Connection()
        {
            var result = await _apiService.TestConnectionAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<JsonResult> CheckStatus()
        {
            var isOnline = await _apiService.PingAsync();
            var status = _apiService.GetApiStatus();
            status.IsOnline = isOnline;

            return Json(new
            {
                success = isOnline,
                status = status,
                message = isOnline ? "API online!" : "API offline!"
            });
        }
    }
}