using MediatR;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public record GetAllMotorcycleCommand(string? Plate) : IRequest<GenericCommandResult>;
}
