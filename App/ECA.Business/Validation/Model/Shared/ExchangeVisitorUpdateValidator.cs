using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class ExchangeVisitorUpdateValidator : AbstractValidator<ExchangeVisitorUpdate>
    {
        public const int ID_MAX_LENGTH = 20;
        public const int USERDEFINEDA_MAX_LENGTH = 10;
        public const int USERDEFINEDB_MAX_LENGTH = 14;

        public ExchangeVisitorUpdateValidator()
        {
            RuleFor(visitor => visitor.sevisID).NotNull().WithMessage("Student: Sevis ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: Sevis ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.requestID).NotNull().WithMessage("Student: Request ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: Request ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.userID).NotNull().WithMessage("Student: User ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Student: User ID can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Student: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Student: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Biographical).NotNull().WithMessage("Visitor: Biographical Information is required").SetValidator(new BiographicalValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.FinancialInfo).NotNull().WithMessage("Student: Financial Information is required").SetValidator(new FinancialInfoValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.Program).SetValidator(new ProgramValidator()).When(visitor => visitor.Program != null);
            RuleFor(visitor => visitor.Reprint).NotNull().WithMessage("Student: Foreign Address is required");
            RuleFor(visitor => visitor.SiteOfActivity).SetValidator(new SiteOfActivityUpdateValidator()).When(visitor => visitor.SiteOfActivity != null);
        }
    }
}