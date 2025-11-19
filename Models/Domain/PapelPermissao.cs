using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class PapelPermissao
    {
        [JsonPropertyName("id_papel")]
        public int IdPapel { get; set; }

        [JsonPropertyName("id_permissao")]
        public int IdPermissao { get; set; }
    }
}
