// Pages/Organizacao/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using THelp_Web.Services;
using THelp_Web.Models.Input;
using OrganizacaoModel = THelp_Web.Models.Domain.Organizacao;

namespace THelp_Web.Pages.Organizacao
{
    public class IndexModel : PageModel
    {
        private readonly IOrganizacaoService _organizacaoService;

        public IndexModel(IOrganizacaoService organizacaoService)
        {
            _organizacaoService = organizacaoService;
        }

        [BindProperty]
        public List<OrganizacaoModel> Organizacoes { get; set; } = new();

        [BindProperty]
        public OrganizacaoInput OrganizacaoInput { get; set; } = new();

        public int OrganizacaoEditId { get; set; }

        public string Mensagem { get; set; } = string.Empty;
        public string TipoMensagem { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await CarregarOrganizacoes();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await CarregarOrganizacoes();
                return Page();
            }

            try
            {
                var resultado = await _organizacaoService.CreateAsync(OrganizacaoInput);
                if (resultado != null)
                {
                    Mensagem = "Organização criada com sucesso!";
                    TipoMensagem = "success";
                    OrganizacaoInput = new OrganizacaoInput();
                    ModelState.Clear();
                }
                else
                {
                    Mensagem = "Erro ao criar organização.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarOrganizacoes();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                await CarregarOrganizacoes();
                return Page();
            }

            try
            {
                var resultado = await _organizacaoService.UpdateAsync(OrganizacaoEditId, OrganizacaoInput);
                if (resultado != null)
                {
                    Mensagem = "Organização atualizada com sucesso!";
                    TipoMensagem = "success";
                    OrganizacaoInput = new OrganizacaoInput();
                    OrganizacaoEditId = 0;
                    ModelState.Clear();
                }
                else
                {
                    Mensagem = "Erro ao atualizar organização.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarOrganizacoes();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var sucesso = await _organizacaoService.DeleteAsync(id);
                if (sucesso)
                {
                    Mensagem = "Organização excluída com sucesso!";
                    TipoMensagem = "success";
                }
                else
                {
                    Mensagem = "Erro ao excluir organização.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarOrganizacoes();
            return Page();
        }

        public async Task<IActionResult> OnPostDesativarAsync(int id)
        {
            try
            {
                var resultado = await _organizacaoService.DesativarAsync(id);
                if (resultado != null)
                {
                    Mensagem = "Organização desativada com sucesso!";
                    TipoMensagem = "success";
                }
                else
                {
                    Mensagem = "Erro ao desativar organização.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarOrganizacoes();
            return Page();
        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var organizacao = await _organizacaoService.GetByIdAsync(id);
            if (organizacao != null)
            {
                OrganizacaoEditId = organizacao.IdOrganizacao;
                OrganizacaoInput = new OrganizacaoInput
                {
                    Nome = organizacao.OrgNome,
                    Cnpj = organizacao.OrgCnpj,
                    Email = organizacao.OrgEmail,
                    Telefone = organizacao.OrgTelefone,
                    Ativo = organizacao.OrgAtivo
                };
            }

            await CarregarOrganizacoes();
            return Page();
        }

        public async Task<IActionResult> OnGetCancelEditAsync()
        {
            OrganizacaoInput = new OrganizacaoInput();
            OrganizacaoEditId = 0;
            await CarregarOrganizacoes();
            return Page();
        }

        private async Task CarregarOrganizacoes()
        {
            Organizacoes = await _organizacaoService.GetAllAsync();
        }
    }
}