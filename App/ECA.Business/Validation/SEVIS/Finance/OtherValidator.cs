using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// The OtherValidator is used to validate an other organization's funding to an exchange visitor participant.
    /// </summary>
    public class OtherValidator : AbstractValidator<Other>
    {
        /// <summary>
        /// The max length of the organization name.
        /// </summary>
        public const int NAME_MAX_LENGTH = 60;

        /// <summary>
        /// The amount max length.
        /// </summary>
        public const int AMOUNT_MAX_LENGTH = 8;

        /// <summary>
        /// The amount regular expression value.
        /// </summary>
        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        /// <summary>
        /// The error message to return when another organization funding the participant does not have a name supplied.
        /// </summary>
        public static string OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE = string.Format("The other organization funding the participant must have a name and can be up to {0} characters.", NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to return when the funding amount is invalid.
        /// </summary>
        public static string AMOUNT_ERROR_MESSAGE = string.Format("The other organization funding the participant is required and may be up to {0} digits.", AMOUNT_MAX_LENGTH);

        /// <summary>
        /// Creates a default instance.
        /// </summary>
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