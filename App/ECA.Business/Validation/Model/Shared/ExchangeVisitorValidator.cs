using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ExchangeVisitorValidator : AbstractValidator<ExchangeVisitor>
    {
        public const int RID_MAX_LENGTH = 20;
        public const int UID_MAX_LENGTH = 10;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;
        public const int POSITION_CODE_LENGTH = 3;
        public const int CATEGORY_CODE_LENGTH = 2;
        public const int OCCUPATION_CATEGORY_CODE_LENGTH = 2;

        public ExchangeVisitorValidator()
        {
            RuleFor(visitor => visitor.requestID).NotNull().WithMessage("Exch. Visitor: Request ID is required").Length(1, RID_MAX_LENGTH).WithMessage("Exch. Visitor: Request ID can be up to " + RID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.userID).NotNull().WithMessage("Exch. Visitor: User ID is required").Length(1, UID_MAX_LENGTH).WithMessage("Exch. Visitor: User ID can be up to " + UID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Exch. Visitor: Print form option is required");
            RuleFor(visitor => visitor.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Exch. Visitor: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Exch. Visitor: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Biographical).NotNull().WithMessage("Exch. Visitor: Biographical Information is required").SetValidator(new BiographicalValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.PositionCode).NotNull().WithMessage("Exch. Visitor: Position Code is required").Length(POSITION_CODE_LENGTH).WithMessage("Exch. Visitor: Position Code must be " + POSITION_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PrgStartDate).NotNull().WithMessage("Exch. Visitor: Program Start Date is required");
            RuleFor(visitor => visitor.PrgEndDate).NotNull().WithMessage("Exch. Visitor: Program End Date is required");
            RuleFor(visitor => visitor.CategoryCode).NotNull().WithMessage("Exch. Visitor: Program Category is required").Length(1, CATEGORY_CODE_LENGTH).WithMessage("Exch. Visitor: Program category can be up to " + CATEGORY_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.OccupationCategoryCode).Length(0, OCCUPATION_CATEGORY_CODE_LENGTH).WithMessage("Exch. Visitor: Occupational Category Code must be " + OCCUPATION_CATEGORY_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SubjectField).NotNull().WithMessage("Exch. Visitor: Subject or Field of Study is required").SetValidator(new SubjectFieldValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.USAddress).SetValidator(new USAddressValidator()).When(visitor => visitor.USAddress != null);
            RuleFor(visitor => visitor.MailAddress).SetValidator(new USAddressValidator()).When(visitor => visitor.MailAddress != null);
            RuleFor(visitor => visitor.FinancialInfo).NotNull().WithMessage("Exch. Visitor: Financial Information is required").SetValidator(new FinancialInfoValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.CreateDependent).SetValidator(new CreateDependentValidator()).When(visitor => visitor.CreateDependent != null);
            RuleFor(visitor => visitor.AddSiteOfActivity).NotNull().WithMessage("Exch. Visitor: Site of activity address is required").SetValidator(new AddSiteOfActivityValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.ResidentialAddress).SetValidator(new ResidentialAddressValidator()).When(visitor => visitor.CreateDependent != null);
        }
    }
}