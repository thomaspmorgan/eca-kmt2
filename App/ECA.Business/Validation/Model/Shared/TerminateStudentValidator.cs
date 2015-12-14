using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class TerminateStudentValidator : AbstractValidator<TerminateStudent>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public TerminateStudentValidator()
        {
            RuleFor(student => student.OtherRemarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Terminate Student: Other Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Terminate Student: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}