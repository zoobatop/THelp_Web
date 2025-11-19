using System;
using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class ParametroConfiguracao
    {
        [JsonPropertyName("id_parametro")]
        public int IdParametro { get; set; }

        [JsonPropertyName("par_chave")]
        public string ParChave { get; set; } = string.Empty;

        [JsonPropertyName("par_valor")]
        public string ParValor { get; set; } = string.Empty;

        [JsonPropertyName("par_categoria")]
        public string ParCategoria { get; set; } = string.Empty;

        [JsonPropertyName("par_descricao")]
        public string? ParDescricao { get; set; }

        [JsonPropertyName("par_ativo")]
        public bool ParAtivo { get; set; }

        [JsonPropertyName("par_atualizado_em")]
        public DateTime ParAtualizadoEm { get; set; }
    }
}
