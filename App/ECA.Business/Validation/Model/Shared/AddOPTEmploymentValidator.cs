using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddOPTEmploymentValidator : AbstractValidator<AddOPTEmployment>
    {
        public const int FPT_LENGTH = 2;
        public const int EMPLOYER_MAX_LENGTH = 100;
        public const int RELEVANCE_MAX_LENGTH = 250;
        public const int COMPLETION_TYPE_LENGTH = 2;
        public const int STUDENT_REMARKS_MAX_LENGTH = 500;
        public const int REMARKS_MAX_LENGTH = 250;

        public AddOPTEmploymentValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("OPT Employment: Print form option is required");
            RuleFor(student => student.FullPartTimeIndicator).Length(FPT_LENGTH).WithMessage("OPT Employment: Full/Part-Time code must be " + FPT_LENGTH.ToString() + " characters");
            RuleFor(student => student.EmployerName).Length(0, EMPLOYER_MAX_LENGTH).WithMessage("OPT Employment: Employer Name can be up to " + EMPLOYER_MAX_LENGTH.ToString() + " characters");
            When(student => student.employerAddress != null, () =>
            {
                RuleFor(student => student.employerAddress).SetValidator(new EmployerAddressValidator());
            });
            RuleFor(student => student.CourseRelevance).Length(0, RELEVANCE_MAX_LENGTH).WithMessage("OPT Employment: Course Relevance can be up to " + RELEVANCE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.CompletionType).Length(COMPLETION_TYPE_LENGTH).WithMessage("OPT Employment: Completion Type code must be " + COMPLETION_TYPE_LENGTH.ToString() + " characters");
            RuleFor(student => student.StudentRemarks).Length(0, STUDENT_REMARKS_MAX_LENGTH).WithMessage("OPT Employment: Student Remarks can be up to " + STUDENT_REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("OPT Employment: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}