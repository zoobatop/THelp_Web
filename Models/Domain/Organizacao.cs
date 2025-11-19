using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Organizacao
    {
        [JsonPropertyName("id_organizacao")]
        public int IdOrganizacao { get; set; }

        [JsonPropertyName("org_nome")]
        public string OrgNome { get; set; } = string.Empty;

        [JsonPropertyName("org_cnpj")]
        public string OrgCnpj { get; set; } = string.Empty;

        [JsonPropertyName("org_email")]
        public string? OrgEmail { get; set; }

        [JsonPropertyName("org_telefone")]
        public string? OrgTelefone { get; set; }

        [JsonPropertyName("org_ativo")]
        public bool OrgAtivo { get; set; }
    }
}
