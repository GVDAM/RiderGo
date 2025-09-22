using RiderGo.Domain.Commands.Rental;
using RiderGo.API.Common.Api;
using MediatR;

namespace RiderGo.API.Endpoints.Rental
{
    public class RentalReturnEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => 
            app.MapPut("/{id}/devolucao", HandleAsync)
                .WithSummary("Informar data da devolução e calcular valor");

        private static async Task<IResult> HandleAsync(
            RentalReturnDto request,
            ISender sender,
            Guid id)
        {            
            var result = await sender.Send(new RentalReturnCommand(id, request.ReturnDate!.Value));

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.BadRequest(result);
        }
    }
}
