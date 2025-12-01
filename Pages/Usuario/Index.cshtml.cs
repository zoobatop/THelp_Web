// Pages/Usuario/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using THelp_Web.Interface;
using THelp_Web.Models.Input;
using THelp_Web.Services;
using UsuarioModel = THelp_Web.Models.Domain.Usuario;

namespace THelp_Web.Pages.Usuario
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public IndexModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public List<UsuarioModel> Usuarios { get; set; } = new();

        [BindProperty]
        public UsuarioInput UsuarioInput { get; set; } = new();

        public int UsuarioEditId { get; set; }

        public string Mensagem { get; set; } = string.Empty;
        public string TipoMensagem { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await CarregarUsuarios();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await CarregarUsuarios();
                return Page();
            }

            try
            {
                var resultado = await _usuarioService.CreateAsync(UsuarioInput);
                if (resultado != null)
                {
                    Mensagem = "Usuário criado com sucesso!";
                    TipoMensagem = "success";
                    UsuarioInput = new UsuarioInput(); // Limpa o formulário
                    ModelState.Clear();
                }
                else
                {
                    Mensagem = "Erro ao criar usuário.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarUsuarios();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                await CarregarUsuarios();
                return Page();
            }

            try
            {
                var resultado = await _usuarioService.UpdateAsync(UsuarioEditId, UsuarioInput);
                if (resultado != null)
                {
                    Mensagem = "Usuário atualizado com sucesso!";
                    TipoMensagem = "success";
                    UsuarioInput = new UsuarioInput();
                    UsuarioEditId = 0;
                    ModelState.Clear();
                }
                else
                {
                    Mensagem = "Erro ao atualizar usuário.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarUsuarios();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var sucesso = await _usuarioService.DeleteAsync(id);
                if (sucesso)
                {
                    Mensagem = "Usuário excluído com sucesso!";
                    TipoMensagem = "success";
                }
                else
                {
                    Mensagem = "Erro ao excluir usuário.";
                    TipoMensagem = "error";
                }
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                TipoMensagem = "error";
            }

            await CarregarUsuarios();
            return Page();
        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario != null)
            {
                UsuarioEditId = usuario.IdUsuario;
                UsuarioInput = new UsuarioInput
                {
                    Nome = usuario.UsuNome,
                    Email = usuario.UsuEmail,
                    Senha = usuario.UsuSenha, // Na prática, você não deveria expor a senha
                    Ativo = usuario.UsuAtivo,
                    IdPapel = usuario.IdPapel,
                    IdOrganizacao = usuario.IdOrganizacao
                };
            }

            await CarregarUsuarios();
            return Page();
        }

        public async Task<IActionResult> OnGetCancelEditAsync()
        {
            UsuarioInput = new UsuarioInput();
            UsuarioEditId = 0;
            await CarregarUsuarios();
            return Page();
        }

        private async Task CarregarUsuarios()
        {
            Usuarios = await _usuarioService.GetAllAsync();
        }
    }
}