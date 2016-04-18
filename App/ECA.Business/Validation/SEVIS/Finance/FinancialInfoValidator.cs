using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// The FinancialInfoValidator is used to validate financial information for a sevis participant.
    /// </summary>
    public class FinancialInfoValidator : AbstractValidator<FinancialInfo>
    {
        /// <summary>
        /// The max length a program sponsfor funding value can be.
        /// </summary>
        public const int SPONSOR_MAX_LENGTH = 8;

        /// <summary>
        /// The error message to return when a program sponsor funds value is invalid.
        /// </summary>
        public static string PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE = string.Format("The participant's program sponsor funds can be up to {0} digits.", SPONSOR_MAX_LENGTH);

        /// <summary>
        /// The program sponsor funds regex.
        /// </summary>
        public static Regex PROGRAM_SPONSOR_FUNDS_REGEX = new Regex(@"^\d{0,8}$");

        /// <summary>
        /// The error message to return when the total funding is less than the minimum amount.
        /// </summary>
        public const string FUNDING_LESS_THAN_MINIMUM_AMOUNT_ERROR_MESSAGE = "The total funding must be at least $1.00.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public FinancialInfoValidator()
        {
            RuleFor(visitor => visitor.ProgramSponsorFunds)
                .Matches(PROGRAM_SPONSOR_FUNDS_REGEX)
                .WithMessage(PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());

            RuleFor(x => x)
                .Must(x => x.GetTotalFunding() >= 1.0m)
                .WithMessage(FUNDING_LESS_THAN_MINIMUM_AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());            

            RuleFor(visitor => visitor.OtherFunds)
                .SetValidator(new OtherFundsValidator())
                .When(x => x.OtherFunds != null);
        }
    }
}