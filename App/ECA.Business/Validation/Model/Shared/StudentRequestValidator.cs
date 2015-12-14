using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class StudentRequestValidator : AbstractValidator<StudentRequest>
    {
        public const int STATUS_LENGTH = 1;

        public StudentRequestValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Student Request: Print form option is required");
            When(student => student.capGapExtension != null, () =>
            {
                RuleFor(student => student.capGapExtension).SetValidator(new CapGapExtensionValidator());
            });
            RuleFor(student => student.Status).Length(0, STATUS_LENGTH).WithMessage("Student Request: Status can be up to " + STATUS_LENGTH.ToString() + " characters");
        }
    }
}