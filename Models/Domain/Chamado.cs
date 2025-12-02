using System.Text.Json.Serialization;
using THelp_Web.Models.Domain;

namespace THelp_Web.Models
{
    public class ChamadoDto
    {
        public int? Id { get; set; }
        public string ChaTitulo { get; set; } = string.Empty;
        public string ChaDescricao { get; set; } = string.Empty;
        public string? ChaStatus { get; set; } = "aberto";
        public int? IdOrganizacao { get; set; }
        public int? IdUsuarioAbertura { get; set; }
        public int? IdUsuarioAtribuido { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataFechamento { get; set; }
        public string? Prioridade { get; set; } = "media";
        public string? Categoria { get; set; }

        // Novos campos do Map do Java
        public string? UsuarioAberturaNome { get; set; }
        public string? UsuarioAtribuidoNome { get; set; }
        public string? OrganizacaoNome { get; set; }

        // Campos com nomes diferentes no JSON
        public double? ChaCriadoEm { get; set; }  // timestamp em segundos
        public double? ChaAtualizadoEm { get; set; }  // timestamp em segundos
        public string? ChaPrioridade { get; set; }  // "chaPrioridade" no JSON

        // Propriedade de conveniência para compatibilidade
        public DateTime? ChaCriadoEmDateTime
        {
            get
            {
                if (ChaCriadoEm.HasValue)
                {
                    // Converte timestamp (segundos) para DateTime
                    return DateTimeOffset.FromUnixTimeSeconds((long)ChaCriadoEm.Value).DateTime;
                }
                return DataAbertura;
            }
        }

        public DateTime? ChaAtualizadoEmDateTime
        {
            get
            {
                if (ChaAtualizadoEm.HasValue)
                {
                    // Converte timestamp (segundos) para DateTime
                    return DateTimeOffset.FromUnixTimeSeconds((long)ChaAtualizadoEm.Value).DateTime;
                }
                return DataAtualizacao;
            }
        }

        // Propriedade para compatibilidade com o código existente
        public string? Status => ChaStatus;
    }

    public class ChamadoCreateDto
    {
        public string ChaTitulo { get; set; } = string.Empty;
        public string ChaDescricao { get; set; } = string.Empty;
        public int IdOrganizacao { get; set; }
        public int IdUsuarioAbertura { get; set; }
        public string? Prioridade { get; set; } = "media";
        public string? Categoria { get; set; }
    }

    public class ChamadoUpdateDto
    {
        public string? ChaTitulo { get; set; }
        public string? ChaDescricao { get; set; }
        public string? Prioridade { get; set; }
        public string? Categoria { get; set; }
        public int? IdUsuarioAtribuido { get; set; }
    }

    public class AtribuirUsuarioDto
    {
        public int IdUsuario { get; set; }
    }

    public class AtualizarStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    public class ChamadoResponse
    {
        public List<ChamadoDto>? Data { get; set; }
        public ChamadoDto? Chamado { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public long? Timestamp { get; set; }
    }
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

        [JsonPropertyName("usuario_abertura")]
        public Usuario? UsuarioAbertura { get; set; }

        [JsonPropertyName("usuario_atribuido")]
        public Usuario? UsuarioAtribuido { get; set; }
    }
}

   
