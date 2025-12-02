using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Pages.Chamado
{
    public class CriarModel : PageModel
    {
        [BindProperty]
        public ChamadoInput Chamado { get; set; }

        public class ChamadoInput
        {
            [Required(ErrorMessage = "O título é obrigatório")]
            [StringLength(100, ErrorMessage = "O título deve ter no máximo {1} caracteres")]
            public string Titulo { get; set; }

            [Required(ErrorMessage = "A descrição é obrigatória")]
            [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
            public string Descricao { get; set; }

            [Required(ErrorMessage = "A categoria é obrigatória")]
            public string Categoria { get; set; }

            [Required(ErrorMessage = "A prioridade é obrigatória")]
            public string Prioridade { get; set; } = "Media";
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // TODO: Implementar lógica de salvar no banco de dados
            // Por enquanto, vamos simular um salvamento
            
            TempData["SuccessMessage"] = "Chamado aberto com sucesso!";
            return RedirectToPage("/Chamado/Index");
        }
    }
}