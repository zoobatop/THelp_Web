using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Endereco
    {
        [JsonPropertyName("id_endereco")]
        public int IdEndereco { get; set; }

        [JsonPropertyName("id_usuario")]
        public int? IdUsuario { get; set; }

        [JsonPropertyName("id_organizacao")]
        public int? IdOrganizacao { get; set; }

        [JsonPropertyName("end_logradouro")]
        public string EndLogradouro { get; set; } = string.Empty;

        [JsonPropertyName("end_numero")]
        public string EndNumero { get; set; } = string.Empty;

        [JsonPropertyName("end_complemento")]
        public string? EndComplemento { get; set; }

        [JsonPropertyName("end_cep")]
        public string EndCep { get; set; } = string.Empty;

        [JsonPropertyName("end_uf")]
        public string EndUf { get; set; } = string.Empty;

        [JsonPropertyName("usuario")]
        public Usuario? Usuario { get; set; }

        [JsonPropertyName("organizacao")]
        public Organizacao? Organizacao { get; set; }
    }
}
