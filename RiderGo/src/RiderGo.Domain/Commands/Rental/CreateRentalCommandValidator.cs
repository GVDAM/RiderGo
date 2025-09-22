using FluentValidation;

namespace RiderGo.Domain.Commands.Rental
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(x => x.RiderId)
                .NotEmpty();

            RuleFor(x => x.MotorcycleId)
                .NotEmpty();

            RuleFor(x => x.Plan)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.StartDate!.Value.Date)
                .Equal(DateTime.Now.AddDays(1).Date)
                .When(x => x.StartDate.HasValue);

            RuleFor(x => x.EndDate)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ExpectedEndDate)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue);
        }
    }

    public class RentalReturnCommandValidator : AbstractValidator<RentalReturnDto>
    {
        public RentalReturnCommandValidator()
        {
            RuleFor(x => x.ReturnDate)
                .NotNull();
        }
    }
}
