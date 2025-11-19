using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Papel
    {
        [JsonPropertyName("id_papel")]
        public int IdPapel { get; set; }

        [JsonPropertyName("pap_nome")]
        public string PapNome { get; set; } = string.Empty;

        [JsonPropertyName("pap_descricao")]
        public string? PapDescricao { get; set; }
    }
}
