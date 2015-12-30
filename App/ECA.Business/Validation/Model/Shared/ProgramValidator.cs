using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator()
        {
            RuleFor(student => student.Extension).SetValidator(new ProgramExtensionValidator()).When(student => student.Extension != null);
            RuleFor(student => student.Shorten).SetValidator(new ShortenProgramValidator()).When(student => student.Shorten != null);
        }
    }
}