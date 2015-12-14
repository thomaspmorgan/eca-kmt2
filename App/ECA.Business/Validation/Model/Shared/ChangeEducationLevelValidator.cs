using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ChangeEducationLevelValidator : AbstractValidator<ChangeEducationLevel>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public ChangeEducationLevelValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Change Education Level: Print form option is required");
            RuleFor(student => student.educationalInfo).SetValidator(new EducationalInfoValidator());
            RuleFor(student => student.financialInfo).SetValidator(new FinancialInfoValidator());
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Change Education Level: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}