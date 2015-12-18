using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FinancialInfoValidator : AbstractValidator<FinancialInfo>
    {
        public const int SPONSOR_MAX_LENGTH = 8;

        public FinancialInfoValidator()
        {
            RuleFor(student => student.ReceivedUSGovtFunds).NotNull().WithMessage("Financial Info: Received US Govt Funds option is required");
            RuleFor(student => student.ProgramSponsorFunds).Length(0, SPONSOR_MAX_LENGTH).WithMessage("Financial Info: Program Sponsor Funds can be up to " + SPONSOR_MAX_LENGTH.ToString() + " characters");
        }
    }
}