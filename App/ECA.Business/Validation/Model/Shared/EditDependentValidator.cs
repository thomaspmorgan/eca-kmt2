using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class EditDependentValidator : AbstractValidator<EditDependent>
    {
        public const int SEVIS_MAX_LENGTH = 11;
        public const int EMAIL_MAX_LENGTH = 255;
        public const int REMARKS_MAX_LENGTH = 500;

        public EditDependentValidator()
        {
            RuleFor(dependent => dependent.dependentSevisID).Length(0, SEVIS_MAX_LENGTH).WithMessage("Cancel Dependent: Sevis ID can be up to " + SEVIS_MAX_LENGTH.ToString() + " characters");
            When(student => student.fullName != null, () =>
            {
                RuleFor(student => student.fullName).SetValidator(new FullNameValidator());
            });
            RuleFor(student => student.BirthDate).NotNull().WithMessage("Edit Dependent: Date of birth is required");
            RuleFor(student => student.Gender).NotNull().WithMessage("Edit Dependent: Gender is required");
            RuleFor(student => student.BirthCountryCode).NotNull().WithMessage("Edit Dependent: Country of birth is required");
            RuleFor(student => student.CitizenshipCountryCode).NotNull().WithMessage("Edit Dependent: Country of citizenship is required");
            RuleFor(student => student.Email).Length(0, EMAIL_MAX_LENGTH).WithMessage("Edit Dependent: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("Dependent: Email is invalid");
            RuleFor(dependent => dependent.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Edit Dependent: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}