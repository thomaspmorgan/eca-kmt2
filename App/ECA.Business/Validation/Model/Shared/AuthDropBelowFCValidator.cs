using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AuthDropBelowFCValidator : AbstractValidator<AuthDropBelowFC>
    {
        public AuthDropBelowFCValidator()
        {
            RuleFor(student => student.addAuthDrop).SetValidator(new AddAuthDropValidator());
            RuleFor(student => student.cancelAuthDrop).SetValidator(new CancelAuthDropValidator());
            RuleFor(student => student.editAuthDrop).SetValidator(new EditAuthDropValidator());
        }
    }
}