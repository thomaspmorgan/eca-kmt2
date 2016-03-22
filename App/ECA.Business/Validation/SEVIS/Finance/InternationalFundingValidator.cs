using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// An InternationalValidator is used to validate international organization participant funding.
    /// </summary>
    public class InternationalFundingValidator : AbstractValidator<InternationalFunding>
    {
        /// <summary>
        /// The max length of an other organizations' name.
        /// </summary>
        public const int OTHER_ORG_NAME_MAX_LENGTH = 60;

        /// <summary>
        /// The max length of an org code.
        /// </summary>
        public const int ORG_CODE_MAX_LENGTH = 6;

        /// <summary>
        /// The max funding length.
        /// </summary>
        public const int AMOUNT_MAX_LENGTH = 8;

        /// <summary>
        /// The amount regular expression value.
        /// </summary>
        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        /// <summary>
        /// The other org code.
        /// </summary>
        public static string OTHER_ORG_CODE = InternationalOrgCodeType.OTHER.ToString();

        /// <summary>
        /// The error message to return when an org 1 code is not specified.
        /// </summary>
        public static string ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE = String.Format("The international organization 1 funding the participant must have an agency code set and it may be {0} characters.", ORG_CODE_MAX_LENGTH);

        /// <summary>
        /// The error message to return when an org 2 code is not specified.
        /// </summary>
        public static string ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE = String.Format("The international organization 2 funding the participant must have an agency code set and it may be {0} characters.", ORG_CODE_MAX_LENGTH);

        /// <summary>
        /// The error message to return when the first international organization is specified as other and the other name of the organization is invalid.
        /// </summary>
        public static string OTHER_ORG_1_NAME_REQUIRED = String.Format("The name of the international organization 1 funding the participant is set to {0}; therefore, a name of the agency must be supplied.  The name can be {1} characters.", OTHER_ORG_CODE, OTHER_ORG_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to return when the second international organization is specified as other and the other name of the organization is invalid.
        /// </summary>
        public static string OTHER_ORG_2_NAME_REQUIRED = String.Format("The name of the international organization 2 funding the participant is set to {0}; therefore, a name of the agency must be supplied.  The name can be {1} characters.", OTHER_ORG_CODE, OTHER_ORG_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to format when a funding amount is invalid.
        /// </summary>
        public const string AMOUNT_ERROR_MESSAGE = "The international organization {0} funding amount is required and can be up to {1} digits.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public InternationalFundingValidator()
        {
            When(visitor => !String.Equals(visitor.Org1, OTHER_ORG_CODE, StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.Org1)
                    .NotNull()
                    .WithMessage(ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE)
                    .WithState(x => new FundingErrorPath())
                    .Length(1, ORG_CODE_MAX_LENGTH)
                    .WithMessage(ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE)
                    .WithState(x => new FundingErrorPath());
            });

            When(visitor => String.Equals(visitor.Org1, OTHER_ORG_CODE, StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.OtherName1)
                    .NotNull()
                    .WithMessage(OTHER_ORG_1_NAME_REQUIRED)
                    .WithState(x => new FundingErrorPath())
                    .Length(1, OTHER_ORG_NAME_MAX_LENGTH)
                    .WithMessage(OTHER_ORG_1_NAME_REQUIRED)
                    .WithState(x => new FundingErrorPath());
            });

            When(visitor => !String.Equals(visitor.Org2, OTHER_ORG_CODE, StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.Org2)
                    .Length(1, ORG_CODE_MAX_LENGTH)
                    .WithMessage(ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE)
                    .WithState(x => new FundingErrorPath());
            });

            When(visitor => String.Equals(visitor.Org2, OTHER_ORG_CODE, StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.OtherName2)
                    .NotNull()
                    .WithMessage(OTHER_ORG_2_NAME_REQUIRED)
                    .WithState(x => new FundingErrorPath())
                    .Length(1, OTHER_ORG_NAME_MAX_LENGTH)
                    .WithMessage(OTHER_ORG_2_NAME_REQUIRED)
                    .WithState(x => new FundingErrorPath());
            });

            When(visitor => !String.IsNullOrWhiteSpace(visitor.Org2), () =>
            {
                RuleFor(x => x.Amount2)
                    .NotNull()
                    .WithMessage(AMOUNT_ERROR_MESSAGE, "2", AMOUNT_MAX_LENGTH)
                    .WithState(x => new FundingErrorPath())
                    .Matches(new Regex(AMOUNT_REGEX))
                    .WithMessage(AMOUNT_ERROR_MESSAGE, "2", AMOUNT_MAX_LENGTH)
                    .WithState(x => new FundingErrorPath());
            });


            RuleFor(visitor => visitor.Amount1)
                .NotNull()
                .WithMessage(AMOUNT_ERROR_MESSAGE, "1", AMOUNT_MAX_LENGTH)
                .WithState(x => new FundingErrorPath())
                .Matches(new Regex(AMOUNT_REGEX))
                .WithMessage(AMOUNT_ERROR_MESSAGE, "1", AMOUNT_MAX_LENGTH)
                .WithState(x => new FundingErrorPath());
            When(x => !String.IsNullOrWhiteSpace(x.Amount2), () =>
            {
                RuleFor(x => x.Org2)
                    .NotNull()
                    .WithMessage(ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE)
                    .WithState(x => new FundingErrorPath());
            });
        }
    }
}