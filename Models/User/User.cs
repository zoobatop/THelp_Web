using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Models.User
{
    public class User
    {
        private int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        private string Nome { get; set; }
        private string Email { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        private string Password { get; set; }
        private bool Ativo { get; set; }

    }
}
