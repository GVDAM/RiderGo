using System.Text.Json.Serialization;
using MediatR;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public record UpdateMotorcycleCommand(string Id, string Plate) : IRequest<GenericCommandResult>;

    public record UpdateMotorcycleDto : IRequest<GenericCommandResult> 
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("placa")]
        public string Plate { get; set; } = string.Empty;
    };

    public record UpdateMotorcycleCnhImageDto : IRequest<GenericCommandResult>
    {
        [JsonPropertyName("imagem_cnh")]
        public string CnhImage { get; set; } = string.Empty;
    };

    public record UpdateMotorcycleCnhImageCommand(string Id, string CnhImage) : IRequest<GenericCommandResult>;

}
