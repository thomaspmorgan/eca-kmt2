using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class FundingValidator : AbstractValidator<Funding>
    {
        public FundingValidator()
        {
            RuleFor(student => student.Personal).NotNull().WithMessage("Financial Funding: Personal is required");
            When(student => student.School != null, () => {
                RuleFor(student => student.School).SetValidator(new SchoolValidator());
            });
            When(student => student.Other != null, () => {
                RuleFor(student => student.Other).SetValidator(new FundingOtherValidator());
            });            
        }
    }
}