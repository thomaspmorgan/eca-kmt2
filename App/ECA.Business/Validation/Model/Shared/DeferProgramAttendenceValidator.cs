using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class DeferProgramAttendenceValidator : AbstractValidator<DeferProgramAttendence>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public DeferProgramAttendenceValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Defer Program Attendence: Print form option is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Defer Program Attendence: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}