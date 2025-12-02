using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using THelp_Web.Services;
using THelp_Web.Models;
using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Pages.Chamado
{
    public class IndexModel : PageModel
    {
        private readonly ChamadoService _chamadoService;
        private readonly AuthService _authService;
        private readonly ILogger<IndexModel> _logger;

        public List<ChamadoDto>? Chamados { get; set; }
        public string? MensagemErro { get; set; }
        public string? MensagemSucesso { get; set; }

        // Propriedades para os métodos POST
        [BindProperty]
        public int ChamadoId { get; set; }

        [BindProperty]
        public string? Status { get; set; }

        [BindProperty]
        public int UsuarioId { get; set; }

        [BindProperty]
        public ChamadoCreateDto NovoChamado { get; set; } = new();

        [BindProperty]
        public ChamadoUpdateDto ChamadoParaAtualizar { get; set; } = new();

        public IndexModel(
            ChamadoService chamadoService,
            AuthService authService,
            ILogger<IndexModel> logger)
        {
            _chamadoService = chamadoService;
            _authService = authService;
            _logger = logger;
            Chamados = new List<ChamadoDto>();
        }

        public async Task<IActionResult> OnGetAsync(string? status = null)
        {
            try
            {
                // Verifica se o usuário está autenticado
                if (!_authService.IsAuthenticated())
                {
                    return RedirectToPage("/Login");
                }

                if (!string.IsNullOrEmpty(status))
                {
                    // Filtra por status
                    var response = await _chamadoService.GetChamadosPorStatusAsync(status);

                    if (response?.Success == true)
                    {
                        Chamados = response.Data ?? new List<ChamadoDto>();
                        MensagemSucesso = response.Message;
                    }
                    else
                    {
                        Chamados = new List<ChamadoDto>();
                        MensagemErro = response?.Message ?? "Nenhum chamado encontrado com este status";
                    }
                }
                else
                {
                    // Busca todos os chamados
                    var response = await _chamadoService.GetAllChamadosAsync();

                    if (response?.Success == true)
                    {
                        Chamados = response.Data ?? new List<ChamadoDto>();
                        MensagemSucesso = response.Message;
                    }
                    else
                    {
                        MensagemErro = response?.Message ?? "Erro ao carregar chamados";
                        Chamados = new List<ChamadoDto>();
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar chamados");
                MensagemErro = "Erro interno ao carregar chamados";
                Chamados = new List<ChamadoDto>();
                return Page();
            }
        }

        // Método para criar novo chamado
        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                // Valida se o usuário está autenticado
                var usuario = _authService.GetCurrentUser();
                if (usuario == null)
                {
                    TempData["ErrorMessage"] = "Usuário não autenticado";
                    return RedirectToPage("/Login");
                }

                // Define o usuário que está criando o chamado
                NovoChamado.IdUsuarioAbertura = usuario.Id;
                NovoChamado.IdOrganizacao = usuario.IdOrganizacao;
                // Não envie IdOrganizacao - o backend vai pegar do usuário

                if (!ModelState.IsValid)
                {
                    // Recarrega os chamados para mostrar erros
                    await OnGetAsync();
                    return Page();
                }

                var response = await _chamadoService.CreateChamadoAsync(NovoChamado);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = response.Message;
                    // Limpa o formulário
                    NovoChamado = new ChamadoCreateDto();
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao criar chamado";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar chamado");
                TempData["ErrorMessage"] = "Erro interno ao criar chamado";
                return RedirectToPage();
            }
        }

        // Método para atribuir usuário (usando query string)
        public async Task<IActionResult> OnPostAtribuirUsuarioAsync([FromQuery] int chamadoId)
        {
            try
            {
                if (chamadoId <= 0 || UsuarioId <= 0)
                {
                    TempData["ErrorMessage"] = "Dados inválidos para atribuição";
                    return RedirectToPage();
                }

                var response = await _chamadoService.AtribuirUsuarioAsync(chamadoId, UsuarioId);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = response.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao atribuir usuário";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atribuir usuário");
                TempData["ErrorMessage"] = "Erro interno ao atribuir usuário";
                return RedirectToPage();
            }
        }

        // Método para atualizar chamado (usando query string)
        public async Task<IActionResult> OnPostUpdateAsync([FromQuery] int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "ID do chamado inválido";
                    return RedirectToPage();
                }

                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Dados inválidos para atualização";
                    return RedirectToPage();
                }

                var response = await _chamadoService.UpdateChamadoAsync(id, ChamadoParaAtualizar);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = response.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao atualizar chamado";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar chamado");
                TempData["ErrorMessage"] = "Erro interno ao atualizar chamado";
                return RedirectToPage();
            }
        }

        // Método para deletar chamado
        public async Task<IActionResult> OnPostDeleteAsync([FromQuery] int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "ID do chamado inválido";
                    return RedirectToPage();
                }

                var response = await _chamadoService.DeleteChamadoAsync(id);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = response.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao excluir chamado";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir chamado");
                TempData["ErrorMessage"] = "Erro interno ao excluir chamado";
                return RedirectToPage();
            }
        }

        // Método para atualizar status (versão com parâmetros na query string)
        public async Task<IActionResult> OnPostAtualizarStatusAsync([FromQuery] int id, [FromQuery] string status)
        {
            try
            {
                if (id <= 0 || string.IsNullOrEmpty(status))
                {
                    TempData["ErrorMessage"] = "Dados inválidos para atualização";
                    return RedirectToPage();
                }

                var response = await _chamadoService.AtualizarStatusAsync(id, status);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = response.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao atualizar status";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar status");
                TempData["ErrorMessage"] = "Erro interno ao atualizar status";
                return RedirectToPage();
            }
        }

        // Métodos auxiliares para o HTML
        public string GetStatusBadgeClass(string? status)
        {
            return status?.ToLower() switch
            {
                "aberto" => "bg-primary",
                "em_andamento" => "bg-warning text-dark",
                "pendente" => "bg-secondary",
                "resolvido" => "bg-success",
                "fechado" => "bg-info",
                "cancelado" => "bg-danger",
                _ => "bg-secondary"
            };
        }

        public string GetPriorityBadgeClass(string? prioridade)
        {
            // Usa ChaPrioridade se disponível, senão usa Prioridade
            var prio = prioridade?.ToLower();
            return prio switch
            {
                "baixa" => "bg-success",
                "media" => "bg-info",
                "alta" => "bg-warning text-dark",
                "urgente" => "bg-danger",
                _ => "bg-secondary"
            };
        }
        // Método para obter a data formatada considerando os novos campos
        public string FormatDateTime(DateTime? dateTime)
        {
            return dateTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
        }

        // Método para obter a data de criação considerando os novos campos
        public DateTime? GetDataAbertura(ChamadoDto chamado)
        {
            // Prioridade: ChaCriadoEmDateTime -> DataAbertura -> null
            return chamado.ChaCriadoEmDateTime ?? chamado.DataAbertura;
        }

        // Método para obter a prioridade considerando os novos campos
        public string? GetPrioridade(ChamadoDto chamado)
        {
            // Prioridade: ChaPrioridade -> Prioridade -> "media"
            return chamado.ChaPrioridade ?? chamado.Prioridade ?? "media";
        }

        // Método para obter o status considerando os novos campos
        public string? GetStatus(ChamadoDto chamado)
        {
            // Prioridade: ChaStatus -> Status -> "aberto"
            return chamado.ChaStatus ?? chamado.Status ?? "aberto";
        }

        // Método para obter a lista de status permitidos (atualizado com "bug")
        public List<string> GetStatusPermitidos()
        {
            return new List<string> { "aberto", "em_andamento", "pendente", "resolvido", "fechado", "cancelado", "bug" };
        }

        // Método para obter a lista de prioridades permitidas
        public List<string> GetPrioridadesPermitidas()
        {
            return new List<string> { "baixa", "media", "alta", "urgente" };
        }
    }
}