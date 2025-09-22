using Google.Cloud.PubSub.V1;
using Google.Api.Gax;
using Grpc.Core;

namespace RiderGo.API.HostedServices
{
    public class PubSubInitializer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PubSubInitializer> _logger;

        public PubSubInitializer(IConfiguration configuration, ILogger<PubSubInitializer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando Pub/Sub Initializer");

            var projectId = _configuration["PubSub:ProjectId"];
            var topicId = _configuration["PubSub:TopicId"];
            var subscriptionId = _configuration["PubSub:SubscriptionId"];
            var emulatorHost = _configuration["PubSub:EmulatorHost"];

            var topicName = TopicName.FromProjectTopic(projectId, topicId);
            var subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

            _logger.LogInformation("Conectando ao Pub/Sub Emulator em {emulatorHost}", emulatorHost);
            Environment.SetEnvironmentVariable("PUBSUB_EMULATOR_HOST", emulatorHost);

            var publisherService = new PublisherServiceApiClientBuilder
            {
                EmulatorDetection = EmulatorDetection.EmulatorOnly
            }.Build(); ;

            var subscriberService = new SubscriberServiceApiClientBuilder
            {
                EmulatorDetection = EmulatorDetection.EmulatorOnly
            }.Build();


            
            try
            {
                _logger.LogInformation("Criando tópico {topicName}", topicName);
                await publisherService.CreateTopicAsync(topicName);

            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                _logger.LogError(e, "Tópico {topicName} já existe", topicName);
            }

            
            try
            {
                _logger.LogInformation("Criando assinatura {subscriptionName} para o tópico {topicName}", subscriptionName, topicName);
                await subscriberService.CreateSubscriptionAsync(
                    subscriptionName,
                    topicName,
                    pushConfig: null,
                    ackDeadlineSeconds: 60);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                _logger.LogError(e, "Assinatura {subscriptionName} já existe", subscriptionName);
            }


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
