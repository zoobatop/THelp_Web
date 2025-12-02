using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Usuario
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("usu_nome")]
        public string UsuNome { get; set; } = string.Empty;

        [JsonPropertyName("usu_email")]
        public string UsuEmail { get; set; } = string.Empty;

        [JsonPropertyName("usu_senha")]
        public string UsuSenha { get; set; } = string.Empty;

        [JsonPropertyName("usu_ativo")]
        public bool UsuAtivo { get; set; }

        [JsonPropertyName("id_papel")]
        public int IdPapel { get; set; }

        [JsonPropertyName("id_organizacao")]
        public int? IdOrganizacao { get; set; }

    }
}
