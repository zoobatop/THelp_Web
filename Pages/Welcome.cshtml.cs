using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace THelp_Web.Pages
{
    public class WelcomeModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Se o usuário já estiver autenticado, redireciona para a página de chamados
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Chamado/Index");
            }
            return Page();
        }
    }
}