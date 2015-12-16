using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class EVFinancialInfoValidator : AbstractValidator<EVFinancialInfo>
    {
        public EVFinancialInfoValidator()
        {
            RuleFor(student => student.receivedUSGovtFunds).NotNull().WithMessage("Financial Info: Received US Govt Funds option is required");
            RuleFor(student => student.otherFunds).NotNull().WithMessage("Financial Info: Other Financial Support is required");
        }
    }
}