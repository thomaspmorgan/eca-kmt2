using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// A USGovtValidator is used to validation us government agency participant funding.
    /// </summary>
    public class USGovernmentFundingValidator : AbstractValidator<USGovernmentFunding>
    {
        /// <summary>
        /// The max length of the other us government agency funding.
        /// </summary>
        public const int OTHER_ORG_NAME_MAX_LENGTH = 60;

        /// <summary>
        /// The max length of an org code.
        /// </summary>
        public const int ORG_CODE_MAX_LENGTH = 5;

        /// <summary>
        /// The max length of an amount.
        /// </summary>
        public const int AMOUNT_MAX_LENGTH = 8;

        /// <summary>
        /// The amount regex value.
        /// </summary>
        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        /// <summary>
        /// The other org code.
        /// </summary>
        public static string OTHER_ORG_CODE = GovAgencyCodeType.OTHER.ToString();

        /// <summary>
        /// The error message to return when the participant's us gov agency 1 code is not set.
        /// </summary>
        public static string ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE = String.Format("The U.S. Government Agency 1 funding the participant must have an agency code set and it may be {0} characters.", ORG_CODE_MAX_LENGTH);

        /// <summary>
        /// The error message to return when the participan't us gov agency 2 code is not set.
        /// </summary>
        public static string ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE = String.Format("The U.S. Government Agency 2 funding the participant must have an agency code set and it may be {0} characters.", ORG_CODE_MAX_LENGTH);

        /// <summary>
        /// The error message to return when a us gov agency 1 code is set to other but a name is not set.
        /// </summary>
        public static string OTHER_ORG_1_NAME_REQUIRED = String.Format("The name of the U.S. Government Agency 1 funding the participant is set to {0}; therefore, a name of the agency must be supplied.  The name can be {1} characters.", OTHER_ORG_CODE, OTHER_ORG_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to return when a us gov agency 2 code is set to other but a name is not set.
        /// </summary>
        public static string OTHER_ORG_2_NAME_REQUIRED = String.Format("The name of the U.S. Government Agency 2 funding the participant is set to {0}; therefore, a name of the agency must be supplied.  The name can be {1} characters.", OTHER_ORG_CODE, OTHER_ORG_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to format when a us government agency's funding amount is not set.
        /// </summary>
        public const string AMOUNT_ERROR_MESSAGE = "The U.S. Government Org {0} funding amount is required and can be up to {1} digits.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public USGovernmentFundingValidator()
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