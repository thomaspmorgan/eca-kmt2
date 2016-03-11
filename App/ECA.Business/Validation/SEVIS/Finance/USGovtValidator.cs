using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace ECA.Business.Validation.Sevis.Finance
{
    public class USGovtValidator : AbstractValidator<USGovt>
    {
        public const int OTHER_ORG_NAME_MAX_LENGTH = 60;

        public const int ORG_CODE_MAX_LENGTH = 5;

        public const int AMOUNT_MAX_LENGTH = 8;

        public const string AMOUNT_REGEX = @"^\d{1,8}$";

        public const string OTHER_ORG_CODE = "OTHER";

        public static string ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE = String.Format("U.S. Gov Funds: The U.S. Government Agency 1 must have an agency code set and it may be {0} characters.", ORG_CODE_MAX_LENGTH);

        public static string ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE = String.Format("U.S. Gov Funds: The U.S. Government Agency 2 must have an agency code set and it may be {0} characters.", ORG_CODE_MAX_LENGTH);

        public static string OTHER_ORG_1_NAME_REQUIRED = String.Format("U.S. Gov Funds: The U.S. Government Agency 1 is set to other; therefore, a name of the agency must be supplied.  The name can be {0} characters.", OTHER_ORG_NAME_MAX_LENGTH);

        public static string OTHER_ORG_2_NAME_REQUIRED = String.Format("U.S. Gov Funds: The U.S. Government Agency 2 is set to other; therefore, a name of the agency must be supplied.  The name can be {0} characters.", OTHER_ORG_NAME_MAX_LENGTH);

        public static string AMOUNT_ERROR_MESSAGE = String.Format("U.S. Gov Funds: U.S. Government Org Amount is required and can be up to {0} digits.", AMOUNT_MAX_LENGTH);

        public USGovtValidator()
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

            When(visitor => visitor.Org2 != null && !String.Equals(visitor.Org2, OTHER_ORG_CODE, StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.Org2)
                    .Length(1, ORG_CODE_MAX_LENGTH)
                    .WithMessage(ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE)
                    .WithState(x => new FundingErrorPath());
            });

            When(visitor => visitor.Org2 != null && String.Equals(visitor.Org2, OTHER_ORG_CODE, StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.OtherName2)
                    .NotNull()
                    .WithMessage(OTHER_ORG_2_NAME_REQUIRED)
                    .WithState(x => new FundingErrorPath())
                    .Length(1, OTHER_ORG_NAME_MAX_LENGTH)
                    .WithMessage(OTHER_ORG_2_NAME_REQUIRED)
                    .WithState(x => new FundingErrorPath());
            });

            When(visitor => !String.IsNullOrWhiteSpace(visitor.Org2) || !String.IsNullOrWhiteSpace(visitor.OtherName2), () =>
            {
                RuleFor(x => x.Amount2)
                .NotNull()
                .WithMessage(AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());
            });

            RuleFor(visitor => visitor.Amount1)
                .NotNull()
                .WithMessage(AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath())
                .Matches(new Regex(AMOUNT_REGEX))
                .WithMessage(AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());

            When(visitor => visitor.Amount2 != null, () =>
            {
                RuleFor(visitor => visitor.Amount2)
                .Matches(new Regex(AMOUNT_REGEX))
                .WithMessage(AMOUNT_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());
            });
        }
    }
}