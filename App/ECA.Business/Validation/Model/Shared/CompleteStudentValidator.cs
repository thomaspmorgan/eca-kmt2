using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CompleteStudentValidator : AbstractValidator<CompleteStudent>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public CompleteStudentValidator()
        {
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Complete Student: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}