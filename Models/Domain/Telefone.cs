using System;
using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Telefone
    {
        [JsonPropertyName("id_telefone")]
        public int IdTelefone { get; set; }

        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("tel_ddd")]
        public string TelDdd { get; set; } = string.Empty;

        [JsonPropertyName("tel_numero")]
        public string TelNumero { get; set; } = string.Empty;

        [JsonPropertyName("tel_tipo")]
        public string TelTipo { get; set; } = "CELULAR";

        [JsonPropertyName("tel_principal")]
        public bool TelPrincipal { get; set; }

        [JsonPropertyName("tel_ativo")]
        public bool TelAtivo { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("usuario")]
        public Usuario? Usuario { get; set; }
    }
}
