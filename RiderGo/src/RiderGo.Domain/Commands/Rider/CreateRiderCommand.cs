using System.Text.Json.Serialization;
using MediatR;

namespace RiderGo.Domain.Commands.Rider
{
    public record CreateRiderCommand : IRequest<GenericCommandResult>
    {
        [JsonPropertyName("identificador")]
        public string Id { get; set; } = String.Empty;

        [JsonPropertyName("nome")]
        public string Name { get; set; } = String.Empty;

        [JsonPropertyName("cnpj")]
        public string CNPJ { get; set; } = String.Empty;

        [JsonPropertyName("data_nascimento")]
        public DateTime Birth { get; set; }

        [JsonPropertyName("numero_cnh")]
        public string CnhNumber { get; set; } = String.Empty;

        [JsonPropertyName("tipo_cnh")]
        public string CnhType { get; set; } = String.Empty;

        [JsonPropertyName("imagem_cnh")]
        public string CnhImage { get; set; } = String.Empty;
    }
}
