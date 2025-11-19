using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace THelp_Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // Exemplo de propriedades para exibir no dashboard
        public int OpenTickets { get; set; }
        public int InProgressTickets { get; set; }
        public int ClosedTodayTickets { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Dashboard accessed.");

            OpenTickets = 12;
            InProgressTickets = 5;
            ClosedTodayTickets = 3;
        }
    }
}
