using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Rider
{
    public class UpdateCnhImageRiderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/{id}/cnh", HandleAsync)
                .WithSummary("Enviar foto da CNh");
        private static async Task<IResult> HandleAsync(
            ISender mediator,
            UpdateMotorcycleCnhImageDto request,
            string id)
        {
            var updatedRequest = new UpdateMotorcycleCnhImageCommand(id, request.CnhImage);

            var result = await mediator.Send(updatedRequest);

            return result.IsSuccess ?
                Results.Created() :
                Results.BadRequest(result);
        }
    }
}
