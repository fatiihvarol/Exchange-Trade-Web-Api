using FluentValidation;
using Web.Schema;

namespace Web.Business.Validation
{
    public class ShareValidator : AbstractValidator<ShareRequest>
    {
        public ShareValidator()
        {
            RuleFor(share => share.Symbol)
                .NotEmpty().WithMessage("Symbol is required.")
                .Length(3).WithMessage("Symbol must be exactly 3 characters.")
                .Must(BeAllCapitalLetters).WithMessage("Symbol should be all capital letters.");

            RuleFor(share => share.CurrentPrice)
                .GreaterThan(0).WithMessage("Current price must be greater than 0.")
                .ScalePrecision(2, 5).WithMessage("Rate of the share should have exactly 2 decimal digits.");

            RuleFor(share => share.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be greater than or equal to 0.");
        }

        private bool BeAllCapitalLetters(string symbol)
        {
            return !string.IsNullOrWhiteSpace(symbol) && symbol.All(char.IsUpper);
        }
    }
}