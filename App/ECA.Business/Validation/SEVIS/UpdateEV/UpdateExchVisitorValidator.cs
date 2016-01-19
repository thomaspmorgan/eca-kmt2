using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class UpdateExchVisitorValidator : AbstractValidator<UpdateExchVisitor>
    {
        public UpdateExchVisitorValidator()
        {
            RuleFor(visitor => visitor.ExchangeVisitor).SetValidator(new ExchangeVisitorUpdateValidator()).When(update => update.ExchangeVisitor != null);
        }
    }
}