using System.Text.Json.Serialization;

namespace RiderGo.Domain.Entities
{
    public class Motorcycle
    {
        [JsonPropertyName("identificador")]
        public string Id { get; set; } = String.Empty;

        [JsonPropertyName("ano")]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; } = String.Empty;

        [JsonPropertyName("plate")]
        public string Plate { get; set; } = String.Empty;

        [JsonIgnore]
        public bool? IsFrom2024 { get; set; }


        [JsonIgnore]
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
