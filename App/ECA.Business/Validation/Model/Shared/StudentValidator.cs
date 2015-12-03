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
            RuleFor(student => student.UserDefinedA).Length(1, USERDEFINEDA_MAX_LENGTH).WithMessage("User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(1, USERDEFINEDB_MAX_LENGTH).WithMessage("User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.IssueReason).NotNull().Length(1, ISSUE_REASON_STRING_LENGTH).WithMessage("Issue Reason is required and can be up to " + ISSUE_REASON_STRING_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(1, REMARKS_MAX_LENGTH).WithMessage("Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");





        }

    }
}
