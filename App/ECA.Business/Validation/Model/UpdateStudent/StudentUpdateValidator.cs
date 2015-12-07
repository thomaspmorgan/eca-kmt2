using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class StudentUpdateValidator : AbstractValidator<StudentUpdate>
    {
        public const int SEVISID_MAX_LENGTH = 11;
        public const int REQUESTID_MAX_LENGTH = 20;
        public const int USERID_MAX_LENGTH = 10;
        public const int STATUSCODE_MAX_LENGTH = 10;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;
        
        public StudentUpdateValidator()
        {
            RuleFor(o => o.sevisID).NotNull().WithMessage("Student: Sevis ID is required").Length(1, SEVISID_MAX_LENGTH).WithMessage("Student: Sevis ID can be up to " + SEVISID_MAX_LENGTH.ToString() + " characters");
            RuleFor(o => o.requestID).NotNull().WithMessage("Student: Request ID is required").Length(1, REQUESTID_MAX_LENGTH).WithMessage("Student: Request ID can be up to " + REQUESTID_MAX_LENGTH.ToString() + " characters");
            RuleFor(o => o.userID).NotNull().WithMessage("Student: User ID is required").Length(1, USERID_MAX_LENGTH).WithMessage("Student: User ID can be up to " + USERID_MAX_LENGTH.ToString() + " characters");
            RuleFor(o => o.statusCode).NotNull().WithMessage("Student: Status Code is required").Length(1, STATUSCODE_MAX_LENGTH).WithMessage("Student: Status Code can be up to " + STATUSCODE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Student: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Student: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            When(student => student.sevisID != null, () => {
                RuleFor(student => student.authDropBelowFC).SetValidator(new AuthDropBelowFCValidator());
                RuleFor(student => student.cptEmployment).SetValidator(new CPTEmploymentValidator());
                RuleFor(student => student.updatedDependent).SetValidator(new UpdatedDependentValidator());
                RuleFor(student => student.disciplinaryAction).SetValidator(new DisciplinaryActionValidator());
                RuleFor(student => student.educationLevel).SetValidator(new EducationLevelValidator());
                RuleFor(student => student.financialInfo).SetValidator(new FinancialInfoValidator());
                RuleFor(student => student.offCampusEmployment).SetValidator(new OffCampusEmploymentValidator());
                RuleFor(student => student.optEmployment).SetValidator(new OPTEmploymentValidator());
                RuleFor(student => student.personalInfo).SetValidator(new StudentPersonalInfoValidator());
                RuleFor(student => student.program).SetValidator(new ProgramValidator());
                RuleFor(student => student.registration).SetValidator(new StudentRegistrationValidator());
                RuleFor(student => student.reprint).SetValidator(new StudentReprintValidator());
                RuleFor(student => student.request).SetValidator(new StudentRequestValidator());
                RuleFor(student => student.status).SetValidator(new StudentStatusValidator());
            });
        }
    }
}