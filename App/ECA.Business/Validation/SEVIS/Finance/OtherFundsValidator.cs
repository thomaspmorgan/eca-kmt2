using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    public class OtherFundsValidator : AbstractValidator<OtherFunds>
    {
        /// <summary>
        /// The max length of amount.
        /// </summary>
        public const int AMOUNT_MAX_LENGTH = 8;

        /// <summary>
        /// The amount regular expression value.
        /// </summary>
        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        /// <summary>
        /// The error message to return when an exchange visitor government funding value is invalid.
        /// </summary>
        public static string EXCHANGE_VISITOR_GOVERNMENT_FUNDING_ERROR_MESSAGE = string.Format("The participant's exchange visitor government funding can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        /// <summary>
        /// The error message to return when an exchange visitor's binational funding is invalid.
        /// </summary>
        public static string BINATIONAL_ERROR_MESSAGE = string.Format("The participant's binational funding can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        /// <summary>
        /// The error message to return when an exchange's visitor's personal funding is invalid.
        /// </summary>
        public static string PERSONAL_ERROR_MESSAGE = string.Format("The participant's personal funding can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public OtherFundsValidator()
        {
            RuleFor(visitor => visitor.Other)
                .SetValidator(new OtherValidator())
                .When(visitor => visitor.Other != null);

            RuleFor(visitor => visitor.USGovt)
                .SetValidator(new USGovtValidator())
                .When(visitor => visitor.USGovt != null);

            RuleFor(visitor => visitor.International)
                .SetValidator(new InternationalValidator())
                .When(visitor => visitor.International != null);

            When(visitor => visitor.EVGovt != null, () =>
            {
                RuleFor(x => x.EVGovt)
                 .Matches(new Regex(AMOUNT_REGEX))
                 .WithMessage(EXCHANGE_VISITOR_GOVERNMENT_FUNDING_ERROR_MESSAGE)
                 .WithState(x => new FundingErrorPath());
            });

            When(visitor => visitor.BinationalCommission != null, () =>
            {
                RuleFor(x => x.BinationalCommission)
                 .Matches(new Regex(AMOUNT_REGEX))
                 .WithMessage(BINATIONAL_ERROR_MESSAGE)
                 .WithState(x => new FundingErrorPath());
            });
            When(visitor => visitor.Personal != null, () =>
            {
                RuleFor(x => x.Personal)
                 .Matches(new Regex(AMOUNT_REGEX))
                 .WithMessage(PERSONAL_ERROR_MESSAGE)
                 .WithState(x => new FundingErrorPath());
            });
        }
    }
}