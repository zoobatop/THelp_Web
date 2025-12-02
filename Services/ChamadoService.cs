using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using THelp_Web.Models;

namespace THelp_Web.Services
{
    public class ChamadoService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly ILogger<ChamadoService> _logger;

        public ChamadoService(
            IHttpClientFactory httpClientFactory,
            AuthService authService,
            ILogger<ChamadoService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _authService = authService;
            _logger = logger;
        }

        private void AddAuthorizationHeader()
        {
            var token = _authService.GetUserToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ChamadoResponse?> GetAllChamadosAsync()
        {
            try
            {
                //AddAuthorizationHeader();

                var response = await _httpClient.GetAsync("chamados");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Resposta da API (todos chamados): {Content}", content);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    await LogErrorResponse(response, "Erro ao buscar todos os chamados");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar chamados");
                return null;
            }
        }

        public async Task<ChamadoResponse?> GetChamadoByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.GetAsync($"chamados/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Resposta da API (chamado por ID): {Content}", content);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Chamado com ID {Id} não encontrado", id);
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = "Chamado não encontrado"
                    };
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao buscar chamado ID {id}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar chamado por ID");
                return null;
            }
        }

        public async Task<ChamadoResponse?> CreateChamadoAsync(ChamadoCreateDto chamadoDto)
        {
            try
            {
                // Validações básicas
                if (string.IsNullOrWhiteSpace(chamadoDto.ChaTitulo))
                    throw new ArgumentException("Título do chamado é obrigatório");

                if (string.IsNullOrWhiteSpace(chamadoDto.ChaDescricao))
                    throw new ArgumentException("Descrição do chamado é obrigatória");

                if (chamadoDto.IdOrganizacao <= 0)
                    throw new ArgumentException("Organização é obrigatória");

                if (chamadoDto.IdUsuarioAbertura <= 0)
                    throw new ArgumentException("Usuário de abertura é obrigatório");

                //AddAuthorizationHeader();

                var requestData = new
                {
                    chaTitulo = chamadoDto.ChaTitulo,
                    chaDescricao = chamadoDto.ChaDescricao,
                    chaPrioridade = chamadoDto.Prioridade,
                    idOrganizacao = chamadoDto.IdOrganizacao,
                    idUsuarioAbertura = chamadoDto.IdUsuarioAbertura,
                    categoria = "aberto"
                };
                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("chamados", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Chamado criado com sucesso: {Content}", responseContent);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    await LogErrorResponse(response, "Erro ao criar chamado");
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = await GetErrorMessage(response)
                    };
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validação falhou: {Message}", ex.Message);
                return new ChamadoResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar chamado");
                return new ChamadoResponse
                {
                    Success = false,
                    Message = "Erro interno ao criar chamado"
                };
            }
        }

        public async Task<ChamadoResponse?> UpdateChamadoAsync(int id, ChamadoUpdateDto chamadoDto)
        {
            try
            {
                // Validação básica
                if (!string.IsNullOrEmpty(chamadoDto.ChaTitulo) && string.IsNullOrWhiteSpace(chamadoDto.ChaTitulo))
                    throw new ArgumentException("Título do chamado não pode ser vazio");

                AddAuthorizationHeader();

                var json = JsonSerializer.Serialize(chamadoDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"chamados/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Chamado atualizado com sucesso: {Content}", responseContent);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Chamado com ID {Id} não encontrado para atualização", id);
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = "Chamado não encontrado"
                    };
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao atualizar chamado ID {id}");
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = await GetErrorMessage(response)
                    };
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validação falhou: {Message}", ex.Message);
                return new ChamadoResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar chamado");
                return new ChamadoResponse
                {
                    Success = false,
                    Message = "Erro interno ao atualizar chamado"
                };
            }
        }

        public async Task<ChamadoResponse?> DeleteChamadoAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.DeleteAsync($"chamados/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Chamado deletado com sucesso: {Content}", responseContent);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Chamado com ID {Id} não encontrado para exclusão", id);
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = "Chamado não encontrado"
                    };
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao deletar chamado ID {id}");
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = await GetErrorMessage(response)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar chamado");
                return new ChamadoResponse
                {
                    Success = false,
                    Message = "Erro interno ao deletar chamado"
                };
            }
        }

        public async Task<ChamadoResponse?> AtribuirUsuarioAsync(int chamadoId, int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("ID do usuário é obrigatório");

                AddAuthorizationHeader();

                var response = await _httpClient.PatchAsync(
                    $"chamados/{chamadoId}/atribuir?idUsuario={usuarioId}",
                    null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Usuário atribuído com sucesso: {Content}", responseContent);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Chamado com ID {Id} não encontrado para atribuição", chamadoId);
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = "Chamado não encontrado"
                    };
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao atribuir usuário ao chamado {chamadoId}");
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = await GetErrorMessage(response)
                    };
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validação falhou: {Message}", ex.Message);
                return new ChamadoResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atribuir usuário ao chamado");
                return new ChamadoResponse
                {
                    Success = false,
                    Message = "Erro interno ao atribuir usuário"
                };
            }
        }

        public async Task<ChamadoResponse?> AtualizarStatusAsync(int chamadoId, string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                    throw new ArgumentException("Status é obrigatório");

                // Validação de status permitidos
                var statusPermitidos = new List<string>
                {
                    "aberto", "em_andamento", "pendente", "resolvido", "fechado", "cancelado"
                };

                if (!statusPermitidos.Contains(status.ToLower()))
                    throw new ArgumentException($"Status inválido. Valores permitidos: {string.Join(", ", statusPermitidos)}");

                AddAuthorizationHeader();

                var response = await _httpClient.PatchAsync(
                    $"chamados/{chamadoId}/status?status={Uri.EscapeDataString(status)}",
                    null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Status atualizado com sucesso: {Content}", responseContent);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Chamado com ID {Id} não encontrado para atualização de status", chamadoId);
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = "Chamado não encontrado"
                    };
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao atualizar status do chamado {chamadoId}");
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = await GetErrorMessage(response)
                    };
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validação falhou: {Message}", ex.Message);
                return new ChamadoResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar status do chamado");
                return new ChamadoResponse
                {
                    Success = false,
                    Message = "Erro interno ao atualizar status"
                };
            }
        }

        public async Task<ChamadoResponse?> GetChamadosPorStatusAsync(string status)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.GetAsync($"chamados/status/{Uri.EscapeDataString(status)}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Chamados por status {Status}: {Content}", status, content);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao buscar chamados por status {status}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar chamados por status");
                return null;
            }
        }

        public async Task<ChamadoResponse?> GetChamadosPorOrganizacaoAsync(int idOrganizacao)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.GetAsync($"chamados/organizacao/{idOrganizacao}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Chamados por organização {Id}: {Content}", idOrganizacao, content);

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao buscar chamados por organização {idOrganizacao}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar chamados por organização");
                return null;
            }
        }

        public async Task<ChamadoResponse?> GetChamadosDoUsuarioAtualAsync()
        {
            try
            {
                var usuario = _authService.GetCurrentUser();
                if (usuario == null)
                {
                    _logger.LogWarning("Usuário não autenticado ao buscar chamados");
                    return new ChamadoResponse
                    {
                        Success = false,
                        Message = "Usuário não autenticado"
                    };
                }

                // Busca chamados abertos pelo usuário
                var response = await _httpClient.GetAsync($"chamados?usuario={usuario.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<ChamadoResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    await LogErrorResponse(response, $"Erro ao buscar chamados do usuário {usuario.Id}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar chamados do usuário atual");
                return null;
            }
        }

        // Métodos auxiliares
        private async Task LogErrorResponse(HttpResponseMessage response, string mensagem)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("{Mensagem}: {StatusCode} - {Content}",
                mensagem, response.StatusCode, errorContent);
        }

        private async Task<string> GetErrorMessage(HttpResponseMessage response)
        {
            try
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<ChamadoResponse>(
                    errorContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return errorResponse?.Message ?? "Erro na requisição";
            }
            catch
            {
                return "Erro na requisição";
            }
        }
    }
}