using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FinancialInfoValidator : AbstractValidator<FinancialInfo>
    {
        public const int SPONSOR_MAX_LENGTH = 8;

        public static string PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE = string.Format("Financial Info: Program Sponsor Funds can be up to {0} characters", SPONSOR_MAX_LENGTH);

        public const string OTHER_FUNDS_ERROR_MESSAGE = "Financial Info: Other Funds is required.";

        public FinancialInfoValidator()
        {
            RuleFor(visitor => visitor.ProgramSponsorFunds)
                .Length(0, SPONSOR_MAX_LENGTH)
                .WithMessage(PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE)
                .WithState(x => new FundingErrorPath());
            RuleFor(visitor => visitor.OtherFunds)
                .SetValidator(new OtherFundsValidator())
                .When(x => x.OtherFunds != null);
        }
    }
}