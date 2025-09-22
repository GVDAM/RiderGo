using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Motorcycle
{
    public class GetAllMotorcyclesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
                .WithSummary("Consultar motos existentes");

        private static async Task<IResult> HandleAsync(
            ISender mediator,
            string? plate)
        {
            var result = await mediator.Send(new GetAllMotorcycleCommand(plate));

            return Results.Ok(result.Data);
        }
    }
}
