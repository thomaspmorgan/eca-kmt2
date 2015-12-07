using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddOCEmploymentValidator : AbstractValidator<AddOCEmployment>
    {
        public const int EMPLOYMENT_TYPE_LENGTH = 2;
        public const int RECOMMEND_MAX_LENGTH = 250;

        public AddOCEmploymentValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("OC Employment: Print form option is required");
            RuleFor(student => student.EmploymentType).Length(EMPLOYMENT_TYPE_LENGTH).WithMessage("OC Employment: Employment Type code must be " + EMPLOYMENT_TYPE_LENGTH.ToString() + " characters");
            RuleFor(student => student.Recommendation).Length(0, RECOMMEND_MAX_LENGTH).WithMessage("OC Employment: Recommendation can be up to " + RECOMMEND_MAX_LENGTH.ToString() + " characters");
        }
    }
}