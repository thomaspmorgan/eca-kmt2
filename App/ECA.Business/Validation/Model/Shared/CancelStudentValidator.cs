using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CancelStudentValidator : AbstractValidator<CancelStudent>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public CancelStudentValidator()
        {
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Cancel Student: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}