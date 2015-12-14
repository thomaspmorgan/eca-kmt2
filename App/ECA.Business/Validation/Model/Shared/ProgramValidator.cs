using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator()
        {
            When(student => student.cancelExtension != null, () =>
            {
                RuleFor(student => student.cancelExtension).SetValidator(new CancelProgramExtensionValidator());
            });
            When(student => student.deferAttendence != null, () =>
            {
                RuleFor(student => student.deferAttendence).SetValidator(new DeferProgramAttendenceValidator());
            });
            When(student => student.edit != null, () =>
            {
                RuleFor(student => student.edit).SetValidator(new EditProgramValidator());
            });
            When(student => student.extension != null, () =>
            {
                RuleFor(student => student.extension).SetValidator(new ProgramExtensionValidator());
            });
            When(student => student.shorten != null, () =>
            {
                RuleFor(student => student.shorten).SetValidator(new ShortenProgramValidator());
            });
        }
    }
}