using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Motorcycle
{
    public class CreateMotorCycleEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync )
                .WithSummary("Cadastrar uma nova moto");

        private static async Task<IResult> HandleAsync(
            CreateMotorcycleCommand request,
            IMediator mediator)
        {
            var result = await mediator.Send(request);

            return result.IsSuccess
                ? Results.Created()
                : Results.BadRequest(result);
        }
    }
}
