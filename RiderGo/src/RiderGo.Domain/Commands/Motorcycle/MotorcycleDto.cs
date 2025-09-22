using System.Text.Json.Serialization;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public class MotorcycleDto
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
