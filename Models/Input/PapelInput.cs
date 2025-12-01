using System.ComponentModel.DataAnnotations;

namespace THelp_Web.Models.Input
{
    public class PapelInput
    {
        [Required(ErrorMessage = "O nome do papel é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }
    }
}