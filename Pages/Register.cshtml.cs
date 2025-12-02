using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using THelp_Web.Services;

namespace THelp_Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public InputModel Input { get; set; }

        public RegisterModel(AuthService authService, ILogger<RegisterModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "O nome é obrigatório")]
            [Display(Name = "Nome Completo")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
            public string Nome { get; set; } = string.Empty;

            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "Digite um e-mail válido")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
                ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial")]
            public string Senha { get; set; } = string.Empty;

            [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Senha")]
            [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
            public string ConfirmarSenha { get; set; } = string.Empty;

        }

        public IActionResult OnGet()
        {
            if (_authService.IsAuthenticated())
            {
                return RedirectToPage("/Chamado/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _logger.LogInformation("Tentativa de registro para: {Email}", Input.Email);

            // Usa o AuthService para fazer registro na API
            var apiResponse = await _authService.RegisterAsync(
                Input.Nome,
                Input.Email,
                Input.Senha,
                Input.ConfirmarSenha);

            if (apiResponse?.Success == true && apiResponse.Data?.Sucesso == true)
            {
                var registerData = apiResponse.Data;
                var usuario = registerData.Usuario;

                _logger.LogInformation("Registro bem-sucedido para: {Nome}", usuario?.Nome);

                // Se o registro inclui login automático (com token), cria as claims
                if (!string.IsNullOrEmpty(registerData.Token) && usuario != null)
                {
                    // Cria as claims do usuário
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Nome ?? Input.Nome),
                        new Claim(ClaimTypes.Email, Input.Email),
                        new Claim("PapelId", usuario.IdPapel.ToString()),
                        new Claim("OrganizacaoId", usuario.IdOrganizacao.ToString()),
                        new Claim("JwtToken", registerData.Token)
                    };

                    // Adiciona role baseada no papel
                    claims.Add(new Claim(ClaimTypes.Role, "Usuario"));

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.FromUnixTimeMilliseconds(registerData.ExpiraEm),
                        IssuedUtc = DateTimeOffset.UtcNow
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    TempData["SuccessMessage"] = "Conta criada com sucesso! Você está logado.";
                    return RedirectToPage("/Chamado/Index");
                }
                else
                {
                    TempData["SuccessMessage"] = "Conta criada com sucesso! Faça login para continuar.";
                    return RedirectToPage("/Login");
                }
            }

            _logger.LogWarning("Registro falhou para: {Email}", Input.Email);

            // Usa a mensagem da API ou uma mensagem padrão
            var errorMessage = apiResponse?.Message ?? "Erro ao criar a conta. Verifique os dados e tente novamente.";
            ModelState.AddModelError(string.Empty, errorMessage);

            return Page();
        }
    }
}