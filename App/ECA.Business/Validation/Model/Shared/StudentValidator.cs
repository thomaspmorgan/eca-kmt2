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
            RuleFor(student => student.requestID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("Student: Request ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.userID).NotNull().Length(1, ID_MAX_LENGTH).WithMessage("Student: User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.printForm).NotNull().WithMessage("Student: Print form option is required");
            RuleFor(student => student.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Student: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Student: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.IssueReason).NotNull().WithMessage("Student: Issue Reason is required").Length(1, ISSUE_REASON_STRING_LENGTH).WithMessage("Student: Issue Reason can be up to " + ISSUE_REASON_STRING_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Student: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            When(student => student.requestID != null, () => {
                // personal info cannot be null
                RuleFor(student => student.personalInfo).NotNull().WithMessage("Student: Personal Information is required").SetValidator(new PersonalInfoValidator());
                // education info cannot be null
                RuleFor(student => student.educationalInfo).NotNull().WithMessage("Student: Educational Information is required").SetValidator(new EducationalInfoValidator());
                // financial info cannot be null
                RuleFor(student => student.financialInfo).NotNull().WithMessage("Student: Financial Information is required").SetValidator(new FinancialInfoValidator());
            });
            When(student => student.usAddress != null, () => {
                RuleFor(student => student.usAddress).SetValidator(new USAddressValidator());
            });
            When(student => student.isNew == true, () => {
                RuleFor(student => student.foreignAddress).NotNull().WithMessage("Student: Foreign Address is required").SetValidator(new ForeignAddressValidator());
            });
            When(student => student.createDependent != null, () => {
                RuleFor(student => student.createDependent).SetValidator(new CreateDependentValidator());
            });
        }
    }
}
