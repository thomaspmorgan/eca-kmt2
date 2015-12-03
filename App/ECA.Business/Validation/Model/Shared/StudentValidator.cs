using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public const int ID_MAX_LENGTH = 20;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;
        public const int ISSUE_REASON_STRING_LENGTH = 1;
        public const int REMARKS_MAX_LENGTH = 500;

        public StudentValidator()
        {
            RuleFor(student => student.requestID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("Request ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.userID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.printForm).NotNull().WithMessage("Print form option is required");
            RuleFor(student => student.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.IssueReason).NotNull().Length(1, ISSUE_REASON_STRING_LENGTH).WithMessage("Issue Reason is required and can be up to " + ISSUE_REASON_STRING_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            When(student => student.requestID != null, () => {
                // personal info cannot be null
                RuleFor(student => student.personalInfo).NotNull().WithMessage("Personal Information cannot be null").SetValidator(new PersonalInfoValidator());
                // education info cannot be null
                RuleFor(student => student.educationalInfo).NotNull().WithMessage("Educational Information cannot be null").SetValidator(new EducationalInfoValidator());
                // financial info cannot be null
                RuleFor(student => student.financialInfo).NotNull().WithMessage("Financial Information cannot be null").SetValidator(new FinancialInfoValidator());
            });
            When(student => student.usAddress != null, () => {
                RuleFor(student => student.usAddress).SetValidator(new USAddressValidator());
            });
            When(student => student.isNew == true, () => {
                RuleFor(student => student.foreignAddress).NotNull().WithMessage("Foreign Address cannot be null").SetValidator(new ForeignAddressValidator());
            });
            When(student => student.createDependent != null, () => {
                RuleFor(student => student.createDependent).SetValidator(new CreateDependentValidator());
            });
        }
    }
}
