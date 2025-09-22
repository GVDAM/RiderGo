using RiderGo.Domain.Commands.Rider;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Rider
{
    public class CreateRiderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
                .WithSummary("Cadastrar entregador");

        private static async Task<IResult> HandleAsync(
            CreateRiderCommand request,
            IMediator mediator)
        {
            var result = await mediator.Send(request);

            return result.IsSuccess
                ? Results.Created()
                : Results.BadRequest(result);
        }
    }
}
