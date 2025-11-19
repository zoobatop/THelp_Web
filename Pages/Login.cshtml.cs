using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace THelp_Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "E-mail inválido")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Lembrar-me")]
            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {
            // Se precisar limpar sessão/cookies de login, pode fazer aqui
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Validar usuário em banco/serviço aqui
            // Exemplo fictício:
            if (Input.Email == "admin@teste.com" && Input.Password == "123456")
            {
                _logger.LogInformation("Usuário logado com sucesso: {Email}", Input.Email);

                // Aqui você deveria criar o cookie de autenticação ou algo similar
                // Por enquanto, só redireciono para o dashboard
                return RedirectToPage("/Index");
            }

            ErrorMessage = "E-mail ou senha inválidos.";
            _logger.LogWarning("Tentativa de login falhou: {Email}", Input.Email);

            return Page();
        }
    }
}
