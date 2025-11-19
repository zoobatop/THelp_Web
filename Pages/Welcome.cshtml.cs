using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace THelp_Web.Pages
{
    public class WelcomeModel : PageModel
    {
        private readonly ILogger<WelcomeModel> _logger;

        public WelcomeModel(ILogger<WelcomeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Welcome page accessed.");
        }
    }
}
