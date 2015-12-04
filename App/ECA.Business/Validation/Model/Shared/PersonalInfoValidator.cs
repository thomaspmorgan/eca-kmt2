using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class PersonalInfoValidator : AbstractValidator<PersonalInfo>
    {
        public const int EMAIL_MAX_LENGTH = 255;

        public PersonalInfoValidator()
        {
            RuleFor(student => student.fullName).NotNull().WithMessage("Personal Info: Full Name is required");
            When(student => student.fullName != null, () =>
            {
                RuleFor(student => student.fullName).SetValidator(new FullNameValidator());
            });
            RuleFor(student => student.BirthDate).NotNull().WithMessage("Personal Info: Date of birth is required");
            RuleFor(student => student.Gender).NotNull().Length(1).WithMessage("Personal Info: Gender is required");
            RuleFor(student => student.BirthCountryCode).NotNull().Length(2).WithMessage("Personal Info: Country of birth is required");
            RuleFor(student => student.CitizenshipCountryCode).NotNull().Length(2).WithMessage("Personal Info: Country of citizenship is required");
            RuleFor(student => student.Email).Length(0, EMAIL_MAX_LENGTH).EmailAddress().WithMessage("Personal Info: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.VisaType).NotNull().Length(2).WithMessage("Personal Info: Visa type is required");
        }

    }
}
