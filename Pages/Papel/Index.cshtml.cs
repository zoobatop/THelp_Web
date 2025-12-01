using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using THelp_Web.Interface;
using THelp_Web.Models.Input;
using PapelModel = THelp_Web.Models.Domain.Papel;

namespace THelp_Web.Pages.Papel
{
    public class IndexModel : PageModel
    {
        private readonly IPapelService _papelService;

        public IndexModel(IPapelService papelService)
        {
            _papelService = papelService;
        }

        [BindProperty]
        public List<PapelModel> Papeis { get; set; } = new();

        [BindProperty]
        public PapelInput PapelInput { get; set; } = new();

        [BindProperty]
        public string SearchTerm { get; set; } = string.Empty;

        public int PapelEditId { get; set; }

        public string Mensagem { get; set; } = string.Empty;
        public string TipoMensagem { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await CarregarPapeis();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                // Verificar se já existe papel com mesmo nome
                var existe = await _papelService.ExistsByNomeAsync(PapelInput.Nome);
                if (existe)
                {
                    Mensagem = "Já existe um papel com este nome.";
                    TipoMensagem = "error";
                    await CarregarPapeis();
                    return Page();
                }

                var resultado = await _papelService.CreateAsync(PapelInput);
                if (resultado != null)
                {
                    Mensagem = "Papel criado com sucesso!";
                    TipoMensagem = "success";
                    PapelInput = new PapelInput();
                    ModelState.Clear();
                }
                else
                {
                    Mensagem = "Erro ao criar papel.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarPapeis();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                await CarregarPapeis();
                return Page();
            }

            try
            {
                var resultado = await _papelService.UpdateAsync(PapelEditId, PapelInput);
                if (resultado != null)
                {
                    Mensagem = "Papel atualizado com sucesso!";
                    TipoMensagem = "success";
                    PapelInput = new PapelInput();
                    PapelEditId = 0;
                    ModelState.Clear();
                }
                else
                {
                    Mensagem = "Erro ao atualizar papel.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarPapeis();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var sucesso = await _papelService.DeleteAsync(id);
                if (sucesso)
                {
                    Mensagem = "Papel excluído com sucesso!";
                    TipoMensagem = "success";
                }
                else
                {
                    Mensagem = "Erro ao excluir papel.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarPapeis();
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await CarregarPapeis();
                return Page();
            }

            try
            {
                Papeis = await _papelService.SearchAsync(SearchTerm);
                if (Papeis.Count == 0)
                {
                    Mensagem = "Nenhum papel encontrado com o termo informado.";
                    TipoMensagem = "warning";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro na busca: {ex.Message}";
                TipoMensagem = "error";
                await CarregarPapeis();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var papel = await _papelService.GetByIdAsync(id);
            if (papel != null)
            {
                PapelEditId = papel.IdPapel;
                PapelInput = new PapelInput
                {
                    Nome = papel.PapNome,
                    Descricao = papel.PapDescricao
                };
            }

            await CarregarPapeis();
            return Page();
        }

        public async Task<IActionResult> OnGetCancelEditAsync()
        {
            PapelInput = new PapelInput();
            PapelEditId = 0;
            await CarregarPapeis();
            return Page();
        }

        private async Task CarregarPapeis()
        {
            Papeis = await _papelService.GetAllAsync();
        }
    }
}