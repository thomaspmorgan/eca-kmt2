using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CreateDependentValidator : AbstractValidator<CreateDependent>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public CreateDependentValidator()
        {
            RuleFor(dependent => dependent.Dependent).NotNull().WithMessage("Create Student Dependent: Dependent is required").SetValidator(new AddDependentValidator());
            RuleFor(dependent => dependent.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Dependent: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }        
    }
}