using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CreateDependentValidator : AbstractValidator<CreateDependent>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public CreateDependentValidator()
        {
            RuleFor(dependent => dependent.Dependent).NotNull().WithMessage("Student Dependent: Dependent is required").SetValidator(new AddDependentValidator());
            RuleFor(dependent => dependent.AddTIPP).NotNull().WithMessage("Student Dependent: T/IPP information is required").SetValidator(new AddTippValidator());
        }
    }
}