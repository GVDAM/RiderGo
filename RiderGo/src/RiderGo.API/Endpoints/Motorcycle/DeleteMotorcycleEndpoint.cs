using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Motorcycle
{
    public class DeleteMotorcycleEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandlerAsync)
                .WithSummary("Remover uma moto");

        private static async Task<IResult> HandlerAsync(
            ISender mediator,
            string id)
        {
            var result = await mediator.Send(new DeleteMotorcycleCommand(id));

            return result.IsSuccess ?
                Results.Ok() :
                Results.BadRequest(result);
        }
    }
}
