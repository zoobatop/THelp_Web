using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Pages.Chamado
{
    public class ChatModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty]
        public string NovaMensagem { get; set; }

        public int ChamadoId { get; set; }
        public string ChamadoTitulo { get; set; } = "Chamado de Exemplo";
        public string ChamadoStatus { get; set; } = "Em Andamento";
        public List<Mensagem> Mensagens { get; set; }

        public class Mensagem
        {
            public string Usuario { get; set; }
            public string Texto { get; set; }
            public DateTime DataHora { get; set; }
            public bool IsUsuarioAtual { get; set; }
        }

        public string GetStatusBadgeClass(string status)
        {
            return status switch
            {
                "Aberto" => "bg-warning",
                "Em Andamento" => "bg-info",
                "Resolvido" => "bg-success",
                "Fechado" => "bg-secondary",
                _ => "bg-light text-dark"
            };
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (Id.HasValue)
            {
                ChamadoId = Id.Value;
                // TODO: Buscar informações do chamado do banco de dados
                CarregarMensagensExemplo();
            }
            else
            {
                return RedirectToPage("/Chamado/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrWhiteSpace(NovaMensagem))
            {
                ModelState.AddModelError("NovaMensagem", "A mensagem não pode estar vazia");
                CarregarMensagensExemplo();
                return Page();
            }

            // TODO: Implementar lógica de salvar mensagem no banco de dados
            // Por enquanto, apenas recarrega a página
            
            TempData["SuccessMessage"] = "Mensagem enviada com sucesso!";
            return RedirectToPage(new { id = Id });
        }

        private void CarregarMensagensExemplo()
        {
            Mensagens = new List<Mensagem>
            {
                new Mensagem 
                { 
                    Usuario = "Suporte Técnico", 
                    Texto = "Olá! Em que posso ajudar?", 
                    DataHora = DateTime.Now.AddMinutes(-30),
                    IsUsuarioAtual = false
                },
                new Mensagem 
                { 
                    Usuario = User.Identity.Name, 
                    Texto = "Estou com problemas para acessar meu e-mail.", 
                    DataHora = DateTime.Now.AddMinutes(-25),
                    IsUsuarioAtual = true
                },
                new Mensagem 
                { 
                    Usuario = "Suporte Técnico", 
                    Texto = "Você poderia verificar se a senha está correta?", 
                    DataHora = DateTime.Now.AddMinutes(-20),
                    IsUsuarioAtual = false
                },
                new Mensagem 
                { 
                    Usuario = User.Identity.Name, 
                    Texto = NovaMensagem ?? "Já tentei e não consegui.", 
                    DataHora = DateTime.Now,
                    IsUsuarioAtual = true
                }
            };
        }
    }
}