using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class UpdateExchVisitorValidator : AbstractValidator<UpdateExchVisitor>
    {
        public UpdateExchVisitorValidator()
        {
            RuleFor(visitor => visitor.visitor).NotNull().WithMessage("Visitor information is required").SetValidator(new ExchangeVisitorValidator());
        }
    }
}