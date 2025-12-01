using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Models.Input
{
    public class UsuarioInput
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres")]
        public string Senha { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        [Required(ErrorMessage = "O papel é obrigatório")]
        public int IdPapel { get; set; }

        public int? IdOrganizacao { get; set; }
    }
}