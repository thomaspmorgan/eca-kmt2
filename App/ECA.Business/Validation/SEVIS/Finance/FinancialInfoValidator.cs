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
        public static string PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE = string.Format("The participant's program sponsor funds can be up to {0} characters.", SPONSOR_MAX_LENGTH);

        /// <summary>
        /// The program sponsor funds regex.
        /// </summary>
        public static Regex PROGRAM_SPONSOR_FUNDS_REGEX = new Regex(@"^\d{0,8}$");

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public FinancialInfoValidator()
        {
            RuleFor(visitor => visitor.ProgramSponsorFunds)
                .Matches(PROGRAM_SPONSOR_FUNDS_REGEX)
                .WithMessage(PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());

            RuleFor(visitor => visitor.OtherFunds)
                .SetValidator(new OtherFundsValidator())
                .When(x => x.OtherFunds != null);
        }
    }
}