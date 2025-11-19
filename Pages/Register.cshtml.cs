using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace THelp_Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ILogger<RegisterModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O nome é obrigatório")]
            [Display(Name = "Nome completo")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
            [Display(Name = "Senha")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar senha")]
            [Compare("Password", ErrorMessage = "As senhas não conferem")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: salvar usuário no banco de dados aqui
            // Exemplo apenas registrando no log
            _logger.LogInformation("Novo usuário registrado: {Email}", Input.Email);

            // Exemplo simples: redireciona para Login depois de registrar
            // Se quiser ficar na mesma página com mensagem de sucesso:
            // SuccessMessage = "Usuário registrado com sucesso! Faça login para continuar.";
            // return Page();

            return RedirectToPage("/Login");
        }
    }
}
