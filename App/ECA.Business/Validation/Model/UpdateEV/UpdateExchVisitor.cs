using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    //[Validator(typeof(UpdateExchVisitorValidator))]
    public class UpdateExchVisitor
    {
        public ExchangeVisitorUpdate ExchangeVisitor { get; set; }
    }
}
