using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Permissao
    {
        [JsonPropertyName("id_permissao")]
        public int IdPermissao { get; set; }

        [JsonPropertyName("per_nome")]
        public string PerNome { get; set; } = string.Empty;

        [JsonPropertyName("per_descricao")]
        public string? PerDescricao { get; set; }
    }
}
