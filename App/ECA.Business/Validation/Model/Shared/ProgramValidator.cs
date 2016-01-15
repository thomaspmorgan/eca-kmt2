using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator()
        {
            RuleFor(program => program.Extension).SetValidator(new ProgramExtensionValidator()).When(program => program.Extension != null);
            RuleFor(program => program.Shorten).SetValidator(new ShortenProgramValidator()).When(program => program.Shorten != null);
        }
    }
}