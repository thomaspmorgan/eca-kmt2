using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CreateExchVisitorValidator : AbstractValidator<CreateExchVisitor>
    {
        public CreateExchVisitorValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(visitor => visitor.ExchangeVisitor).SetValidator(new ExchangeVisitorValidator()).When(update => update.ExchangeVisitor != null);
        }
    }
}