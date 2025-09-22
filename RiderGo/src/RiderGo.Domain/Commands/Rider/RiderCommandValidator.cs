using RiderGo.Domain.Extension;
using FluentValidation;

namespace RiderGo.Domain.Commands.Rider
{
    public class RiderCommandValidator : AbstractValidator<CreateRiderCommand>
    {
        public RiderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.CNPJ)
                .NotEmpty()
                .Must(x => x.IsValidCnpj());

            RuleFor(x => x.Birth)
                .NotEmpty();

            RuleFor(x => x.CnhNumber)
                .NotEmpty()
                .Length(11);

            RuleFor(x => x.CnhType)
                .NotEmpty();

            RuleFor(x => x.CnhImage)
                .NotEmpty();
        }
    }
}
