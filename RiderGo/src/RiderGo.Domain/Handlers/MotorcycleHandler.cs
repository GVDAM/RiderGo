using Microsoft.Extensions.Configuration;
using RiderGo.Domain.Commands.Motorcycle;
using Microsoft.Extensions.Logging;
using RiderGo.Domain.Repositories;
using RiderGo.Domain.Entities;
using RiderGo.Domain.Commands;
using Google.Cloud.PubSub.V1;
using System.Text.Json;
using Google.Protobuf;
using MediatR;

namespace RiderGo.Domain.Handlers
{
    public class MotorcycleHandler : 
        IRequestHandler<CreateMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<UpdateMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<GetAllMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<GetMotorcycleByIdCommand, GenericCommandResult>,
        IRequestHandler<DeleteMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<ValidateYearCommand, GenericCommandResult>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly PublisherClient _publisherClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MotorcycleHandler> _logger;

        public MotorcycleHandler(IMotorcycleRepository motorcycleRepository, 
            PublisherClient publisherClient,
            IConfiguration configuration,
            ILogger<MotorcycleHandler> logger)
        {
            _repository = motorcycleRepository;
            _publisherClient = publisherClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<GenericCommandResult> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando criação de moto");

                _logger.LogInformation("Validando se a moto já está cadastrada");
                var existingMotorcycle = await _repository.AlreadyRegisteredAsync(request.Plate, request.Id);

                if (existingMotorcycle)
                    return new GenericCommandResult(false, "Dados Inválidos", null!);

                var motorcycle = new Motorcycle()
                {
                    Id = request.Id,
                    Year = request.Year,
                    Model = request.Model,
                    Plate = request.Plate
                };

                _logger.LogInformation("Moto validada com sucesso, prosseguindo para criação");
                await _repository.CreateAsync(motorcycle);

                _logger.LogInformation("Moto criada com sucesso, publicando mensagem no Pub/Sub. motorcycle Id: {motoId}", motorcycle.Id);
                await PublishMessagePubSub(motorcycle.Id);

                _logger.LogInformation("Mensagem publicada com sucesso no Pub/Sub. motorcycle Id: {motoId}", motorcycle.Id);
                return new GenericCommandResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar moto: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Dados Inválidos");
            }            
        }        

        public async Task<GenericCommandResult> Handle(GetAllMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando todas as motos");
                var motorcycles = await _repository.GetAll(request.Plate);

                _logger.LogInformation("Motos buscadas com sucesso. Total de motos encontradas: {total}", motorcycles.Count());
                return new GenericCommandResult(true, "sucesso", motorcycles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar as motos: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao buscar as motos");
            }
            
        }

        public async Task<GenericCommandResult> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando atualização da placa da moto. motorcycle Id: {motoId}", request.Id);
                var motorcycle = await _repository.GetByIdAsync(request.Id);

                if (motorcycle is null)
                    return new GenericCommandResult(false, "Dados inválidos", null!);

                await _repository.UpdatePlateAsync(request.Id, request.Plate);

                _logger.LogInformation("Placa da moto atualizada com sucesso. motorcycle Id: {motoId}", request.Id);
                return new GenericCommandResult(true, "Placa modificada com sucesso", null!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a moto: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao atualizar a moto");
            }
            
        }

        public async Task<GenericCommandResult> Handle(GetMotorcycleByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando moto por ID. motorcycle Id: {motoId}", request.Id);
                var motorcycle = await _repository.GetByIdAsync(request.Id);

                if (motorcycle is null)
                    return new GenericCommandResult(false, "Moto não encontrada", null!);

                _logger.LogInformation("Moto encontrada com sucesso. motorcycle Id: {motoId}", request.Id);
                return new GenericCommandResult(true, "sucesso", motorcycle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar a moto: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao buscar a moto");
            }
            
        }

        public async Task<GenericCommandResult> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando remoção de moto. motorcycle Id: {motoId}", request.Id);
                var motorcycle = await _repository.GetByIdAsync(request.Id);

                _logger.LogInformation("Verificando se a moto existe. motorcycle Id: {motoId}", request.Id);
                if (motorcycle is null)
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Verificando se a moto já foi alugada. motorcycle Id: {motoId}", request.Id);
                if (await _repository.HasAlreadyBeenRentAsync(request.Id))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Removendo moto. motorcycle Id: {motoId}", request.Id);
                await _repository.DeleteAsync(motorcycle);

                return new GenericCommandResult(true, "Moto removida com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover a moto: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao remover a moto");
            }
            
        }

        public async Task<GenericCommandResult> Handle(ValidateYearCommand request, CancellationToken cancellationToken)
        {
            try
            {
                const int exactYear = 2024;

                _logger.LogInformation("Iniciando validação do ano da moto. motorcycle Id: {motoId}", request.Id);
                var motorcycle = await _repository.GetByIdAsync(request.Id);

                if (motorcycle is null)
                    return new GenericCommandResult(false, "Moto não encontrada", null!);

                if (motorcycle.Year == exactYear)
                    motorcycle.IsFrom2024 = true;
                else
                    motorcycle.IsFrom2024 = false;

                _logger.LogInformation("Ano da moto validado com sucesso. motorcycle Id: {motoId}, isFrom2024: {isFrom2024}", request.Id, motorcycle.IsFrom2024);
                await _repository.UpdateAsync(motorcycle);

                return new GenericCommandResult(true, "Ano validado com sucesso", motorcycle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar o ano da moto: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao validar o ano da moto");
            }
            

        }

        private async Task PublishMessagePubSub(string id)
        {
            var messagObjetct = new { Id = id };

            var jsonString = JsonSerializer.Serialize(messagObjetct);

            var message = new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8(jsonString)
            };

            TopicName topicName = TopicName.FromProjectTopic(
                _configuration["PubSub:ProjectId"],
                _configuration["PubSub:TopicId"]);

            await _publisherClient.PublishAsync(message);
        }
    }
}
