using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class StudentPersonalInfoValidator : AbstractValidator<StudentPersonalInfo>
    {
        public const int EMAIL_MAX_LENGTH = 255;
        public const int REMARKS_MAX_LENGTH = 500;

        public StudentPersonalInfoValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Personal Info: Print form option is required");
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
            When(student => student.usAddress != null, () => {
                RuleFor(student => student.usAddress).SetValidator(new USAddressValidator());
            });
            When(student => student.foreignAddress != null, () => {
                RuleFor(student => student.foreignAddress).SetValidator(new ForeignAddressValidator());
            });
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Personal Info: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}