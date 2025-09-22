using MediatR;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public record GetMotorcycleByIdCommand(string Id) : IRequest<GenericCommandResult>;
}
