using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(UpdateExchVisitorValidator))]
    public class UpdateExchVisitor
    {
        public ExchangeVisitor visitor { get; set; }
    }
}
