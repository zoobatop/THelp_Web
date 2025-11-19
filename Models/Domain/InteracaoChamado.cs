using System;
using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class InteracaoChamado
    {
        [JsonPropertyName("id_interacao")]
        public int IdInteracao { get; set; }

        [JsonPropertyName("id_chamado")]
        public int IdChamado { get; set; }

        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("int_mensagem")]
        public string IntMensagem { get; set; } = string.Empty;

        [JsonPropertyName("int_url_anexo")]
        public string? IntUrlAnexo { get; set; }

        [JsonPropertyName("int_criado_em")]
        public DateTime IntCriadoEm { get; set; }

        [JsonPropertyName("chamado")]
        public Chamado? Chamado { get; set; }

        [JsonPropertyName("usuario")]
        public Usuario? Usuario { get; set; }
    }
}
