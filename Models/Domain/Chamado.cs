using System;
using System.Text.Json.Serialization;

namespace THelp_Web.Models.Domain
{
    public class Chamado
    {
        [JsonPropertyName("id_chamado")]
        public int IdChamado { get; set; }

        [JsonPropertyName("cha_titulo")]
        public string ChaTitulo { get; set; } = string.Empty;

        [JsonPropertyName("cha_descricao")]
        public string ChaDescricao { get; set; } = string.Empty;

        [JsonPropertyName("id_organizacao")]
        public int IdOrganizacao { get; set; }

        [JsonPropertyName("id_usuario_abertura")]
        public int IdUsuarioAbertura { get; set; }

        [JsonPropertyName("id_usuario_atribuido")]
        public int? IdUsuarioAtribuido { get; set; }

        [JsonPropertyName("cha_status")]
        public string ChaStatus { get; set; } = "aberto";

        [JsonPropertyName("cha_prioridade")]
        public string ChaPrioridade { get; set; } = "media";

        [JsonPropertyName("cha_criado_em")]
        public DateTime ChaCriadoEm { get; set; }

        [JsonPropertyName("cha_atualizado_em")]
        public DateTime ChaAtualizadoEm { get; set; }

        [JsonPropertyName("cha_finalizado_em")]
        public DateTime? ChaFinalizadoEm { get; set; }

        [JsonPropertyName("organizacao")]
        public Organizacao? Organizacao { get; set; }

        [JsonPropertyName("usuario_abertura")]
        public Usuario? UsuarioAbertura { get; set; }

        [JsonPropertyName("usuario_atribuido")]
        public Usuario? UsuarioAtribuido { get; set; }
    }
}
