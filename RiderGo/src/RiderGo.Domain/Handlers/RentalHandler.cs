using RiderGo.Domain.Commands.Rental;
using Microsoft.Extensions.Logging;
using RiderGo.Domain.Repositories;
using RiderGo.Domain.Commands;
using RiderGo.Domain.Entities;
using RiderGo.Domain.Consts;
using MediatR;

namespace RiderGo.Domain.Handlers
{
    public class RentalHandler :
        IRequestHandler<CreateRentalCommand, GenericCommandResult>,
        IRequestHandler<RentalReturnCommand, GenericCommandResult>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<RentalHandler> _logger;

        private const int BASIC_PLAN_7_DAYS = 7;

        public RentalHandler(IRentalRepository rentalRepository, ILogger<RentalHandler> logger)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
        }


        public async Task<GenericCommandResult> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Verificando se a moto está disponível para aluguel. motorcycle Id: {motorcycleId}", request.MotorcycleId);
                if (!await _rentalRepository.IsMotocycleAvaibleToRentAsync(request.MotorcycleId))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Verificando se a moto já está alugada no momento. motorcycle Id: {motorcycleId}", request.MotorcycleId);
                if (await _rentalRepository.IsMotorcycleCurrentlyRentedAsync(request.MotorcycleId))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Verificando se já existe um aluguel ativo para o entregador e a moto informados");
                if (await _rentalRepository.AlreadyExistRentAsync(request.RiderId, request.MotorcycleId))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Verificando se o entregador possui CNH do tipo A. rider Id: {riderId}", request.RiderId);
                if (!await _rentalRepository.HasRiderValidCnhTypeAsync(request.RiderId))
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Verificando se o plano é válido. plano: {plan}", request.Plan);
                if (!Plans.PlanValues.ContainsKey(request.Plan)) // TODO - Mover esse dicionário para o mongoDB ?
                    return new GenericCommandResult(false, "Dados inválidos");

                var rental = new Rental()
                {
                    Id = Guid.NewGuid(),
                    RiderId = request.RiderId,
                    MotorcycleId = request.MotorcycleId,
                    Plan = request.Plan,
                    StartDate = request.StartDate!.Value,
                    EndDate = request.EndDate!.Value,
                    ExpectedEndDate = request.ExpectedEndDate!.Value,
                    DailyRate = Plans.PlanValues[request.Plan]
                };

                _logger.LogInformation("Criando aluguel para o entregador. rider Id: {riderId}, motorcycle Id: {motorcycleId}", request.RiderId, request.MotorcycleId);
                await _rentalRepository.CreateRentalAsync(rental);

                _logger.LogInformation("Aluguel criado com sucesso. rental Id: {rentalId}", rental.Id);
                return new GenericCommandResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar aluguel: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao criar aluguel");
            }
        }

        public async Task<GenericCommandResult> Handle(RentalReturnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processando devolução de aluguel. rental Id: {rentalId}", request.Id);
                var rental = await _rentalRepository.GetRentalByIdAsync(request.Id);

                if (rental == null)
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Validando dados do aluguel para devolução. rental Id: {rentalId}", request.Id);
                if (rental.EndDate == null && rental.EndDate <= DateTime.MinValue)
                    return new GenericCommandResult(false, "Dados inválidos");

                _logger.LogInformation("Validando se a data de devolução é válida. rental Id: {rentalId}", request.Id);
                if (request.ReturnDate < rental.StartDate)
                    return new GenericCommandResult(false, "Dados inválidos");

                CalculateDaysTotalAmount(rental, request.ReturnDate);
                CalculateFine(rental, request.ReturnDate);

                rental.ReturnDate = request.ReturnDate;

                _logger.LogInformation("Atualizando dados do aluguel com a data de devolução. rental Id: {rentalId}", request.Id);
                await _rentalRepository.UpdateRentalAsync(rental);

                _logger.LogInformation("Devolução processada com sucesso. rental Id: {rentalId}", request.Id);
                return new GenericCommandResult(true, "Data de devolução informada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar devolução do aluguel: {mensagem}", ex.Message);
                return new GenericCommandResult(false, "Erro ao processar devolução do aluguel");
            }            
        }

        private void CalculateDaysTotalAmount(Rental rental, DateTime returnDate)
        {
            _logger.LogInformation("Calculando valor total do aluguel com base na data de devolução. rental Id: {rentalId}", rental.Id);

            var totalAmount = 0m;

            // Se devolvido no prazo ou após, cobra o valor total do plano
            _logger.LogInformation("Verificando se a devolução foi no prazo ou após. rental Id: {rentalId}", rental.Id);
            if (returnDate.Date >= rental.ExpectedEndDate.Date)
                totalAmount = rental.Plan * rental.DailyRate;
            // Se devolvido antes do prazo, cobra apenas os dias usados
            else
                totalAmount = (decimal)(returnDate.Date - rental.StartDate.Date).TotalDays * rental.DailyRate;

            rental.TotalAmount = totalAmount;
        }

        private void CalculateFine(Rental rental, DateTime returnDate)
        {
            _logger.LogInformation("Calculando multa do aluguel com base na data de devolução. rental Id: {rentalId}", rental.Id);

            var totalAmountFine = 0m;

            // Valor total dos dias restantes do aluguel e Aplica multa conforme plano
            _logger.LogInformation("Verificando se a devolução foi antes do prazo. rental Id: {rentalId}", rental.Id);
            if (returnDate.Date < rental.ExpectedEndDate.Date)
            {
                totalAmountFine = (rental.DailyRate * (decimal)(rental.ExpectedEndDate.Date - returnDate.Date).TotalDays);

                if (rental.Plan == BASIC_PLAN_7_DAYS)
                    totalAmountFine *= 0.20m;
                else
                    totalAmountFine *= 0.40m;

                _logger.LogInformation("Multa calculada com base no plano. rental Id: {rentalId}, multa: {multa}", rental.Id, totalAmountFine);
            }
            // Multa de R$50 por dia de atraso
            else if (returnDate.Date > rental.ExpectedEndDate.Date)
            {
                _logger.LogInformation("Calculando multa por dia de atraso. rental Id: {rentalId}", rental.Id);
                totalAmountFine = (decimal)(returnDate.Date - rental.ExpectedEndDate.Date).TotalDays * 50m;
            }

            rental.TotalAmount += totalAmountFine;
        }
    }
}
