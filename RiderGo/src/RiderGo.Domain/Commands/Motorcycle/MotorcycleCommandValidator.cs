using FluentValidation;

namespace RiderGo.Domain.Commands.Motorcycle
{
    public class MotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
    {
        public MotorcycleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year);

            RuleFor(x => x.Model)
                .NotEmpty();

            RuleFor(x => x.Plate)
                .NotEmpty();
        }
    }

    public class UpdateMotorcycleCommandValidator : AbstractValidator<UpdateMotorcycleCommand>
    {
        public UpdateMotorcycleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Plate)
                .NotEmpty();
        }
    }
}
