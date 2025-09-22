using System.Text.Json.Serialization;
using MediatR;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public record CreateMotorcycleCommand : IRequest<GenericCommandResult>
    {
        [JsonPropertyName("identificador")]
        public required string Id { get; set; }

        [JsonPropertyName("ano")]
        public required int Year { get; set; }

        [JsonPropertyName("modelo")]
        public required string Model { get; set; }

        [JsonPropertyName("plate")]
        public required string Plate { get; set; }


    }
}
