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
            RuleFor(student => student.requestID).NotNull().WithMessage("Student: Request ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: Request ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.userID).NotNull().WithMessage("Student: User ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: User ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.printForm).NotNull().WithMessage("Student: Print form option is required");
            RuleFor(student => student.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Student: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Student: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Biographical).NotNull().WithMessage("Student: Biographical Information is required").SetValidator(new BiographicalValidator()).When(student => student.requestID != null);
            RuleFor(student => student.PositionCode).NotNull().WithMessage("Student: Position Code is required").Length(POSITION_CODE_LENGTH).WithMessage("Student: Position Code must be " + POSITION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.PrgStartDate).NotNull().WithMessage("Student: Program Start Date is required");
            RuleFor(student => student.PrgEndDate).NotNull().WithMessage("Student: Program End Date is required");
            RuleFor(student => student.CategoryCode).NotNull().WithMessage("Student: Program Category is required").Length(1, CATEGORY_CODE_LENGTH).WithMessage("Student: Program category can be up to " + CATEGORY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.OccupationCategoryCode).Length(OCCUPATION_CATEGORY_CODE_LENGTH).WithMessage("Student: Occupational Category Code must be " + OCCUPATION_CATEGORY_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.SubjectField).NotNull().WithMessage("Student: Subject or Field of Study is required").SetValidator(new SubjectFieldValidator()).When(student => student.requestID != null);
            RuleFor(student => student.USAddress).SetValidator(new USAddressValidator()).When(student => student.USAddress != null);
            RuleFor(student => student.MailAddress).SetValidator(new USAddressValidator()).When(student => student.MailAddress != null);
            RuleFor(student => student.FinancialInfo).NotNull().WithMessage("Student: Financial Information is required").SetValidator(new FinancialInfoValidator()).When(student => student.requestID != null);
            RuleFor(student => student.CreateDependent).SetValidator(new CreateDependentValidator()).When(student => student.CreateDependent != null);
            RuleFor(student => student.AddSiteOfActivity).NotNull().WithMessage("Student: Site of activity address is required").SetValidator(new AddSiteOfActivityValidator()).When(student => student.requestID != null);
            RuleFor(student => student.ResidentialAddress).SetValidator(new ResidentialAddressValidator()).When(student => student.CreateDependent != null);
        }
    }
}