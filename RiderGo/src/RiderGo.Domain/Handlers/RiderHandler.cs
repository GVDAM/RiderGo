using RiderGo.Domain.Commands.Motorcycle;
using RiderGo.Domain.Commands.Rider;
using Microsoft.Extensions.Logging;
using RiderGo.Domain.Repositories;
using RiderGo.Domain.Extension;
using RiderGo.Domain.Commands;
using RiderGo.Domain.Entities;
using RiderGo.Domain.Storage;
using RiderGo.Domain.Consts;
using MediatR;

namespace RiderGo.Domain.Handlers
{
    public class RiderHandler :
        IRequestHandler<CreateRiderCommand, GenericCommandResult>,
        IRequestHandler<UpdateMotorcycleCnhImageCommand, GenericCommandResult>
    {
        private readonly IRiderRepository _riderRepository;
        private readonly IFileStorage _fileStorage;
        private readonly ILogger<RiderHandler> _logger;

        public RiderHandler(IRiderRepository riderRepository, 
            IFileStorage fileStorage, 
            ILogger<RiderHandler> logger)
        {
            _riderRepository = riderRepository;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        public async Task<GenericCommandResult> Handle(CreateRiderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Validando tipo de CNH");
                if (!IsCnhValid(request.CnhType.ToUpper()))
                    return new GenericCommandResult(false, "Dados inválidos");


                _logger.LogInformation("Validando se entregador já está cadastrado");
                if (await _riderRepository.AlreadyRegisteredAsync(request.Id, request.CnhNumber, request.CNPJ))
                    return new GenericCommandResult(false, "Dados inválidos");


                _logger.LogInformation("Validando se entregador é maior de idade");
                if (request.Birth > DateTime.UtcNow.AddYears(-18).Date)
                    return new GenericCommandResult(false, "Dados inválidos");


                var imageBytes = GetBytesFromBase64String(request.CnhImage);

                _logger.LogInformation("Validando dados da imagem da CNH");
                if (imageBytes is null || imageBytes.Length == 0)
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Validando extensão da imagem da CNH");
                var fileExtension = GetExtensionFromFile(imageBytes);
                if (string.IsNullOrEmpty(fileExtension))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Fazendo upload da imagem da CNH");
                var cnhImageFileName = $"{request.Id}_cnh_{DateTime.UtcNow.Ticks}.{fileExtension}";
                var cnhImageUrl = await _fileStorage.UploadAsync(cnhImageFileName, imageBytes);


                var rider = new Rider()
                {
                    Id = request.Id,
                    Name = request.Name,
                    CNPJ = request.CNPJ.CleanUpCnpj(),
                    Birth = request.Birth,
                    CnhNumber = request.CnhNumber,
                    CnhType = request.CnhType.ToUpper(),
                    CnhImageUrl = cnhImageUrl,
                };

                _logger.LogInformation("Criando entregador");
                await _riderRepository.CreateAsync(rider);


                _logger.LogInformation("Entregador criado com sucesso");
                return new GenericCommandResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar entregador: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao criar entregador");
            }
            
        }

        public async Task<GenericCommandResult> Handle(UpdateMotorcycleCnhImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Validando se entregador existe");
                var motorcycle = await _riderRepository.GetByIdAsync(request.Id);

                if (motorcycle is null)
                    return new GenericCommandResult(false, "Dados inválidos");

                var imageBytes = GetBytesFromBase64String(request.CnhImage);

                _logger.LogInformation("Validando dados da imagem da CNH");
                if (imageBytes is null || imageBytes.Length == 0)
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Validando extensão da imagem da CNH");
                var fileExtension = GetExtensionFromFile(imageBytes);
                if (string.IsNullOrEmpty(fileExtension))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Fazendo upload da imagem da CNH");
                var cnhImageFileName = $"{request.Id}_cnh_{DateTime.UtcNow.Ticks}.{fileExtension}";
                var cnhImageUrl = await _fileStorage.UploadAsync(cnhImageFileName, imageBytes);

                motorcycle.CnhImageUrl = cnhImageUrl;

                _logger.LogInformation("Atualizando imagem da CNH do entregador");
                await _riderRepository.UpdateCnhImageAsync(motorcycle.Id, motorcycle.CnhImageUrl);

                return new GenericCommandResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar imagem da CNH do entregador: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao atualizar imagem da CNH do entregador");
            }
        }

        private byte[] GetBytesFromBase64String(string base64String)
        {
            // Limpar cabeçalho base64, caso exista
            if (base64String.Contains("base64,"))
                base64String = base64String.Split("base64,")[1];

            return Convert.FromBase64String(base64String);
        }

        private string GetExtensionFromFile(byte[] cnhImageBytes)
        {
            var extension = "";

            // Fazer a validação de extensão do arquivo pela assinatura mágica
            foreach (var signature in FileSignature.ImageFileSignatures)
            {
                if (cnhImageBytes.Take(signature.Key.Length).SequenceEqual(signature.Key))
                {
                    extension = signature.Value;
                    break;
                }
            }

            return extension;
        }

        private bool IsCnhValid(string cnhType)
            => cnhType is CnhTypeConst.A or CnhTypeConst.B or CnhTypeConst.AB;
    }
}
