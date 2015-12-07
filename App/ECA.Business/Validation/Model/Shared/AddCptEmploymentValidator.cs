using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddCptEmploymentValidator : AbstractValidator<AddCptEmployment>
    {
        public const int FPT_LENGTH = 2;
        public const int EMPLOYER_MAX_LENGTH = 100;
        public const int RELEVANCE_MAX_LENGTH = 250;
        public const int REMARKS_MAX_LENGTH = 200;

        public AddCptEmploymentValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("CPT Employment: Print form option is required");
            RuleFor(student => student.FullPartTimeIndicator).Length(FPT_LENGTH).WithMessage("CPT Employment: Full/Part-Time code must be " + FPT_LENGTH.ToString() + " characters");
            RuleFor(student => student.EmployerName).Length(0, EMPLOYER_MAX_LENGTH).WithMessage("CPT Employment: Employer Name can be up to " + EMPLOYER_MAX_LENGTH.ToString() + " characters");
            When(student => student.employerAddress != null, () =>
            {
                RuleFor(student => student.employerAddress).SetValidator(new EmployerAddressValidator());
            });
            RuleFor(student => student.CourseRelevance).Length(0, RELEVANCE_MAX_LENGTH).WithMessage("CPT Employment: Course Relevance can be up to " + RELEVANCE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("CPT Employment: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}