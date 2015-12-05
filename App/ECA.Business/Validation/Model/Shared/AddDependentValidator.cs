using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddDependentValidator : AbstractValidator<AddDependent>
    {
        public const int EMAIL_MAX_LENGTH = 255;
        public const int REMARKS_MAX_LENGTH = 500;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;

        public AddDependentValidator()
        {
            RuleFor(student => student.fullName).NotNull().WithMessage("Dependent: Full Name is required");
            RuleFor(student => student.fullName.LastName).NotNull().WithMessage("Dependent: Last Name is required");
            RuleFor(student => student.BirthDate).NotNull().WithMessage("Dependent: Date of birth is required");
            RuleFor(student => student.Gender).NotNull().WithMessage("Dependent: Gender is required");
            RuleFor(student => student.BirthCountryCode).NotNull().WithMessage("Dependent: Country of birth is required").Length(2);
            RuleFor(student => student.CitizenshipCountryCode).Length(2).WithMessage("Dependent: Country of citizenship is required");
            RuleFor(student => student.Email).Length(0, EMAIL_MAX_LENGTH).EmailAddress().WithMessage("Dependent: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.VisaType).NotNull().WithMessage("Dependent: Visa type is required").Length(2);
            RuleFor(student => student.Relationship).NotNull().WithMessage("Dependent: Relationship is required").Length(2);
            RuleFor(dependent => dependent.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Dependent: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Dependent: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(dependent => dependent.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Dependent: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
        }
    }
}