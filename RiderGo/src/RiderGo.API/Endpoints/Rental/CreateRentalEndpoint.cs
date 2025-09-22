using RiderGo.Domain.Commands.Rental;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Rental
{
    public class CreateRentalEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/", HandleAsync)
                .WithSummary("Alugar uma moto");

        private static async Task<IResult> HandleAsync(
            CreateRentalCommand request,
            ISender mediator)
        {
            var result = await mediator.Send(request);

            return result.IsSuccess
                ? Results.Created()
                : Results.BadRequest(result);
        }
    }
}
