using RiderGo.Domain.Commands.Motorcycle;
using Google.Cloud.PubSub.V1;
using System.Text.Json;
using MediatR;

namespace RiderGo.API.HostedServices
{
    public class MessageSubscriberService : IHostedService
    {
        private readonly ILogger<MessageSubscriberService> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly SubscriberClient _subscriberClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public MessageSubscriberService(
            ILogger<MessageSubscriberService> logger,
            IServiceScopeFactory scopeFactory,
            SubscriberClient subscriberClient)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _subscriberClient = subscriberClient;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _logger.LogInformation("Subscriber pronto e ouvindo por mensagens");

            _subscriberClient.StartAsync(async (message, ct) =>
            {
                _logger.LogInformation($"Mensagem recebida: {message.MessageId}");
                                    
                try
                {
                    var messageString = message.Data.ToStringUtf8();
                    
                    var command = JsonSerializer.Deserialize<ValidateYearCommand>(messageString);

                    if (command != null)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();
                            var result = await mediator.Send(command, ct);

                            if (result.IsSuccess)
                            {
                                _logger.LogInformation($"Mensagem processada com sucesso: {message.MessageId}");
                            }
                            else
                            {
                                _logger.LogWarning($"Falha ao processar a mensagem: {message.MessageId} - {result.Message}");
                                return SubscriberClient.Reply.Nack;
                            }
                        }
                    }

                    return SubscriberClient.Reply.Ack;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao processar a mensagem: {message.MessageId}");
                    return SubscriberClient.Reply.Nack;
                }
            });


            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _subscriberClient.StopAsync(cancellationToken);
        }
    }
}
