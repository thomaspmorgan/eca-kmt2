using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator()
        {
            RuleFor(student => student.CancelExtension).SetValidator(new CancelProgramExtensionValidator()).When(student => student.CancelExtension != null);
            RuleFor(student => student.DeferAttendence).SetValidator(new DeferProgramAttendenceValidator()).When(student => student.DeferAttendence != null);
            RuleFor(student => student.Edit).SetValidator(new EditProgramValidator()).When(student => student.Edit != null);
            RuleFor(student => student.Extension).SetValidator(new ProgramExtensionValidator()).When(student => student.Extension != null);
            RuleFor(student => student.Shorten).SetValidator(new ShortenProgramValidator()).When(student => student.Shorten != null);
        }
    }
}