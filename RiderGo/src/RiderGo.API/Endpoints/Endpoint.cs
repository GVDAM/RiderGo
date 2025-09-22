using RiderGo.API.Endpoints.Motorcycle;
using RiderGo.API.Endpoints.Rental;
using RiderGo.API.Endpoints.Rider;
using RiderGo.API.Common.Api;

namespace RiderGo.API.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");

            endpoints.MapGroup("motos")
                .WithTags("motos")
                .MapEndpoint<CreateMotorCycleEndpoint>()
                .MapEndpoint<UpdateMotorcycleEndpoint>()
                .MapEndpoint<GetAllMotorcyclesEndpoint>()
                .MapEndpoint<GetMotorcycleByIdEndpoint>()
                .MapEndpoint<DeleteMotorcycleEndpoint>();


            endpoints.MapGroup("entregadores")
                .WithTags("entregadores")
                .MapEndpoint<CreateRiderEndpoint>()
                .MapEndpoint<UpdateCnhImageRiderEndpoint>();

            endpoints.MapGroup("locacao")
                .WithTags("locação")
                .MapEndpoint<CreateRentalEndpoint>()
                .MapEndpoint<RentalReturnEndpoint>();

        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
