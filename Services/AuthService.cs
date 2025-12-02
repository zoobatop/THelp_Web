using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace THelp_Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IHttpClientFactory httpClientFactory,
                          IHttpContextAccessor httpContextAccessor,
                          ILogger<AuthService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<LoginResponse>?> LoginAsync(string email, string senha)
        {
            try
            {
                var loginRequest = new { email, senha };
                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Resposta da API: {Response}", responseContent);

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginResponse>>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Success == true && apiResponse.Data?.Token != null)
                    {
                        // Salva o token e dados do usuário na sessão
                        SaveUserSession(apiResponse.Data);
                        return apiResponse;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Erro na API: {StatusCode} - {Content}",
                        response.StatusCode, errorContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no login");
                return null;
            }
        }

        public async Task<ApiResponse<RegisterResponse>?> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var contentRequest = new
                {
                    nome = registerRequest.Nome,
                    email = registerRequest.Email,
                    confirmarSenha = registerRequest.ConfirmarSenha,
                    senha = registerRequest.Senha
                };
                var json = JsonSerializer.Serialize(contentRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Enviando requisição de registro: {Email}", registerRequest.Email);

                var response = await _httpClient.PostAsync("auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Resposta do registro: {Response}", responseContent);

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<RegisterResponse>>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Success == true && apiResponse.Data?.Token != null)
                    {
                        // Salva o token e dados do usuário na sessão (login automático após registro)
                        SaveUserSession(apiResponse.Data);
                        return apiResponse;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Erro no registro: {StatusCode} - {Content}",
                        response.StatusCode, errorContent);

                    // Tenta deserializar a resposta de erro
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(
                            errorContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (errorResponse != null)
                        {
                            return new ApiResponse<RegisterResponse>
                            {
                                Success = false,
                                Message = errorResponse.Message
                            };
                        }
                    }
                    catch
                    {
                        // Se não conseguir deserializar, usa o conteúdo bruto
                    }
                }

                return new ApiResponse<RegisterResponse>
                {
                    Success = false,
                    Message = "Erro ao processar o registro. Tente novamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no registro");
                return new ApiResponse<RegisterResponse>
                {
                    Success = false,
                    Message = "Erro de conexão. Verifique sua internet e tente novamente."
                };
            }
        }

        public async Task<ApiResponse<RegisterResponse>?> RegisterAsync(string nome, string email, string senha, string confirmarSenha)
        {
            var registerRequest = new RegisterRequest
            {
                Nome = nome,
                Email = email,
                Senha = senha,
                ConfirmarSenha = confirmarSenha
            };

            return await RegisterAsync(registerRequest);
        }

        private void SaveUserSession(LoginResponse loginData)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            // Salva o token JWT
            httpContext.Session.SetString("JwtToken", loginData.Token);

            // Salva informações do usuário
            if (loginData.Usuario != null)
            {
                httpContext.Session.SetString("UserId", loginData.Usuario.Id.ToString());
                httpContext.Session.SetString("UserName", loginData.Usuario.Nome);
                httpContext.Session.SetString("UserEmail", loginData.Usuario.Email);
                httpContext.Session.SetString("UserPapelId", loginData.Usuario.IdPapel.ToString());
                httpContext.Session.SetString("UserOrganizacaoId", loginData.Usuario.IdOrganizacao.ToString());
            }

            // Salva a data de expiração
            httpContext.Session.SetString("TokenExpiration", loginData.ExpiraEm.ToString());
        }

        private void SaveUserSession(RegisterResponse registerData)
        {
            if (registerData?.Usuario == null || string.IsNullOrEmpty(registerData.Token))
                return;

            var loginResponse = new LoginResponse
            {
                Token = registerData.Token,
                Usuario = registerData.Usuario,
                ExpiraEm = registerData.ExpiraEm
            };

            SaveUserSession(loginResponse);
        }

        public void Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.Clear();
            }
        }

        public bool IsAuthenticated()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            var expirationStr = _httpContextAccessor.HttpContext?.Session.GetString("TokenExpiration");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expirationStr))
                return false;

            // Verifica se o token ainda não expirou
            if (long.TryParse(expirationStr, out var expirationMs))
            {
                var expirationDate = DateTimeOffset.FromUnixTimeMilliseconds(expirationMs);
                return expirationDate > DateTimeOffset.UtcNow;
            }

            return false;
        }

        public string? GetUserToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        }

        public UsuarioDto? GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            var userId = httpContext.Session.GetString("UserId");
            var userName = httpContext.Session.GetString("UserName");
            var userEmail = httpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userId)) return null;

            return new UsuarioDto
            {
                Id = int.Parse(userId),
                Nome = userName ?? string.Empty,
                Email = userEmail ?? string.Empty,
                IdPapel = int.Parse(httpContext.Session.GetString("UserPapelId") ?? "0"),
                IdOrganizacao = int.Parse(httpContext.Session.GetString("UserOrganizacaoId") ?? "0")
            };
        }
    }

    // Classes de modelo
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public long? Timestamp { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto? Usuario { get; set; }
        public long ExpiraEm { get; set; }
    }

    public class RegisterResponse
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public string Token { get; set; } = string.Empty;
        public long ExpiraEm { get; set; }
    }

    public class RegisterRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Senha { get; set; }
        public bool Ativo { get; set; }
        public int IdPapel { get; set; }
        public int IdOrganizacao { get; set; }
    }
}