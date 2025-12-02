using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using THelp_Web.Services;

namespace THelp_Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public InputModel Input { get; set; }

        public LoginModel(AuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "O e-mail é obrigatório")]
            [EmailAddress(ErrorMessage = "Digite um e-mail válido")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória")]
            [DataType(DataType.Password)]
            public string Senha { get; set; } = string.Empty;

            public bool RememberMe { get; set; }
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

            _logger.LogInformation("Tentativa de login para: {Email}", Input.Email);

            // Usa o AuthService para fazer login na API
            var apiResponse = await _authService.LoginAsync(Input.Email, Input.Senha);

            if (apiResponse?.Success == true && apiResponse.Data?.Token != null)
            {
                var loginData = apiResponse.Data;
                var usuario = loginData.Usuario;

                _logger.LogInformation("Login bem-sucedido para: {Nome}", usuario?.Nome);

                // Cria as claims do usuário
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario?.Id.ToString() ?? "0"),
                    new Claim(ClaimTypes.Name, usuario?.Nome ?? Input.Email),
                    new Claim(ClaimTypes.Email, Input.Email),
                    new Claim("PapelId", usuario?.IdPapel.ToString() ?? "0"),
                    new Claim("OrganizacaoId", usuario?.IdOrganizacao.ToString() ?? "0"),
                    new Claim("JwtToken", loginData.Token)
                };

                // Adiciona role baseada no papel
                if (usuario?.IdPapel == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Input.RememberMe,
                    ExpiresUtc = DateTimeOffset.FromUnixTimeMilliseconds(loginData.ExpiraEm),
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                TempData["SuccessMessage"] = "Login realizado com sucesso!";
                return RedirectToPage("/Chamado/Index");
            }

            _logger.LogWarning("Login falhou para: {Email}", Input.Email);

            // Tenta pegar a mensagem de erro da API
            var errorMessage = apiResponse?.Message ?? "E-mail ou senha inválidos.";
            ModelState.AddModelError(string.Empty, errorMessage);

            return Page();
        }

        public async Task<IActionResult> OnGetLogout()
        {
            _authService.Logout();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["InfoMessage"] = "Você foi desconectado com sucesso.";
            return RedirectToPage("/Welcome");
        }
    }
}