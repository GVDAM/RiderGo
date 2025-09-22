using MediatR;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public record ValidateYearCommand(string Id) : IRequest<GenericCommandResult>;
}
