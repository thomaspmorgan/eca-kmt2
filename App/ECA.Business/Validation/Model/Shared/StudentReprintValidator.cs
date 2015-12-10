using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class StudentReprintValidator : AbstractValidator<StudentReprint>
    {
        public const int REMARKS_MAX_LENGTH = 500;
        public const int REASON_LENGTH = 2;

        public StudentReprintValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Student Reprint: Print form option is required");
            RuleFor(student => student.Reason).Length(0, REASON_LENGTH).WithMessage("Student Reprint: Reason can be up to " + REASON_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Student Reprint: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}