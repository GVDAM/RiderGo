using RiderGo.Infrastructure.Repositories;
using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.Domain.Commands.Rental;
using RiderGo.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Commands.Rider;
using RiderGo.Domain.Repositories;
using RiderGo.Infrastructure.Data;
using Google.Apis.Storage.v1.Data;
using RiderGo.API.HostedServices;
using RiderGo.Domain.Behaviors;
using RiderGo.Domain.Commands;
using Google.Cloud.Storage.V1;
using RiderGo.Domain.Handlers;
using Google.Cloud.PubSub.V1;
using RiderGo.Domain.Storage;
using FluentValidation;
using Google.Api.Gax;
using Grpc.Core;
using MediatR;


namespace RiderGo.API.Common.Api
{
    public static class BuilderExtension
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            var googleStorageConfig = builder.Configuration.GetSection("GoogleCloudStorage");
            var credentialJson = googleStorageConfig.GetSection("CredentialsJson").Value;
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(credentialJson);
            
            builder.Services.AddSingleton<IFileStorage>(sp =>
            {
                   var logger = sp.GetRequiredService<ILogger<GoogleCloudFileStorage>>();

                    return new GoogleCloudFileStorage(
                        googleStorageConfig.GetSection("BucketName").Value!, 
                        credential, 
                        googleStorageConfig.GetSection("Uri").Value!,
                        logger);
            });

        }

        public static void AddDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });
        }

        public static void AddDataContexts(this WebApplicationBuilder builder)
        {
            var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? throw new Exception("Connection string not found!");

            builder.Services.AddDbContext<AppDbContext>(options
                => options.UseNpgsql(cnnStr, x => x.MigrationsAssembly("RiderGo.API")));
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(
                options => options.AddDefaultPolicy(x => x.AllowAnyOrigin()));
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            #region MediatR Handlers

            // MotorCycle
            builder.Services.AddScoped<IRequestHandler<CreateMotorcycleCommand, GenericCommandResult>, MotorcycleHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdateMotorcycleCommand, GenericCommandResult>, MotorcycleHandler>();
            builder.Services.AddScoped<IRequestHandler<GetAllMotorcycleCommand, GenericCommandResult>, MotorcycleHandler>();
            builder.Services.AddScoped<IRequestHandler<GetMotorcycleByIdCommand, GenericCommandResult>, MotorcycleHandler>();
            builder.Services.AddScoped<IRequestHandler<DeleteMotorcycleCommand, GenericCommandResult>, MotorcycleHandler>();
            builder.Services.AddScoped<IRequestHandler<ValidateYearCommand, GenericCommandResult>, MotorcycleHandler>();

            // Rider
            builder.Services.AddScoped<IRequestHandler<CreateRiderCommand, GenericCommandResult>, RiderHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdateMotorcycleCnhImageCommand, GenericCommandResult>, RiderHandler>();

            // Rental
            builder.Services.AddScoped<IRequestHandler<CreateRentalCommand, GenericCommandResult>, RentalHandler>();
            builder.Services.AddScoped<IRequestHandler<RentalReturnCommand, GenericCommandResult>, RentalHandler>();

            #endregion


            #region Repositories

            builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            builder.Services.AddScoped<IRiderRepository, RiderRepository>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();

            #endregion

        }

        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(BuilderExtension).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(CreateMotorcycleCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateRiderCommand).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static void AddPubSubConfiguration(this WebApplicationBuilder builder)
        {
            var pubSubConfig = builder.Configuration.GetSection("PubSub");
            var emulatorHost = pubSubConfig["EmulatorHost"];
            Environment.SetEnvironmentVariable("PUBSUB_EMULATOR_HOST", emulatorHost);

            builder.Services.AddSingleton<PublisherClient>(provider =>
            {
                var publisherClientBuilder = new PublisherClientBuilder
                {
                    EmulatorDetection = EmulatorDetection.EmulatorOnly,
                    TopicName = TopicName.FromProjectTopic(
                        pubSubConfig["ProjectId"]!,
                        pubSubConfig["TopicId"]!)
                };

                return publisherClientBuilder.Build();
            });

            builder.Services.AddSingleton<SubscriberClient>(provider =>
            {
                var subscriberClientBuilder = new SubscriberClientBuilder
                {
                    EmulatorDetection = EmulatorDetection.EmulatorOnly,
                    SubscriptionName = SubscriptionName.FromProjectSubscription(
                        pubSubConfig["ProjectId"]!,
                        pubSubConfig["SubscriptionId"]!)
                };


                return subscriberClientBuilder.Build();
            });

            builder.Services.AddHostedService<PubSubInitializer>();
            builder.Services.AddHostedService<MessageSubscriberService>();
        }

    }
}
