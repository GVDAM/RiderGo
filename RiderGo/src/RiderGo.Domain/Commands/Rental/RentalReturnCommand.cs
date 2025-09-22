using System.Text.Json.Serialization;
using MediatR;

namespace RiderGo.Domain.Commands.Rental
{
    public record RentalReturnCommand(Guid Id, DateTime ReturnDate) : IRequest<GenericCommandResult>;

    public class RentalReturnDto
    {
        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; }
    }
}
