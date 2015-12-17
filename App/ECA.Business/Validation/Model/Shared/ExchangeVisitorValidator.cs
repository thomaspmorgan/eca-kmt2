using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ExchangeVisitorValidator : AbstractValidator<ExchangeVisitor>
    {
        public const int ID_MAX_LENGTH = 20;
        public const int SEVISID_MAX_LENGTH = 11;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;
        public const int POSITION_CODE_LENGTH = 3;
        public const int CATEGORY_CODE_LENGTH = 2;
        public const int OCCUPATION_CATEGORY_CODE_LENGTH = 2;
        public const int REMARKS_MAX_LENGTH = 500;

        public ExchangeVisitorValidator()
        {
            RuleFor(student => student.sevisID).NotNull().WithMessage("Visitor: Sevis ID is required").Length(1, SEVISID_MAX_LENGTH).WithMessage("Visitor: Sevis ID can be up to " + SEVISID_MAX_LENGTH.ToString() + " characters").When(student => student.isNew == false);
            RuleFor(student => student.requestID).NotNull().WithMessage("Student: Request ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: Request ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.userID).NotNull().WithMessage("Student: User ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: User ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.printForm).NotNull().WithMessage("Student: Print form option is required");
            RuleFor(student => student.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Student: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Student: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.biographical).NotNull().WithMessage("Student: Biographical Information is required").SetValidator(new PersonalInfoValidator()).When(student => student.requestID != null);
            RuleFor(student => student.positionCode).NotNull().WithMessage("Student: Position Code is required").Length(POSITION_CODE_LENGTH).WithMessage("Student: Position Code must be " + POSITION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.PrgStartDate).NotNull().WithMessage("Student: Program Start Date is required");
            RuleFor(student => student.PrgEndDate).NotNull().WithMessage("Student: Program End Date is required");
            RuleFor(student => student.categoryCode).NotNull().WithMessage("Student: Program Category is required").Length(1, CATEGORY_CODE_LENGTH).WithMessage("Student: Program category can be up to " + CATEGORY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.occupationCategoryCode).Length(OCCUPATION_CATEGORY_CODE_LENGTH).WithMessage("Student: Occupational Category Code must be " + OCCUPATION_CATEGORY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.subjectField).NotNull().WithMessage("Student: Subject or Field of Study is required").SetValidator(new SubjectFieldValidator()).When(student => student.requestID != null);
            RuleFor(student => student.usAddress).SetValidator(new USAddressValidator()).When(student => student.usAddress != null);
            RuleFor(student => student.mailAddress).SetValidator(new USAddressValidator()).When(student => student.mailAddress != null);
            RuleFor(student => student.financialInfo).NotNull().WithMessage("Student: Financial Information is required").SetValidator(new FinancialInfoValidator()).When(student => student.requestID != null);
            RuleFor(student => student.createDependent).SetValidator(new CreateDependentValidator()).When(student => student.createDependent != null);
            RuleFor(student => student.addSiteOfActivity).NotNull().WithMessage("Student: Site of activity address is required").SetValidator(new AddSiteOfActivityValidator()).When(student => student.requestID != null);
            RuleFor(student => student.residentialAddress).SetValidator(new ResidentialAddressValidator()).When(student => student.createDependent != null);
        }
    }
}