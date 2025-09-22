using System.Text.Json.Serialization;
using MediatR;

namespace RiderGo.Domain.Commands.Rental
{
    public record CreateRentalCommand : IRequest<GenericCommandResult>
    {
        [JsonPropertyName("entregador_id")]
        public string RiderId { get; set; } = string.Empty;

        [JsonPropertyName("moto_id")]
        public string MotorcycleId { get; set; } = string.Empty;

        [JsonPropertyName("data_inicio")]
        public DateTime? StartDate { get; set; } 

        [JsonPropertyName("data_termino")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime? ExpectedEndDate { get; set; } 

        [JsonPropertyName("plano")]
        public int Plan { get; set; }
    }
}
