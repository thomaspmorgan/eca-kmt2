using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Model.Shared
{
    public class OtherValidator : AbstractValidator<Other>
    {
        public const int NAME_MAX_LENGTH = 60;
        public const int AMOUNT_MAX_LENGTH = 8;

        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        public static string OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE = string.Format("Other Funds: The other organization name is required and must be {0} characters.", NAME_MAX_LENGTH);

        public static string AMOUNT_ERROR_MESSAGE = string.Format("Other Funds: The other fund amount is required and may be up to {0} digits.", AMOUNT_MAX_LENGTH);

        public OtherValidator()
        {
            RuleFor(visitor => visitor.Name)
                .NotNull()
                .WithMessage(OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath())
                .Length(1, NAME_MAX_LENGTH)
                .WithMessage(OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath()); 

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage(AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath())
                .Matches(new Regex(AMOUNT_REGEX))
                .WithMessage(AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());

        }
    }
}