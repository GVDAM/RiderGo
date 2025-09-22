using MediatR;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public record DeleteMotorcycleCommand(string Id) : IRequest<GenericCommandResult>;
}
