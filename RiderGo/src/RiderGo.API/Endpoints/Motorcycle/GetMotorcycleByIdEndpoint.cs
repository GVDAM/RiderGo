using MediatR;
using RiderGo.API.Common.Api;
using RiderGo.Domain.Commands.Motorcycle;

namespace RiderGo.API.Endpoints.Motorcycle
{
    public class GetMotorcycleByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{id}", HandleAsync)
                .WithSummary("Consultar motos existentes por id");

        private static async Task<IResult> HandleAsync(
            ISender mediator,
            string id)
        {
            if (string.IsNullOrEmpty(id))
                return Results.BadRequest(new { mensagem = "Request mal formada" });

            var result = await mediator.Send(new GetMotorcycleByIdCommand(id));

            return result.IsSuccess ?
                Results.Ok(result.Data) :
                Results.NotFound(result);
        }
    }
}
