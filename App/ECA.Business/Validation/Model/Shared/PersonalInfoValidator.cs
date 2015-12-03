using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class PersonalInfoValidator : AbstractValidator<PersonalInfo>
    {
        public PersonalInfoValidator()
        {
            RuleFor(student => student.fullName).NotNull().WithMessage("Personal Info: Full Name is required");
            When(student => student.fullName != null, () =>
            {
                RuleFor(student => student.fullName).SetValidator(new FullNameValidator());
            });
            RuleFor(student => student.BirthDate).NotNull().WithMessage("Personal Info: Date of birth is required");
            RuleFor(student => student.Gender).NotNull().WithMessage("Personal Info: Gender is required");
            RuleFor(student => student.BirthCountryCode).NotNull().Length(2).WithMessage("Personal Info: Country of birth is required");
            RuleFor(student => student.CitizenshipCountryCode).NotNull().Length(2).WithMessage("Personal Info: Country of citizenship is required");
            RuleFor(student => student.Email).Length(0, 255).EmailAddress().WithMessage("Personal Info: Country of citizenship is required");
            RuleFor(student => student.VisaType).NotNull().Length(2).WithMessage("Personal Info: Visa type is required");
        }

    }
}
