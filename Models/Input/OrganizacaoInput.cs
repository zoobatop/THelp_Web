using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Models.Input
{
    public class OrganizacaoInput
    {
        [Required(ErrorMessage = "O nome da organização é obrigatório")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 caracteres")]
        public string Cnpj { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
        public string? Telefone { get; set; }

        public bool Ativo { get; set; } = true;
    }
}