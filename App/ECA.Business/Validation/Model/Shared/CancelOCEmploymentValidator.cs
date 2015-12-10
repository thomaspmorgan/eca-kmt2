using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelOCEmploymentValidator : AbstractValidator<CancelOCEmployment>
    {
        public const int EMPLOYMENT_TYPE_LENGTH = 2;

        public CancelOCEmploymentValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Cancel OC Employment: Print form option is required");
            RuleFor(student => student.EmploymentType).Length(EMPLOYMENT_TYPE_LENGTH).WithMessage("Cancel OC Employment: Employment Type code must be " + EMPLOYMENT_TYPE_LENGTH.ToString() + " characters");
        }
    }
}