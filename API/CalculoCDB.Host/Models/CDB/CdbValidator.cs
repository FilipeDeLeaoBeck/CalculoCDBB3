using FluentValidation;

namespace CalculoCDB.API.Models.CDB
{
    public class CdbValidator : AbstractValidator<CdbRequest>
    {
        public CdbValidator()
        {
            RuleFor(x => x.InitialValue)
                .GreaterThan(0.0M)
                .WithMessage("O valor inicial deve ser positivo.");

            RuleFor(x => x.Months)
                .GreaterThan(1)
                .WithMessage("O prazo em meses deve ser maior que 1(um).");
        }
    }
}
