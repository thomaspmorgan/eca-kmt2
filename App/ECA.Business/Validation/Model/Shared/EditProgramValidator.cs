using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class EditProgramValidator : AbstractValidator<EditProgram>
    {
        public const int MAJOR_MAX_LENGTH = 7;
        public const int REMARKS_MAX_LENGTH = 500;

        public EditProgramValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Edit Program: Print form option is required");
            RuleFor(student => student.PrimaryMajor).Length(0, MAJOR_MAX_LENGTH).WithMessage("Edit Program: Primary Major can be up to " + MAJOR_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SecondMajor).Length(0, MAJOR_MAX_LENGTH).WithMessage("Edit Program: Secondary Major can be up to " + MAJOR_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Minor).Length(0, MAJOR_MAX_LENGTH).WithMessage("Edit Program: Minor can be up to " + MAJOR_MAX_LENGTH.ToString() + " characters");
            When(student => student.engProficiency != null, () =>
            {
                RuleFor(student => student.engProficiency).SetValidator(new EngProficiencyValidator());
            });
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Edit Program: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}