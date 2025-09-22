using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Motorcycle
{
    public class UpdateMotorcycleEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPut("/{id}/placa", HandleAsync)
                .WithSummary("Modificar a placa de uma moto");
        private static async Task<IResult> HandleAsync(
            ISender mediator,
            UpdateMotorcycleDto request,
            string id)
        {
            var updatedRequest = new UpdateMotorcycleCommand(id, request.Plate);

            var result = await mediator.Send(updatedRequest);

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.BadRequest(result);
        }
    }
}
