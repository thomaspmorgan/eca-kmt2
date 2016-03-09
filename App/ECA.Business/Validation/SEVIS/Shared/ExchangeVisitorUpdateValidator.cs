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
            RuleFor(visitor => visitor.sevisID).NotNull().WithMessage("Exch. Visitor: Sevis ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Exch. Visitor: Sevis ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.requestID).NotNull().WithMessage("Exch. Visitor: Request ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Exch. Visitor: Request ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.userID).NotNull().WithMessage("Exch. Visitor: User ID is required").Length(1, ID_MAX_LENGTH).WithMessage("Exch. Visitor: User ID is required and can be up to " + ID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.UserDefinedA).Length(0, USERDEFINEDA_MAX_LENGTH).WithMessage("Exch. Visitor: User Defined A can be up to " + USERDEFINEDA_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.UserDefinedB).Length(0, USERDEFINEDB_MAX_LENGTH).WithMessage("Exch. Visitor: User Defined B can be up to " + USERDEFINEDB_MAX_LENGTH.ToString() + " characters");
            //RuleFor(visitor => visitor.Biographical).NotNull().WithMessage("Exch. Visitor: Biographical Information is required").SetValidator(new BiographicalValidator()).When(visitor => visitor.requestID != null);
            //RuleFor(visitor => visitor.FinancialInfo).NotNull().WithMessage("Exch. Visitor: Financial Information is required").SetValidator(new FinancialInfoValidator()).When(visitor => visitor.requestID != null);
            RuleFor(visitor => visitor.Program).SetValidator(new ProgramValidator()).When(visitor => visitor.Program != null);
            RuleFor(visitor => visitor.Reprint).NotNull().WithMessage("Exch. Visitor: Foreign Address is required");
        }
    }
}